using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.PushNotifications.Contract.Commands
{
    public class PushTxDialogCommand
    {
        public IEnumerable<string> NotiicationIds { get; set; }
        public double Amount { get; set; }
        public string AssetId { get; set; }
        public string AddressFrom { get; set; }
        public string AddressTo { get; set; }
        public string Message { get; set; }
    }
}
