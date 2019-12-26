using System;
using System.ComponentModel.DataAnnotations;
using Lykke.Service.PushNotifications.Contract.Enums;

namespace Lykke.Service.PushNotifications.Client.Models
{
    public class InstallationModel
    {
        public string InstallationId { get; set; }
        [Required]
        public string NotificationId { get; set; }
        [Required]
        public string ClientId { get; set; }
        [Required]
        public MobileOs Platform { get; set; }
        [Required]
        public string PushChannel { get; set; }
    }
}
