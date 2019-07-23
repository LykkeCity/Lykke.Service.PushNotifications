using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PushNotifications.Client.Models
{
    public class TextNotificationModel
    {
        [Required]
        public IEnumerable<string> NotificationIds { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
