using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lykke.Service.PushNotifications.Contract.Enums;
using Lykke.Service.PushNotifications.Core.Services;

namespace Lykke.Service.PushNotifications.Models
{
    public class DataNotificationModel
    {
        [Required]
        public IEnumerable<string> NotificationIds { get; set; }
        [Required]
        public NotificationType Type { get; set; }
        [Required]
        public string Entity { get; set; }
        public string Id { get; set; }
    }
}
