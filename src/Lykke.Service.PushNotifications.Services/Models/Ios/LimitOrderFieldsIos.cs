using Lykke.Service.PushNotifications.Core.Domain;
using Newtonsoft.Json;

namespace Lykke.Service.PushNotifications.Services.Models.Ios
{
    public class LimitOrderFieldsIos : IosFields
    {
        [JsonProperty("orderType")]
        public OrderType OrderType { get; set; }
        [JsonProperty("orderStatus")]
        public OrderStatus OrderStatus { get; set; }
    }
}