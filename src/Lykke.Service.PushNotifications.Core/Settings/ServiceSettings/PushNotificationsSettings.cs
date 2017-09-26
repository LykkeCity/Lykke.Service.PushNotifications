namespace Lykke.Service.PushNotifications.Core.Settings.ServiceSettings
{
    public class PushNotificationsSettings
    {
        public DbSettings Db { get; set; }
        public string HubConnectionString { get; set; }
        public string HubName { get; set; }
    }
}