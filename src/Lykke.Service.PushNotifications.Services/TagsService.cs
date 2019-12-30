using System.Threading.Tasks;
using Lykke.Service.PushNotifications.Core.Domain;
using Lykke.Service.PushNotifications.Core.Services;

namespace Lykke.Service.PushNotifications.Services
{
    public class TagsService : ITagsService
    {
        private readonly ITagsRepository _tagsRepository;
        private readonly IClientTagsRepository _clientTagsRepository;

        public TagsService(
            ITagsRepository tagsRepository,
            IClientTagsRepository clientTagsRepository)
        {
            _tagsRepository = tagsRepository;
            _clientTagsRepository = clientTagsRepository;
        }

        public Task<ITag[]> GetTagsAsync()
        {
            return _tagsRepository.GetTagsAsync();
        }

        public Task<string[]> GetClientTagsAsync(string clientId)
        {
            return _clientTagsRepository.GetAsync(clientId);
        }

        public async Task AddClientTagAsync(string clientId, string tag)
        {
            var existingTag = await _tagsRepository.GetTagAsync(tag);

            if (existingTag == null)
                return;

            bool added = await _clientTagsRepository.AddAsync(clientId, tag);
            if (added)
                await _tagsRepository.IncrementCountAsync(tag);
        }

        public Task CreateTagAsync(string tag)
        {
            return _tagsRepository.AddAsync(tag);
        }

        public async Task DeleteTagAsync(string tag)
        {
            await _tagsRepository.DeleteAsync(tag);
            await _clientTagsRepository.DeleteByTagAsync(tag);
        }
    }
}
