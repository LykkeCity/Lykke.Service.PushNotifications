using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lykke.Service.PushNotifications.Contract.Enums;
using Lykke.Service.PushNotifications.Core.Services;

namespace Lykke.Service.PushNotifications.Models
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
