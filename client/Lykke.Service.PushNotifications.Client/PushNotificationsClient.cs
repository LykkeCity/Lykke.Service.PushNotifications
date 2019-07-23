using Lykke.HttpClientGenerator;

namespace Lykke.Service.PushNotifications.Client
{
    internal class PushNotificationsClient : IPushNotificationsClient
    {
        public INotificationsApi Notifications { get; }
        public IInstallationsApi Installations { get; }

        public PushNotificationsClient(IHttpClientGenerator httpClientGenerator)
        {
            Notifications = httpClientGenerator.Generate<INotificationsApi>();
            Installations = httpClientGenerator.Generate<IInstallationsApi>();
        }
    }
}
