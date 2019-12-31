using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.PushNotifications.Core.Domain;

namespace Lykke.Service.PushNotifications.AzureRepositories
{
    public class TagsRepository: ITagsRepository
    {
        private readonly INoSQLTableStorage<TagEntity> _tableStorage;

        public TagsRepository(INoSQLTableStorage<TagEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<ITag[]> GetTagsAsync()
        {
            return (await _tableStorage.GetDataAsync(TagEntity.GeneratePk())).ToArray();
        }

        public async Task<ITag> GetTagAsync(string tag)
        {
            return await _tableStorage.GetDataAsync(TagEntity.GeneratePk(), tag);
        }

        public Task AddAsync(string tag)
        {
            return _tableStorage.InsertOrMergeAsync(TagEntity.Create(tag));
        }

        public Task IncrementCountAsync(string tag)
        {
            return _tableStorage.MergeAsync(TagEntity.GeneratePk(), tag, entity =>
            {
                entity.Count++;
                return entity;
            });
        }

        public Task DeleteAsync(string tag)
        {
            return _tableStorage.DeleteIfExistAsync(TagEntity.GeneratePk(), tag);
        }
    }
}
