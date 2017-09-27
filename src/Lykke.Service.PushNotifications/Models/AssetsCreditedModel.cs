using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PushNotifications.Models
{
    public class AssetsCreditedModel
    {
        [Required]
        public IEnumerable<string> NotificationIds { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public string AssetId { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
