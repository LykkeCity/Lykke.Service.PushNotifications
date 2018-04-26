using Lykke.Service.PushNotifications.Core.Settings.ServiceSettings;
using Lykke.Service.PushNotifications.Core.Settings.SlackNotifications;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.PushNotifications.Core.Settings
{
    public class AppSettings
    {
        public PushNotificationsSettings PushNotificationsService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
        public SagasRabbitMq SagasRabbitMq { get; set; }
    }

    public class SagasRabbitMq
    {
        [AmqpCheck]
        public string RabbitConnectionString { get; set; }
        
        public string RetryDelay { get; set; }
    }
}
