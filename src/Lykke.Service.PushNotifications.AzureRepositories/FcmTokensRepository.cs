using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables.Templates.Index;
using Lykke.Service.PushNotifications.Core.Domain;

namespace Lykke.Service.PushNotifications.AzureRepositories
{
    public class FcmTokensRepository : IFcmTokensRepository
    {
        private readonly INoSQLTableStorage<FcmTokenEntity> _tableStorage;
        private readonly INoSQLTableStorage<AzureIndex> _index;

        public FcmTokensRepository(INoSQLTableStorage<FcmTokenEntity> tableStorage, INoSQLTableStorage<AzureIndex> index)
        {
            _tableStorage = tableStorage;
            _index = index;
        }

        public async Task AddAsync(string notificationId, string clientId, string sessionId, string token)
        {
            var entity = FcmTokenEntity.Create(notificationId, clientId, sessionId, token);
            var indexEntity = AzureIndex.Create(sessionId, $"{clientId}_{notificationId}" , entity);

            await Task.WhenAll(
                _tableStorage.InsertOrReplaceAsync(entity),
                _index.InsertOrMergeAsync(indexEntity)
            );
        }

        public async Task<string[]> GetAsync(string notificationId)
        {
            var entities = await _tableStorage.GetDataAsync(FcmTokenEntity.GetPk(notificationId));
            return entities.Select(x => x.FcmToken).Distinct().ToArray();
        }

        public async Task<string[]> GetAsync(string[] notificationIds)
        {
            var entities = await _tableStorage.GetDataAsync(notificationIds.Select(FcmTokenEntity.GetPk));
            return entities.Select(x => x.FcmToken).Distinct().ToArray();

        }

        public async Task DeleteBySessionIdAsync(string sessionId)
        {
            var indexes = await _index.GetDataAsync(sessionId);
            var entities = await _tableStorage.GetDataAsync(indexes);

            var tasks = new List<Task>();
            foreach (var entity in entities)
            {
                tasks.Add(_tableStorage.DeleteIfExistAsync(entity.PartitionKey, entity.RowKey));
                tasks.Add(_index.DeleteIfExistAsync(entity.SessionId, $"{entity.ClientId}_{entity.NotificationId}"));
            }

            await Task.WhenAll(tasks);
        }
    }
}
