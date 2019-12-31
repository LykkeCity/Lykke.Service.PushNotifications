using JetBrains.Annotations;
using Lykke.Service.PushNotifications.Client.Models;

namespace Lykke.Service.PushNotifications.Client
{
    [PublicAPI]
    public interface IPushNotificationsClient
    {
        /// <summary>
        /// Api for notifications
        /// </summary>
        INotificationsApi Notifications { get; }

        /// <summary>
        /// Api for installations
        /// </summary>
        IInstallationsApi Installations { get; }

        /// <summary>
        /// Api for tags
        /// </summary>
        ITagsApi Tags { get; }
    }
}
