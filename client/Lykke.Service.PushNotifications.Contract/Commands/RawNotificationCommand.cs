using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Service.PushNotifications.Contract.Enums;

namespace Lykke.Service.PushNotifications.Contract.Commands
{
    public class RawNotificationCommand
    {
        public string NotificationId { get; set; }
        public string Payload { get; set; }
        public MobileOs MobileOs { get; set; }
    }
}
