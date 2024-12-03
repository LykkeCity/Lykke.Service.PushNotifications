﻿using System.Threading.Tasks;
using Antares.Sdk;

namespace Lykke.Service.PushNotifications
{
    internal sealed class Program
    {
        public static async Task Main(string[] args)
        {
#if DEBUG
            await LykkeStarter.Start<Startup>(true);
#else
            await LykkeStarter.Start<Startup>(false);
#endif
        }
    }
}
