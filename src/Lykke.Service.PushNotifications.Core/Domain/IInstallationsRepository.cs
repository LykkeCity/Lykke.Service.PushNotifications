using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.PushNotifications.Core.Domain
{
    public interface IInstallationsRepository
    {
        Task AddOrUpdateAsync(IInstallation installation);
        Task<IEnumerable<IInstallation>> GetByNotificationIdAsync(string notificationId);
        Task<IInstallation> GetAsync(string notificationId, string installationId);
        Task DeleteAsync(string notificationId, string installationId);
    }
}
