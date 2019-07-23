using Autofac;
using AzureStorage.Tables;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Sdk;
using Lykke.Service.PushNotifications.AzureRepositories;
using Lykke.Service.PushNotifications.Core.Domain;
using Lykke.Service.PushNotifications.Core.Services;
using Lykke.Service.PushNotifications.Services;
using Lykke.Service.PushNotifications.Settings;
using Lykke.Service.PushNotifications.Settings.ServiceSettings;
using Lykke.SettingsReader;
using Microsoft.Azure.NotificationHubs;

namespace Lykke.Service.PushNotifications.Modules
{
    [UsedImplicitly]
    public class ServiceModule : Module
    {
        private readonly PushNotificationsSettings _settings;
        private readonly IReloadingManager<DbSettings> _dbSettings;

        public ServiceModule(IReloadingManager<AppSettings> appSettings)
        {
            _settings = appSettings.CurrentValue.PushNotificationsService;
            _dbSettings = appSettings.Nested(x => x.PushNotificationsService.Db);
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StartupManager>()
                .As<IStartupManager>()
                .SingleInstance();

            builder.RegisterType<SrvAppNotifications>()
                .As<IAppNotifications>()
                .SingleInstance();

            builder.Register(ctx =>
                new InstallationsRepository(
                    AzureTableStorage<InstallationEntity>.Create(_dbSettings.ConnectionString(x => x.DataConnString),
                        "Installations", ctx.Resolve<ILogFactory>()))
            ).As<IInstallationsRepository>().SingleInstance();

            builder.RegisterInstance(
                NotificationHubClient.CreateClientFromConnectionString(_settings.HubConnectionString,_settings.HubName)
            );
        }
    }
}
