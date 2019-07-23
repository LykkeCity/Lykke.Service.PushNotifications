using Newtonsoft.Json;

namespace Lykke.Service.PushNotifications.Services.Models.Ios
{
    public class IosMtPositionFields : IosFields
    {
        [JsonProperty("order")]
        public MtOrderModel Order { get; set; }
    }
}