namespace Lykke.Service.PushNotifications.Client.Models
{
    public class FcmTokenModel
    {
        public string NotificationId { get; set; }
        public string ClientId { get; set; }
        public string SessionId { get; set; }
        public string FcmToken { get; set; }
    }
}
