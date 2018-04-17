using Lykke.Service.PushNotifications.Core.Settings.ServiceSettings;
using Lykke.Service.PushNotifications.Core.Settings.SlackNotifications;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.PushNotifications.Core.Settings
{
    public class AppSettings
    {
        public PushNotificationsSettings PushNotificationsService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
        public TransportSettings Transports { get; set; }
    }

    public class TransportSettings
    {
        [AmqpCheck]
        public string ClientRabbitMqConnectionString { get; set; }
        [AmqpCheck]
        public string MeRabbitMqConnectionString { get; set; }
    }
}
