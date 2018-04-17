using Lykke.Service.PushNotifications.Contract.Enums;
using System.Collections.Generic;

namespace Lykke.Service.PushNotifications.Contract.Commands
{
    public class DataNotificationCommand
    {
        public IEnumerable<string> NotificationIds { get; set; }
        public NotificationType Type { get; set; }
        public string Entity { get; set; }
        public string Id { get; set; }
    }
}
