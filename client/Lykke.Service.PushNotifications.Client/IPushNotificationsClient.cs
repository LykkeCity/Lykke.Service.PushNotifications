using JetBrains.Annotations;

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
    }
}
