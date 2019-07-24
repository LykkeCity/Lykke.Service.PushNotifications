using Newtonsoft.Json;

namespace Lykke.Service.PushNotifications.Services.Models.Android
{
    public class AndroidPayloadFields
    {
        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("entity")]
        public string Entity { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}