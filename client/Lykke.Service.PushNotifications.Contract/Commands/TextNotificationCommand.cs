﻿using Lykke.Service.PushNotifications.Contract.Enums;
using System.Collections.Generic;

namespace Lykke.Service.PushNotifications.Contract.Commands
{
    public class TextNotificationCommand
    {
        public IEnumerable<string> NotificationIds { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
    }
}
