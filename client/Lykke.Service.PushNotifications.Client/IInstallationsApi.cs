using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.PushNotifications.Client.Models;
using Lykke.Service.PushNotifications.Contract;
using Refit;

namespace Lykke.Service.PushNotifications.Client
{
    public interface IInstallationsApi
    {
        [Post("/api/Installations/register")]
        Task RegisterAsync([Body] InstallationModel model);

        [Post("/api/Installations/remove")]
        Task RemoveAsync([Body] InstallationRemoveModel model);

        [Get("/api/Installations/{notificationId}")]
        Task<IReadOnlyList<DeviceInstallation>> GetByNotificationIdAsync(string notificationId);

        [Post("/api/Installations/tags/add")]
        Task AddTagsAsync([Body] TagsUpdateModel model);

        [Post("/api/Installations/tags/remove")]
        Task RemoveTagsAsync([Body] TagsUpdateModel model);
    }
}
