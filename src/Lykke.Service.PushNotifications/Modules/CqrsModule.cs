using Autofac;
using Common.Log;
using Lykke.Cqrs;
using Lykke.Cqrs.Configuration;
using Lykke.Messaging;
using Lykke.Messaging.RabbitMq;
using Lykke.Service.PushNotifications.Contract;
using Lykke.Service.PushNotifications.Core.Settings;
using Lykke.Service.PushNotifications.Utils;
using Lykke.Service.PushNotifications.Workflow.CommandHandlers;
using Lykke.SettingsReader;
using System.Collections.Generic;
using System.Linq;

namespace Lykke.Service.PushNotifications.Modules
{
    public class CqrsModule : Module
    {
        public static readonly string Self = PushNotificationsBoundedContext.Name;

        private readonly IReloadingManager<AppSettings> _settings;
        private readonly ILog _log;

        public CqrsModule(IReloadingManager<AppSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            string commandsRoute = "commands";
            string eventsRoute = "events";
            Messaging.Serialization.MessagePackSerializerFactory.Defaults.FormatterResolver = MessagePack.Resolvers.ContractlessStandardResolver.Instance;
            var rabbitMqMeSettings = new RabbitMQ.Client.ConnectionFactory { Uri = _settings.CurrentValue.PushNotificationsService.Cqrs.RabbitConnectionString };

            builder.Register(context => new AutofacDependencyResolver(context)).As<IDependencyResolver>();
            builder.RegisterType<NotificationCommandsHandler>().SingleInstance();

            #region ME_Rabbit

            var messagingEngineMeRabbit = new MessagingEngine(_log,
                new TransportResolver(new Dictionary<string, TransportInfo>
                {
                    {"RabbitMq", new TransportInfo(rabbitMqMeSettings.Endpoint.ToString(), rabbitMqMeSettings.UserName, rabbitMqMeSettings.Password, "None", "RabbitMq")}
                }),
                new RabbitMqTransportFactory());

            //Get all types from Commands namespace
            var pushNotificationsCommands = typeof(PushNotificationsBoundedContext).Assembly
                                            .GetTypes()
                                            .Where(x => x.Namespace == "Lykke.Service.PushNotifications.Contract.Commands")
                                            .ToArray();

            builder.Register(ctx =>
            {
                return new CqrsEngine(_log,
                    ctx.Resolve<IDependencyResolver>(),
                    messagingEngineMeRabbit,
                    new DefaultEndpointProvider(),
                    true,
                    Register.DefaultEndpointResolver(new RabbitMqConventionEndpointResolver(
                        "RabbitMq",
                        "messagepack",
                        environment: "lykke",
                        exclusiveQueuePostfix: "k8s")),

                    Register.BoundedContext(Self)
                    .ListeningCommands(pushNotificationsCommands)
                    .On(commandsRoute)
                    .WithCommandsHandler<NotificationCommandsHandler>()
                    .ProcessingOptions(commandsRoute).MultiThreaded(4).QueueCapacity(1024)
                    );
            })
            .As<ICqrsEngine>()
            .SingleInstance()
            .AutoActivate();

            #endregion
        }
    }
}
