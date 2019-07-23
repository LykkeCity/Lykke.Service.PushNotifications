using Newtonsoft.Json;

namespace Lykke.Service.PushNotifications.Services.Models.Ios
{
    public class PushTxDialogFieldsIos : IosFields
    {
        [JsonProperty("amount")]
        public double Amount { get; set; }
        [JsonProperty("assetId")]
        public string AssetId { get; set; }
        [JsonProperty("addressFrom")]
        public string AddressFrom { get; set; }
        [JsonProperty("addressTo")]
        public string AddressTo { get; set; }
    }
}