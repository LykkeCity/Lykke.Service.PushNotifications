using System.Collections.Generic;

namespace Lykke.Service.PushNotifications.Contract.Commands
{
    public class MtOrderChangedNotificationCommand
    {
        public IEnumerable<string> NotificationIds { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string OrderId { get; set; }
    }
}
