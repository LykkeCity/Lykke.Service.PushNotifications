using JetBrains.Annotations;
using Lykke.Sdk.Settings;
using Lykke.Service.PushNotifications.Settings.ServiceSettings;

namespace Lykke.Service.PushNotifications.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public PushNotificationsSettings PushNotificationsService { get; set; }
        public SagasRabbitMq SagasRabbitMq { get; set; }
    }
}
