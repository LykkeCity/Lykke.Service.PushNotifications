using Newtonsoft.Json;

namespace Lykke.Service.PushNotifications.Services.Models
{
    public class MtOrderModel
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
    }
}