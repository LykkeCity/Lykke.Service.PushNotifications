﻿using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Common;
using Lykke.Common.ApiLibrary.Middleware;
using Lykke.Common.ApiLibrary.Swagger;
using Lykke.Logs;
using Lykke.Service.PushNotifications.Core.Services;
using Lykke.Service.PushNotifications.Modules;
using Lykke.Service.PushNotifications.Settings;
using Lykke.SettingsReader;
using Lykke.SlackNotification.AzureQueue;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.PushNotifications
{
    public class Startup
    {
        private const string _apiVersion = "v1";
        private const string _apiTitle = "PushNotifications API";

        public IHostingEnvironment Environment { get; }
        public IContainer ApplicationContainer { get; private set; }
        public IConfigurationRoot Configuration { get; }
        public ILog Log { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            Environment = env;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddMvc()
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.ContractResolver =
                            new Newtonsoft.Json.Serialization.DefaultContractResolver();
                    });

                services.AddSwaggerGen(options =>
                {
                    options.DefaultLykkeConfiguration(_apiVersion, _apiTitle);
                });

                var builder = new ContainerBuilder();
                var appSettings = Configuration.LoadSettings<AppSettings>(o =>
                {
                    o.SetConnString(s => s.SlackNotifications.AzureQueue.ConnectionString);
                    o.SetQueueName(s => s.SlackNotifications.AzureQueue.QueueName);
                    o.SenderName = $"{AppEnvironment.Name} {AppEnvironment.Version}";
                });
                Log = CreateLogWithSlack(services, appSettings);

                builder.RegisterModule(new ServiceModule(appSettings.Nested(x => x.PushNotificationsService), Log));
                builder.RegisterModule(new CqrsModule(appSettings, Log));

                builder.Populate(services);

                ApplicationContainer = builder.Build();

                return new AutofacServiceProvider(ApplicationContainer);
            }
            catch (Exception ex)
            {
                Log?.WriteFatalError(nameof(Startup), nameof(ConfigureServices), ex);
                throw;
            }
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            try
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseLykkeMiddleware("PushNotifications", ex => new {Message = "Technical problem"});

                app.UseMvc();
                app.UseSwagger(c =>
                {
                    c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
                });
                app.UseSwaggerUI(x =>
                {
                    x.RoutePrefix = "swagger/ui";
                    x.SwaggerEndpoint($"/swagger/{_apiVersion}/swagger.json", _apiVersion);

                    if (!string.IsNullOrWhiteSpace(_apiTitle))
                        x.DocumentTitle(_apiTitle);
                });
                app.UseStaticFiles();

                appLifetime.ApplicationStarted.Register(StartApplication);
                appLifetime.ApplicationStopping.Register(StopApplication);
                appLifetime.ApplicationStopped.Register(CleanUp);
            }
            catch (Exception ex)
            {
                Log?.WriteFatalError(nameof(Startup), nameof(ConfigureServices), ex);
                throw;
            }
        }

        private void StartApplication()
        {
            try
            {
                // NOTE: Service not yet recieve and process requests here

                ApplicationContainer.Resolve<IStartupManager>().StartAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Log?.WriteFatalError(nameof(Startup), nameof(StartApplication), ex);
                throw;
            }
        }

        private void StopApplication()
        {
            try
            {
                // NOTE: Service still can recieve and process requests here, so take care about it if you add logic here.

                ApplicationContainer.Resolve<IShutdownManager>().StopAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Log?.WriteFatalError(nameof(Startup), nameof(StopApplication), ex);
                throw;
            }
        }

        private void CleanUp()
        {
            try
            {
                // NOTE: Service can't recieve and process requests here, so you can destroy all resources

                ApplicationContainer.Dispose();
            }
            catch (Exception ex)
            {
                Log?.WriteFatalError(nameof(Startup), nameof(CleanUp), ex);
                throw;
            }
        }

        private static ILog CreateLogWithSlack(IServiceCollection services, IReloadingManager<AppSettings> settings)
        {
            var consoleLogger = new LogToConsole();
            var aggregateLogger = new AggregateLogger();

            aggregateLogger.AddLog(consoleLogger);

            // Creating slack notification service, which logs own azure queue processing messages to aggregate log
            var slackService = services.UseSlackNotificationsSenderViaAzureQueue(new AzureQueueIntegration.AzureQueueSettings
            {
                ConnectionString = settings.CurrentValue.SlackNotifications.AzureQueue.ConnectionString,
                QueueName = settings.CurrentValue.SlackNotifications.AzureQueue.QueueName
            }, aggregateLogger);

            var dbLogConnectionStringManager = settings.Nested(x => x.PushNotificationsService.Db.LogsConnString);
            var dbLogConnectionString = dbLogConnectionStringManager.CurrentValue;

            // Creating azure storage logger, which logs own messages to concole log
            if (!string.IsNullOrEmpty(dbLogConnectionString) && !(dbLogConnectionString.StartsWith("${") && dbLogConnectionString.EndsWith("}")))
            {
                const string appName = "Lykke.Service.PushNotifications";

                var persistenceManager = new LykkeLogToAzureStoragePersistenceManager(
                    appName,
                    AzureTableStorage<LogEntity>.Create(dbLogConnectionStringManager, "PushNotificationsLog", consoleLogger),
                    consoleLogger);

                var slackNotificationsManager = new LykkeLogToAzureSlackNotificationsManager(appName, slackService, consoleLogger);

                var azureStorageLogger = new LykkeLogToAzureStorage(
                    appName,
                    persistenceManager,
                    slackNotificationsManager,
                    consoleLogger);

                azureStorageLogger.Start();

                aggregateLogger.AddLog(azureStorageLogger);
            }

            return aggregateLogger;
        }
    }
}
