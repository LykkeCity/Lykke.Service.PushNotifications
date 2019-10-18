using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.PushNotifications.Contract.Enums;
using Lykke.Service.PushNotifications.Core.Domain;
using Lykke.Service.PushNotifications.Core.Services;
using Lykke.Service.PushNotifications.Services.Models;
using Lykke.Service.PushNotifications.Services.Models.Android;
using Lykke.Service.PushNotifications.Services.Models.Ios;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;
using Polly;

namespace Lykke.Service.PushNotifications.Services
{
    public class SrvAppNotifications : IAppNotifications
    {
        private readonly NotificationHubClient _notificationHubClient;
        private readonly ILog _log;

        public SrvAppNotifications(
            NotificationHubClient notificationHubClient,
            ILogFactory logFactory)
        {
            _notificationHubClient = notificationHubClient;
            _log = logFactory.CreateLog(this);
        }

        public Task SendDataNotificationToAllDevicesAsync(string[] notificationIds, string notificationType, string entity, string id = "")
        {
            if (!Enum.TryParse(notificationType, out NotificationType type))
                throw new InvalidOperationException($"{notificationType} is unknown");

            var apnsMessage = new IosNotification
            {
                Aps = new DataNotificationFields
                {
                    Type = type
                }
            };

            var gcmMessage = new AndoridPayloadNotification
            {
                Data = new AndroidPayloadFields
                {
                    Entity = EventsAndEntities.GetEntity(type),
                    Event = EventsAndEntities.GetEvent(type),
                    Id = id
                }
            };

            return Task.WhenAll(
                SendIosNotificationAsync(notificationIds, apnsMessage),
                SendAndroidNotificationAsync(notificationIds, gcmMessage)
            );
        }

        public Task SendTextNotificationAsync(string[] notificationIds, string notificationType, string message)
        {
            if (!Enum.TryParse(notificationType, out NotificationType type))
                throw new InvalidOperationException($"{notificationType} is unknown");

            var apnsMessage = new IosNotification
            {
                Aps = new IosFields
                {
                    Alert = message,
                    Type = type
                }
            };

            var gcmMessage = new AndoridPayloadNotification
            {
                Data = new AndroidPayloadFields
                {
                    Entity = EventsAndEntities.GetEntity(type),
                    Event = EventsAndEntities.GetEvent(type),
                    Message = message,
                }
            };

            return Task.WhenAll(
                SendIosNotificationAsync(notificationIds, apnsMessage),
                SendAndroidNotificationAsync(notificationIds, gcmMessage)
            );
        }

        public Task SendLimitOrderNotification(string[] notificationsIds, string message, string orderType,
            string orderStatus)
        {
            if (!Enum.TryParse(orderType, out OrderType type))
                throw new InvalidOperationException($"{orderType} is unknown");

            if (!Enum.TryParse(orderStatus, out OrderStatus status))
                throw new InvalidOperationException($"{orderStatus} is unknown");

            var apnsMessage = new IosNotification
            {
                Aps = new LimitOrderFieldsIos
                {
                    Alert = message,
                    Type = NotificationType.LimitOrderEvent,
                    OrderStatus = status,
                    OrderType = type
                }
            };

            var gcmMessage = new AndoridPayloadNotification
            {
                Data = new AndroidPayloadFields
                {
                    Entity = EventsAndEntities.GetEntity(NotificationType.LimitOrderEvent),
                    Event = EventsAndEntities.GetEvent(NotificationType.LimitOrderEvent),
                    Message = message,
                }
            };

            return Task.WhenAll(
                SendIosNotificationAsync(notificationsIds, apnsMessage),
                SendAndroidNotificationAsync(notificationsIds, gcmMessage)
            );
        }

        public Task SendMtOrderChangedNotification(string[] notificationIds, string notificationType, string message, string orderId)
        {
            if (!Enum.TryParse(notificationType, out NotificationType type))
                throw new InvalidOperationException($"{notificationType} is unknown");

            var mtOrder = !string.IsNullOrEmpty(orderId)
                ? new MtOrderModel {Id = orderId}
                : null;

            var apnsMessage = new IosNotification
            {
                Aps = new IosMtPositionFields
                {
                    Alert = message,
                    Type = type,
                    Order = mtOrder
                }
            };

            var gcmMessage = new AndoridPayloadNotification
            {
                Data = new AndroidMtPositionFields
                {
                    Entity = EventsAndEntities.GetEntity(type),
                    Event = EventsAndEntities.GetEvent(type),
                    Message = message,
                    Order = mtOrder
                }
            };

            return Task.WhenAll(
                SendIosNotificationAsync(notificationIds, apnsMessage),
                SendAndroidNotificationAsync(notificationIds, gcmMessage)
            );
        }

        public Task SendAssetsCreditedNotification(string[] notificationsIds, double amount, string assetId, string message)
        {
            var apnsMessage = new IosNotification
            {
                Aps = new AssetsCreditedFieldsIos
                {
                    Alert = message,
                    Amount = amount,
                    AssetId = assetId,
                    Type = NotificationType.AssetsCredited
                }
            };

            var gcmMessage = new AndoridPayloadNotification
            {
                Data = new AssetsCreditedFieldsAndroid
                {
                    Entity = EventsAndEntities.GetEntity(NotificationType.AssetsCredited),
                    Event = EventsAndEntities.GetEvent(NotificationType.AssetsCredited),
                    BalanceItem = new AssetsCreditedFieldsAndroid.BalanceItemModel
                    {
                        AssetId = assetId,
                        Amount = amount,
                    },
                    Message = message,
                }
            };

            return Task.WhenAll(
                SendIosNotificationAsync(notificationsIds, apnsMessage),
                SendAndroidNotificationAsync(notificationsIds, gcmMessage)
            );
        }

        public Task SendRawIosNotification(string notificationId, string payload)
        {
            return SendRawNotificationAsync(MobileOs.Ios, new[] { notificationId }, payload);
        }

        public Task SendRawAndroidNotification(string notificationId, string payload)
        {
            return SendRawNotificationAsync(MobileOs.Android, new[] { notificationId }, payload);
        }

        private Task SendIosNotificationAsync(string[] notificationIds, IIosNotification notification)
        {
            return SendRawNotificationAsync(MobileOs.Ios, notificationIds, notification.ToJson(ignoreNulls: true));
        }

        private Task SendAndroidNotificationAsync(string[] notificationIds, IAndroidNotification notification)
        {
            return SendRawNotificationAsync(MobileOs.Android, notificationIds, notification.ToJson(ignoreNulls: true));
        }

        private async Task SendRawNotificationAsync(MobileOs device, string[] notificationIds, string payload)
        {
            try
            {
                notificationIds = notificationIds?.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                if (notificationIds != null && notificationIds.Any())
                {
                    NotificationOutcome outcome;

                    _log.Info($"Sending notification to {string.Join(", ", notificationIds)}", context: payload.ToJson());

                    if (device == MobileOs.Ios)
                        outcome = await _notificationHubClient.SendAppleNativeNotificationAsync(payload, notificationIds);
                    else
                        outcome = await _notificationHubClient.SendFcmNativeNotificationAsync(payload, notificationIds);

#pragma warning disable 4014
                    // will only work from Standard tier for azure notification hub
                    if (!string.IsNullOrEmpty(outcome.NotificationId))
                        Task.Run(async () => await WaitForThePushStatusAsync(outcome.NotificationId));
#pragma warning restore 4014
                }
            }
            catch (Exception e)
            {
                _log.Error(e, context: new { ids = notificationIds, payload });
            }
        }

        public Task SendPushTxDialogAsync(string[] notificationsIds, double amount, string assetId, string addressFrom,
            string addressTo, string message)
        {
            var apnsMessage = new IosNotification
            {
                Aps = new PushTxDialogFieldsIos
                {
                    Alert = message,
                    Amount = amount,
                    AssetId = assetId,
                    Type = NotificationType.PushTxDialog,
                    AddressFrom = addressFrom,
                    AddressTo = addressTo
                }
            };

            var gcmMessage = new AndoridPayloadNotification
            {
                Data = new PushTxDialogFieldsAndroid
                {
                    Entity = EventsAndEntities.GetEntity(NotificationType.PushTxDialog),
                    Event = EventsAndEntities.GetEvent(NotificationType.PushTxDialog),
                    PushTxItem = new PushTxDialogFieldsAndroid.PushDialogTxItemModel
                    {
                        Amount = amount,
                        AssetId = assetId,
                        AddressFrom = addressFrom,
                        AddressTo = addressTo
                    },
                    Message = message,
                }
            };

            return Task.WhenAll(
                SendIosNotificationAsync(notificationsIds, apnsMessage),
                SendAndroidNotificationAsync(notificationsIds, gcmMessage)
            );
        }

        private async Task WaitForThePushStatusAsync(string notificationId)
        {
            NotificationDetails outcomeDetails = await Policy
                .Handle<MessagingEntityNotFoundException>()
                .OrResult<NotificationDetails>(x =>
                    x != null && (x.State == NotificationOutcomeState.Enqueued || x.State == NotificationOutcomeState.Processing))
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(1))
                .ExecuteAsync(async() => await _notificationHubClient.GetNotificationOutcomeDetailsAsync(notificationId));

            if (outcomeDetails?.State != NotificationOutcomeState.Completed)
            {
                _log.Warning("Failed to send push notification", context: outcomeDetails.ToJson());
            }
        }
    }
}
