using System.Collections.Generic;

namespace Lykke.Service.PushNotifications.Contract.Commands
{
    public class LimitOrderNotificationCommand
    {
        public IEnumerable<string> NotificationIds { get; set; }
        public string OrderType { get; set; }
        public string OrderStatus { get; set; }
        public string Message { get; set; }
    }
}
