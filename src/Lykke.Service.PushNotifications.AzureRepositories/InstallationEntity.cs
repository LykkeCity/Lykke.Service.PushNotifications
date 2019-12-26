using System;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.Serializers;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;
using Lykke.Service.PushNotifications.Contract.Enums;
using Lykke.Service.PushNotifications.Core.Domain;

namespace Lykke.Service.PushNotifications.AzureRepositories
{
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateAlways)]
    public class InstallationEntity : AzureTableEntity, IInstallation
    {
        public string ClientId { get; set; }
        public string NotificationId { get; set; }
        public string InstallationId { get; set; }
        public MobileOs Platform { get; set; }
        public DateTime LastUpdated { get; set; }
        [ValueSerializer(typeof(JsonStorageValueSerializer))]
        public string[] Tags { get; set; }
        public string PushChannel { get; set; }

        public static string GeneratePk(string clientId) => clientId;
        public static string GenerateRk(string installationId) => installationId;

        public static InstallationEntity Create(IInstallation installation)
        {
            return new InstallationEntity
            {
                PartitionKey = GeneratePk(installation.ClientId),
                RowKey = GenerateRk(installation.InstallationId),
                ClientId = installation.ClientId,
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
