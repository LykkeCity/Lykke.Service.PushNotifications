using System.Threading.Tasks;
using Lykke.Service.PushNotifications.Client.AutorestClient.Models;

namespace Lykke.Service.PushNotifications.Client
{
    public interface IPushNotificationsClient
    {
        Task SendDataNotificationToAllDevicesAsync(string[] notificationIds, NotificationType type, string entity, string id);
        Task SendTextNotificationAsync(string[] notificationsIds, NotificationType type, string message);
        Task SendPushTxDialogAsync(string[] notificationsIds, double amount, string assetId, string addressFrom,
            string addressTo, string message);
        Task SendAssetsCreditedNotificationAsync(string[] notificationsIds, double amount, string assetId, string message);
        Task SendRawIosNotificationAsync(string notificationId, string payload);
        Task SendRawAndroidNotificationAsync(string notificationId, string payload);
    }
}