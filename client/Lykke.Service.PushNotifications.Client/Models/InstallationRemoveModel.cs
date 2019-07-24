using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PushNotifications.Client.Models
{
    public class InstallationRemoveModel
    {
        [Required]
        public string NotificationId { get; set; }
        [Required]
        public string InstallationId { get; set; }
    }
}
