using Lykke.Service.PushNotifications.Core.Settings.ServiceSettings;
using Lykke.Service.PushNotifications.Core.Settings.SlackNotifications;

namespace Lykke.Service.PushNotifications.Core.Settings
{
    public class AppSettings
    {
        public PushNotificationsSettings PushNotificationsService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }
}
