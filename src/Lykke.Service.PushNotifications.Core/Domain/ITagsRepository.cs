using System.Threading.Tasks;

namespace Lykke.Service.PushNotifications.Core.Domain
{
    public interface ITagsRepository
    {
        Task<ITag[]> GetTagsAsync();
        Task<ITag> GetTagAsync(string tag);
        Task AddAsync(string tag);
        Task IncrementCountAsync(string tag);
        Task DeleteAsync(string tag);
    }
}
