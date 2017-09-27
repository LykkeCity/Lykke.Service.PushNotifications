using System;
using System.Threading.Tasks;
using Lykke.Service.PushNotifications.AutorestClient;
using Lykke.Service.PushNotifications.AutorestClient.Models;

namespace Lykke.Service.PushNotifications.Client
{
    public class PushNotificationsClient : IPushNotificationsClient, IDisposable
    {
        private IPushNotificationsAPI _apiClient;

        public PushNotificationsClient(string serviceUrl)
        {
            _apiClient = new PushNotificationsAPI(new Uri(serviceUrl));
        }

        public void Dispose()
        {
            if (_apiClient == null)
                return;
            _apiClient.Dispose();
            _apiClient = null;
        }

        public async Task SendDataNotificationToAllDevicesAsync(string[] notificationIds, NotificationType type, string entity, string id)
        {
            var error = await _apiClient.SendDataNotificationToAllDevicesAsync(new DataNotificationModel
            {
                NotificationIds = notificationIds,
                Id = id,
                Type = type,
                Entity = entity
            });

            if (error != null)
            {
                throw new Exception(error.ErrorMessage);
            }
        }

        public async Task SendTextNotificationAsync(string[] notificationsIds, NotificationType type, string message)
        {
            var error = await _apiClient.SendTextNotificationAsync(new TextNotificationModel
            {
                NotificationIds = notificationsIds,
                Type = type,
                Message = message
            });

            if (error != null)
            {
                throw new Exception(error.ErrorMessage);
            }
        }

        public async Task SendPushTxDialogAsync(string[] notificationsIds, double amount, string assetId, string addressFrom,
            string addressTo, string message)
        {
            var error = await _apiClient.SendPushTxDialogAsync(new PushTxDialogModel
            {
                NotiicationIds = notificationsIds,
                Amount = amount,
                AssetId = assetId,
                Message = message,
                AddressTo = addressTo,
                AddressFrom = addressFrom
            });

            if (error != null)
            {
                throw new Exception(error.ErrorMessage);
            }
        }

        public async Task SendAssetsCreditedNotificationAsync(string[] notificationsIds, double amount, string assetId, string message)
        {
            var error = await _apiClient.SendAssetsCreditedNotificationAsync(new AssetsCreditedModel
            {
                NotificationIds = notificationsIds,
                Message = message,
                AssetId = assetId,
                Amount = amount
            });

            if (error != null)
            {
                throw new Exception(error.ErrorMessage);
            }
        }

        public async Task SendRawIosNotificationAsync(string notificationId, string payload)
        {
            var error = await _apiClient.SendRawNotificationAsync(new RawNotificationModel
            {
                NotificationId = notificationId,
                Payload = payload,
                MobileOs = MobileOs.Ios
            });

            if (error != null)
            {
                throw new Exception(error.ErrorMessage);
            }
        }

        public async Task SendRawAndroidNotificationAsync(string notificationId, string payload)
        {
            var error = await _apiClient.SendRawNotificationAsync(new RawNotificationModel
            {
                NotificationId = notificationId,
                Payload = payload,
                MobileOs = MobileOs.Android
            });

            if (error != null)
            {
                throw new Exception(error.ErrorMessage);
            }
        }
    }
}
