using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.PushNotifications.Contract.Commands
{
    public class AssetsCreditedCommand
    {
        public IEnumerable<string> NotificationIds { get; set; }
        public double Amount { get; set; }
        public string AssetId { get; set; }
        public string Message { get; set; }
    }
}
