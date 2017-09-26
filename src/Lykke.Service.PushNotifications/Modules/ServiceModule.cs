using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Log;
using Lykke.Service.PushNotifications.Core.Services;
using Lykke.Service.PushNotifications.Core.Settings.ServiceSettings;
using Lykke.Service.PushNotifications.Services;
using Lykke.SettingsReader;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.PushNotifications.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<PushNotificationsSettings> _settings;
        private readonly ILog _log;
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies
        private readonly IServiceCollection _services;

        public ServiceModule(IReloadingManager<PushNotificationsSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;

            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            builder.Register<IAppNotifications>(x =>
                new SrvAppNotifications(_settings.CurrentValue.HubConnectionString, _settings.CurrentValue.HubName));

            builder.Populate(_services);
        }
    }
}
