using System.Threading.Tasks;
using Lykke.Service.PushNotifications.Core.Domain;

namespace Lykke.Service.PushNotifications.Core.Services
{
    public interface ITagsService
    {
        Task<ITag[]> GetTagsAsync();
        Task<string[]> GetClientTagsAsync(string clientId);
        Task AddClientTagAsync(string clientId, string tag);
        Task CreateTagAsync(string tag);
        Task DeleteTagAsync(string tag);
    }
}
