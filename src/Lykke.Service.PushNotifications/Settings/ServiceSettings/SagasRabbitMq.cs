using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.PushNotifications.Settings.ServiceSettings
{
    public class SagasRabbitMq
    {
        [AmqpCheck]
        public string RabbitConnectionString { get; set; }
    }
}
