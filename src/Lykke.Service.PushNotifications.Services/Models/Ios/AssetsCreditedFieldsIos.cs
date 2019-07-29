using Newtonsoft.Json;

namespace Lykke.Service.PushNotifications.Services.Models.Ios
{
    public class AssetsCreditedFieldsIos : IosFields
    {
        [JsonProperty("amount")]
        public double Amount { get; set; }
        [JsonProperty("assetId")]
        public string AssetId { get; set; }
    }
}