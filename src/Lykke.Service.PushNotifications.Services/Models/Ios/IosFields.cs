using Lykke.Service.PushNotifications.Contract.Enums;
using Newtonsoft.Json;

namespace Lykke.Service.PushNotifications.Services.Models.Ios
{
    public class IosFields
    {
        [JsonProperty("alert")]
        public string Alert { get; set; }
        [JsonProperty("type")]
        public NotificationType Type { get; set; }
        [JsonProperty("sound")]
        public string Sound { get; set; } = "default";
    }
}