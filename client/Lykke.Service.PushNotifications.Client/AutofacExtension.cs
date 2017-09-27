using System;
using Autofac;
using Common.Log;

namespace Lykke.Service.PushNotifications.Client
{
    public static class AutofacExtension
    {
        public static void RegisterPushNotificationsClient(this ContainerBuilder builder, string serviceUrl)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (serviceUrl == null) throw new ArgumentNullException(nameof(serviceUrl));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));

            builder.RegisterInstance(new PushNotificationsClient(serviceUrl)).As<IPushNotificationsClient>().SingleInstance();
        }
    }
}
