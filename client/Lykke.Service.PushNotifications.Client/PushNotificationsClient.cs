using System;
using Common.Log;

namespace Lykke.Service.PushNotifications.Client
{
    public class PushNotificationsClient : IPushNotificationsClient, IDisposable
    {
        private readonly ILog _log;

        public PushNotificationsClient(string serviceUrl, ILog log)
        {
            _log = log;
        }

        public void Dispose()
        {
            //if (_service == null)
            //    return;
            //_service.Dispose();
            //_service = null;
        }
    }
}
