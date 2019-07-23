using Lykke.Service.PushNotifications.Core.Domain;
using Newtonsoft.Json;

namespace Lykke.Service.PushNotifications.Services.Models.Android
{
    public class AndoridPayloadNotification : IAndroidNotification
    {
        [JsonProperty("data")]
        public AndroidPayloadFields Data { get; set; }
    }
}