using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Common;
using Common.Log;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Lykke.Common.Log;
using Lykke.Service.PushNotifications.Core.Domain;
using Lykke.Service.PushNotifications.Core.Services;

namespace Lykke.Service.PushNotifications.Services
{
    public class FirebasePushService : IFirebasePushService, IStartable, IDisposable
    {
        private readonly string _firebasePrivateKey;
        private readonly IFcmTokensRepository _fcmTokensRepository;
        private readonly ILog _log;
        private readonly bool _isActive;
        private FirebaseApp _firebaseApp;

        public FirebasePushService(
            string firebasePrivateKey,
            IFcmTokensRepository fcmTokensRepository,
            ILogFactory logFactory
            )
        {
            _firebasePrivateKey = firebasePrivateKey;
            _fcmTokensRepository = fcmTokensRepository;
            _log = logFactory.CreateLog(this);
            _isActive = !string.IsNullOrEmpty(firebasePrivateKey);
        }

        public async Task SendPushNotifications(string[] notificationsIds, string title, string message)
        {
            string traceLog = string.Empty;

            try
            {
                if (!_isActive)
                    return;

                var tokens = await _fcmTokensRepository.GetAsync(notificationsIds);

                if (!tokens.Any())
                {
                    _log.Info($"No tokens for tags = {string.Join(",", notificationsIds)}");
                    return;
                }

                var fcmMessage = new MulticastMessage
                {
                    Notification = new Notification
                    {
                        Title = title,
                        Body = message
                    },
                    Tokens = tokens
                };

                traceLog = fcmMessage.ToJson();

                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(fcmMessage);

                _log.Info($"Push notification for tags = {string.Join(",", notificationsIds)} is sent. SuccessCount: {response.SuccessCount}. FailureCount: {response.FailureCount}.");

                if (response.FailureCount > 0)
                {
                    var tokensList = tokens.ToJson();

                    foreach (var result in response.Responses.Where(r => r.IsSuccess == false))
                    {
                        _log.Warning("Fail push notification", result.Exception, context: $"tokens: {tokensList}, message: {result.Exception.Message}, error code: {result.Exception.MessagingErrorCode}");
                    }
                }

            }
            catch(Exception ex)
            {
                _log.Error(ex, "Cannot send push notification", context: $"tags: {string.Join(",", notificationsIds)}, fcmMessage: {traceLog}");
            }
        }

        public void Start()
        {
            if (!_isActive)
            {
                _log.Info("DISABLE FireBase application. Cannot send push notifications.");
                return;
            }

            _firebaseApp = FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromJson(_firebasePrivateKey)
            });

            var serviceAccountCredentials = (ServiceAccountCredential) _firebaseApp.Options.Credential.UnderlyingCredential;

            _log.Info($"FireBase application is ready to send push notifications. AppName: {_firebaseApp.Name}; ProjectId: {serviceAccountCredentials?.ProjectId}; Id: {serviceAccountCredentials?.Id}");
        }

        public void Dispose()
        {
            _firebaseApp?.Delete();
        }
    }
}
