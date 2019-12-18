using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.PushNotifications.Core.Domain
{
    public interface IInstallationsRepository
    {
        Task AddOrUpdateAsync(IInstallation installation);
        Task<IEnumerable<IInstallation>> GetByClientIdAsync(string clientId);
        Task<IInstallation> GetAsync(string clientId, string installationId);
        Task DisableAsync(string clientId, string installationId);
        Task DeleteAsync(string clientId, string installationId);
    }
}
