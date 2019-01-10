using System.Collections.Generic;

namespace Lykke.Service.PushNotifications.Contract.Events
{
    public class DataNotificationEvent
    {
        public IEnumerable<string> NotificationIds { get; set; }
        public string Type { get; set; }
        public string Entity { get; set; }
        public string Id { get; set; }
    }
}
