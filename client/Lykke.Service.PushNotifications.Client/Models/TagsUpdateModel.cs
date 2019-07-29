using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PushNotifications.Client.Models
{
    public class TagsUpdateModel
    {
        [Required]
        public string NotificationId { get; set; }
        [Required]
        public string InstallationId { get; set; }
        public string[] Tags { get; set; }
    }
}
