using System;
using System.IO;
using Lykke.Common;
using Microsoft.AspNetCore.Hosting;

namespace Lykke.Service.PushNotifications
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine($"{AppEnvironment.Name} version {AppEnvironment.Version}");
#if DEBUG
            Console.WriteLine("Is DEBUG");
#else
            Console.WriteLine("Is RELEASE");
#endif
            Console.WriteLine($"ENV_INFO: {Environment.GetEnvironmentVariable("ENV_INFO")}");

            try
            {
                var hostBuilder = new WebHostBuilder()
                    .UseKestrel()
                    .UseUrls("http://*:5000")
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>();
#if !DEBUG
                hostBuilder = hostBuilder.UseApplicationInsights();
#endif
                var host = hostBuilder.Build();

                host.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal error:");
                Console.WriteLine(ex);
            }

            Console.WriteLine("Terminated");
        }
    }
}
