using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;

namespace Lykke.Service.PushNotifications.AzureRepositories
{
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateAlways)]
    public class FcmTokenEntity : AzureTableEntity
    {
        public string NotificationId { get; set; }
        public string ClientId { get; set; }
        public string SessionId { get; set; }
        public string FcmToken { get; set; }

        public static string GetPk(string notificationId) => notificationId;
        public static string GetSessionRk(string sessionId) => sessionId;

        public static FcmTokenEntity Create(string notificationId, string clientId, string sessionId, string fcmToken)
        {
            return new FcmTokenEntity
            {
                PartitionKey = GetPk(notificationId),
                RowKey = GetSessionRk(sessionId),
                NotificationId = notificationId,
                ClientId = clientId,
                SessionId = sessionId,
                FcmToken = fcmToken
            };
        }
    }
}
