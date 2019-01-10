using Autofac;
using Common.Log;
using Lykke.Service.PushNotifications.Core.Services;
using Lykke.Service.PushNotifications.Services;
using Lykke.Service.PushNotifications.Settings.ServiceSettings;
using Lykke.SettingsReader;

namespace Lykke.Service.PushNotifications.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<PushNotificationsSettings> _settings;
        private readonly ILog _log;

        public ServiceModule(IReloadingManager<PushNotificationsSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;
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
        }
    }
}
