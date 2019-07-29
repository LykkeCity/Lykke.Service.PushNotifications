using Newtonsoft.Json;

namespace Lykke.Service.PushNotifications.Services.Models.Android
{
    public class AssetsCreditedFieldsAndroid : AndroidPayloadFields
    {
        public class BalanceItemModel
        {
            [JsonProperty("amount")]
            public double Amount { get; set; }
            [JsonProperty("assetId")]
            public string AssetId { get; set; }
        }

        [JsonProperty("balanceItem")]
        public BalanceItemModel BalanceItem { get; set; }
    }
}