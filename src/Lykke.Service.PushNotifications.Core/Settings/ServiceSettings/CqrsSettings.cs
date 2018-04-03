using System;
using JetBrains.Annotations;

namespace Lykke.Service.PushNotifications.Core.Settings.ServiceSettings
{
    public class CqrsSettings
    {
        [UsedImplicitly(ImplicitUseKindFlags.Assign)]
        public TimeSpan RetryDelay { get; set; }
    }
}
