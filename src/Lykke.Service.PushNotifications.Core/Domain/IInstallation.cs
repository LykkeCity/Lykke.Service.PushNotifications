using System;
using Lykke.Service.PushNotifications.Contract.Enums;

namespace Lykke.Service.PushNotifications.Core.Domain
{
    public interface IInstallation
    {
        string ClientId { get; }
        string NotificationId { get; }
        string InstallationId { get; }
        MobileOs Platform { get; }
        string PushChannel { get; }
        DateTime LastUpdated { get; }
        string[] Tags { get; }
    }
}
