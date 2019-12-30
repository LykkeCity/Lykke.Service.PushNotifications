using Lykke.HttpClientGenerator;
using Lykke.Service.PushNotifications.Client.Models;

namespace Lykke.Service.PushNotifications.Client
{
    internal class PushNotificationsClient : IPushNotificationsClient
    {
        public INotificationsApi Notifications { get; }
        public IInstallationsApi Installations { get; }
        public ITagsApi Tags { get; set; }

        public PushNotificationsClient(IHttpClientGenerator httpClientGenerator)
        {
            Notifications = httpClientGenerator.Generate<INotificationsApi>();
            Installations = httpClientGenerator.Generate<IInstallationsApi>();
            Tags = httpClientGenerator.Generate<ITagsApi>();
        }
    }
}
