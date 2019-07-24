using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PushNotifications.Client.Models
{
    public class DataNotificationModel
    {
        [Required]
        public IEnumerable<string> NotificationIds { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Entity { get; set; }
        public string Id { get; set; }
    }
}
