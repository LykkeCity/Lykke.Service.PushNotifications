using System;
using Autofac;
using Lykke.HttpClientGenerator.Infrastructure;

namespace Lykke.Service.PushNotifications.Client
{
    public static class AutofacExtension
    {
        /// <summary>
        /// Registers push notifications client
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="serviceUrl"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void RegisterPushNotificationsClient(this ContainerBuilder builder, string serviceUrl)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));

            builder.RegisterInstance(
                    new PushNotificationsClient(HttpClientGenerator.HttpClientGenerator.BuildForUrl(serviceUrl)
                        .WithAdditionalCallsWrapper(new ExceptionHandlerCallsWrapper())
                        .WithoutRetries()
                        .Create())
                )
                .As<IPushNotificationsClient>()
                .SingleInstance();
        }

        /// <summary>
        /// Registers push notifications client
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="settings"></param>
        public static void RegisterClientDialogsClient(this ContainerBuilder builder, PushNotificationsServiceClientSettings settings)
        {
            builder.RegisterPushNotificationsClient(settings?.ServiceUrl);
        }
    }
}
