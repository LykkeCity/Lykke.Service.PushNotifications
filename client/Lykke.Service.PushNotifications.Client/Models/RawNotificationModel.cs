using System.ComponentModel.DataAnnotations;
using Lykke.Service.PushNotifications.Contract.Enums;

namespace Lykke.Service.PushNotifications.Client.Models
{
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
