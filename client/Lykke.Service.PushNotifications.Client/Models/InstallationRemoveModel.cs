using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PushNotifications.Client.Models
{
    public class InstallationRemoveModel
    {
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string InstallationId { get; set; }
    }
}
