using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables.Templates.Index;
using Lykke.Service.PushNotifications.Core.Domain;

namespace Lykke.Service.PushNotifications.AzureRepositories
{
    public class ClientTagsRepository : IClientTagsRepository
    {
        private readonly INoSQLTableStorage<ClientTagEntity> _tableStorage;
        private readonly INoSQLTableStorage<AzureIndex> _index;

        public ClientTagsRepository(
            INoSQLTableStorage<ClientTagEntity> tableStorage,
            INoSQLTableStorage<AzureIndex> index)
        {
            _tableStorage = tableStorage;
            _index = index;
        }

        public async Task<bool> AddAsync(string clientId, string tag)
        {
            var entity = ClientTagEntity.Create(clientId, tag);
            var index = AzureIndex.Create(tag, $"{tag}_{clientId}", entity);

            var added = await _tableStorage.TryInsertAsync(entity);

            if (added)
            {
                await _index.TryInsertAsync(index);
            }

            return added;
        }

        public async Task<string[]> GetAsync(string clientId)
        {
            var entities = await _tableStorage.GetDataAsync(clientId);
            return entities.Select(x => x.RowKey).ToArray();
        }

        public async Task DeleteByTagAsync(string tag)
        {
            var indexes = (await _index.GetDataAsync(tag)).ToList();

            var entities = await _tableStorage.GetDataAsync(indexes);
            await Task.WhenAll(
                _tableStorage.DeleteAsync(entities),
                _index.DeleteAsync(indexes)
                );
        }
    }
}
