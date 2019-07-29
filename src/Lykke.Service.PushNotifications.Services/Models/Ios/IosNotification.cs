using Lykke.Service.PushNotifications.Core.Domain;
using Newtonsoft.Json;

namespace Lykke.Service.PushNotifications.Services.Models.Ios
{
    public class IosNotification : IIosNotification
    {
        [JsonProperty("aps")]
        public IosFields Aps { get; set; }
    }
}