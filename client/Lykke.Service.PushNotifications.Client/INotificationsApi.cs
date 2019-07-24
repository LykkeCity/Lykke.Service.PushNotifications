using System.Threading.Tasks;
using Lykke.Service.PushNotifications.Client.Models;
using Refit;

namespace Lykke.Service.PushNotifications.Client
{
    public interface INotificationsApi
    {
        [Post("/api/AppNotifications/SendDataNotificationToAllDevices")]
        Task SendDataNotificationToAllDevicesAsync([Body] DataNotificationModel model);
        [Post("/api/AppNotifications/SendTextNotification")]
        Task SendTextNotificationAsync([Body] TextNotificationModel model);
        [Post("/api/AppNotifications/SendPushTxDialog")]
        Task SendPushTxDialogAsync([Body] PushTxDialogModel model);
        [Post("/api/AppNotifications/SendAssetsCreditedNotification")]
        Task SendAssetsCreditedNotificationAsync([Body] AssetsCreditedModel model);
        [Post("/api/AppNotifications/SendRawNotification")]
        Task SendRawNotificationAsync([Body] RawNotificationModel model);
    }
}
