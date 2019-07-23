using System;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.Serializers;
using Lykke.Service.PushNotifications.Contract.Enums;
using Lykke.Service.PushNotifications.Core.Domain;

namespace Lykke.Service.PushNotifications.AzureRepositories
{
    public class InstallationEntity : AzureTableEntity, IInstallation
    {
        public string NotificationId { get; set; }
        public string InstallationId { get; set; }
        public MobileOs Platform { get; set; }
        public DateTime LastUpdated { get; set; }
        [ValueSerializer(typeof(JsonStorageValueSerializer))]
        public string[] Tags { get; set; }

        public string PushChannel { get; set; }

        public static InstallationEntity Create(IInstallation installation)
        {
            return new InstallationEntity
            {
                PartitionKey = installation.NotificationId,
                RowKey = installation.InstallationId,
                NotificationId = installation.NotificationId,
                InstallationId = installation.InstallationId,
                PushChannel = installation.PushChannel,
                Platform = installation.Platform,
                Tags = installation.Tags,
                LastUpdated = DateTime.UtcNow
            };
        }
    }
}
