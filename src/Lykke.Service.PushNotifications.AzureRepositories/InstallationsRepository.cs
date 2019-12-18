using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.PushNotifications.Core.Domain;

namespace Lykke.Service.PushNotifications.AzureRepositories
{
    public class InstallationsRepository : IInstallationsRepository
    {
        private readonly INoSQLTableStorage<InstallationEntity> _tableStorage;

        public InstallationsRepository(INoSQLTableStorage<InstallationEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task AddOrUpdateAsync(IInstallation installation)
        {
            var entity = InstallationEntity.Create(installation);
            return _tableStorage.InsertOrReplaceAsync(entity);
        }

        public async Task<IEnumerable<IInstallation>> GetByClientIdAsync(string clientId)
        {
            return await _tableStorage.GetDataAsync(InstallationEntity.GeneratePk(clientId));
        }

        public async Task<IInstallation> GetAsync(string clientId, string installationId)
        {
            return await _tableStorage.GetDataAsync(InstallationEntity.GeneratePk(clientId), InstallationEntity.GenerateRk(installationId));
        }

        public Task DeleteAsync(string clientId, string installationId)
        {
            return _tableStorage.DeleteIfExistAsync(InstallationEntity.GeneratePk(clientId), InstallationEntity.GenerateRk(installationId));
        }
    }
}
