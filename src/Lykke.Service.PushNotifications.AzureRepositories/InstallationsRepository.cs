using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables.Templates.Index;
using Lykke.Service.PushNotifications.Core.Domain;

namespace Lykke.Service.PushNotifications.AzureRepositories
{
    public class InstallationsRepository : IInstallationsRepository
    {
        private readonly INoSQLTableStorage<InstallationEntity> _tableStorage;
        private readonly INoSQLTableStorage<AzureIndex> _index;
        private const string ActiveIndex = "Active";

        public InstallationsRepository(
            INoSQLTableStorage<InstallationEntity> tableStorage,
            INoSQLTableStorage<AzureIndex> index)
        {
            _tableStorage = tableStorage;
            _index = index;
        }

        public async Task AddOrUpdateAsync(IInstallation installation)
        {
            var entity = InstallationEntity.Create(installation);
            await _tableStorage.InsertOrReplaceAsync(entity);

            var indexEntity = AzureIndex.Create(ActiveIndex, installation.InstallationId, entity);
            await _index.InsertOrMergeAsync(indexEntity);
        }

        public async Task<IEnumerable<IInstallation>> GetByClientIdAsync(string clientId)
        {
            return await _tableStorage.GetDataAsync(InstallationEntity.GeneratePk(clientId));
        }

        public async Task<IInstallation> GetAsync(string clientId, string installationId)
        {
            return await _tableStorage.GetDataAsync(InstallationEntity.GeneratePk(clientId), InstallationEntity.GenerateRk(installationId));
        }

        public Task DisableAsync(string clientId, string installationId)
        {
            return Task.WhenAll(
                _tableStorage.MergeAsync(InstallationEntity.GeneratePk(clientId),
                InstallationEntity.GenerateRk(installationId),
                entity =>
                {
                    entity.Enabled = false;
                    return entity;
                }),

                _index.DeleteIfExistAsync(ActiveIndex, installationId)
            );
        }

        public Task DeleteAsync(string clientId, string installationId)
        {
            return _tableStorage.DeleteIfExistAsync(InstallationEntity.GeneratePk(clientId), InstallationEntity.GenerateRk(installationId));
        }
    }
}
