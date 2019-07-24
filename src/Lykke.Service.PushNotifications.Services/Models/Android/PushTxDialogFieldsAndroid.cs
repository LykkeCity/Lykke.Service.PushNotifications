using Newtonsoft.Json;

namespace Lykke.Service.PushNotifications.Services.Models.Android
{
    public class PushTxDialogFieldsAndroid : AndroidPayloadFields
    {
        public class PushDialogTxItemModel
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

        [JsonProperty("pushTxItem")]
        public PushDialogTxItemModel PushTxItem { get; set; }
    }
}