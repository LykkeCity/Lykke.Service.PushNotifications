using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.PushNotifications.Client.Models;
using Lykke.Service.PushNotifications.Contract;
using Refit;

namespace Lykke.Service.PushNotifications.Client
{
    public interface IInstallationsApi
    {
        [Post("/api/Installations")]
        Task RegisterAsync([Body] InstallationModel model);

        [Delete("/api/Installations")]
        Task RemoveAsync([Body] InstallationRemoveModel model);

        [Get("/api/Installations/{clientId}")]
        Task<IReadOnlyList<DeviceInstallation>> GetByClientIdAsync(string clientId);

        [Post("/api/Installations/tags")]
        Task AddTagsAsync([Body] TagsUpdateModel model);

        [Delete("/api/Installations/tags")]
        Task RemoveTagsAsync([Body] TagsUpdateModel model);
    }
}
