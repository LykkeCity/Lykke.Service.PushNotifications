using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.PushNotifications.Settings.ServiceSettings
{
    [UsedImplicitly]
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }
        [AzureTableCheck]
        public string DataConnString { get; set; }
    }
}
