using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PushNotifications.Models
{
    public enum MobileOs
    {
        Ios,
        Android
    }

    public class RawNotificationModel
    {
        [Required]
        public string NotificationId { get; set; }
        [Required]
        public string Payload { get; set; }
        [Required]
        public MobileOs MobileOs { get; set; }
    }
}
