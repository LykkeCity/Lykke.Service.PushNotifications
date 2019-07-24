using Newtonsoft.Json;

namespace Lykke.Service.PushNotifications.Services.Models.Ios
{
    public class DataNotificationFields : IosFields
    {
        [JsonProperty("content-available")]
        public int ContentAvailable { get; set; } = 1;
    }
}