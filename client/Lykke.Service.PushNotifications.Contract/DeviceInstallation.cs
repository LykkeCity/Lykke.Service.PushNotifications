using System;
using Lykke.Service.PushNotifications.Contract.Enums;

namespace Lykke.Service.PushNotifications.Contract
{
    public class DeviceInstallation
    {
        public string NotificationId { get; set; }
        public string InstallationId { get; set; }
        public MobileOs Platform { get; set; }
        public string PushChannel { get; set; }
        public DateTime LastUpdated { get; set; }
        public string[] Tags { get; set; }
    }
}
