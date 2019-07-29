using JetBrains.Annotations;

namespace Lykke.Service.PushNotifications.Settings.ServiceSettings
{
    [UsedImplicitly]
    public class PushNotificationsSettings
    {
        public DbSettings Db { get; set; }
        public string HubConnectionString { get; set; }
        public string HubName { get; set; }
    }
}
