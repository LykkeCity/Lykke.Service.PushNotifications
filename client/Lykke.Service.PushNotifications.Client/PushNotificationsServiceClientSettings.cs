using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.PushNotifications.Client
{
    public class PushNotificationsServiceClientSettings
    {
        [HttpCheck("api/isalive")]
        public string ServiceUrl {get; set;}
    }
}
