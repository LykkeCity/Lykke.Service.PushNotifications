using Newtonsoft.Json;

namespace Lykke.Service.PushNotifications.Services.Models.Android
{
    public class AndroidMtPositionFields : AndroidPayloadFields
    {
        [JsonProperty("order")]
        public MtOrderModel Order { get; set; }
    }
}