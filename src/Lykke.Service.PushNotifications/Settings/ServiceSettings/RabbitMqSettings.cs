namespace Lykke.Service.PushNotifications.Settings.ServiceSettings
{
    public class RabbitMqSettings
    {
        public RabbitMqConnection AntaresSessions { get; set; }
    }

    public class RabbitMqConnection
    {
        public string ConnectionString { get; set; }
        public string ExchangeName { get; set; }
    }
}
