using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.PushNotifications.Core.Domain
{
    public interface IClientTagsRepository
    {
        Task<bool> AddAsync(string clientId, string tag);
        Task<string[]> GetAsync(string clientId);
        Task DeleteByTagAsync(string tag);
    }
}
