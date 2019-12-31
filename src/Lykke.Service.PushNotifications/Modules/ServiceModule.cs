using Autofac;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
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
                        "Installations", ctx.Resolve<ILogFactory>())
                    )
            ).As<IInstallationsRepository>().SingleInstance();

            builder.Register(ctx =>
                new TagsRepository(
                    AzureTableStorage<TagEntity>.Create(_dbSettings.ConnectionString(x => x.DataConnString),
                        "InstallationTags", ctx.Resolve<ILogFactory>())
                )
            ).As<ITagsRepository>().SingleInstance();

            builder.Register(ctx =>
                new ClientTagsRepository(
                    AzureTableStorage<ClientTagEntity>.Create(_dbSettings.ConnectionString(x => x.DataConnString),
                        "ClientTags", ctx.Resolve<ILogFactory>()),
                    AzureTableStorage<AzureIndex>.Create(_dbSettings.ConnectionString(x => x.DataConnString),
                        "ClientTags", ctx.Resolve<ILogFactory>())
                )
            ).As<IClientTagsRepository>().SingleInstance();

            builder.RegisterInstance(
                NotificationHubClient.CreateClientFromConnectionString(_settings.HubConnectionString,_settings.HubName)
            );

            builder.RegisterType<InstallationsService>()
                .As<IInstallationsService>()
                .SingleInstance();

            builder.RegisterType<TagsService>()
                .As<ITagsService>()
                .SingleInstance();
        }
    }
}
