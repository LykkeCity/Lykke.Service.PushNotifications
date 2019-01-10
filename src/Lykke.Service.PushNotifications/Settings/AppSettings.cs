using Lykke.Service.PushNotifications.Settings.ServiceSettings;
using Lykke.Service.PushNotifications.Settings.SlackNotifications;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.PushNotifications.Settings
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
