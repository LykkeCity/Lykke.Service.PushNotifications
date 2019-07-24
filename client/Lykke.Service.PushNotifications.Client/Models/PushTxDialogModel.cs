using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PushNotifications.Client.Models
{
    public class PushTxDialogModel
    {
        [Required]
        public IEnumerable<string> NotiicationIds { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public string AssetId { get; set; }
        [Required]
        public string AddressFrom { get; set; }
        [Required]
        public string AddressTo { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
