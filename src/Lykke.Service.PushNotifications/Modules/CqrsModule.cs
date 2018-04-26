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
using Lykke.Service.PushNotifications.Contract.Commands;

namespace Lykke.Service.PushNotifications.Modules
{
    public class CqrsModule : Module
    {
        public static readonly string PushNotifications = PushNotificationsBoundedContext.Name;

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
            var rabbitMqClientSettings = new RabbitMQ.Client.ConnectionFactory { Uri = _settings.CurrentValue.SagasRabbitMq.RabbitConnectionString };

            builder.Register(context => new AutofacDependencyResolver(context)).As<IDependencyResolver>();
            builder.RegisterType<NotificationCommandsHandler>().SingleInstance();
            
            var messagingEngineMeRabbit = new MessagingEngine(_log,
                new TransportResolver(new Dictionary<string, TransportInfo>
                {
                    {
                        "SagasRabbitMq", new TransportInfo(rabbitMqClientSettings.Endpoint.ToString(), rabbitMqClientSettings.UserName, rabbitMqClientSettings.Password, "None", "RabbitMq")
                    }
                }),
                new RabbitMqTransportFactory());

            //Get all types from Commands namespace
            var pushNotificationsCommands = typeof(PushNotificationsBoundedContext).Assembly
                                            .GetTypes()
                                            .Where(x => x.Namespace == typeof(TextNotificationCommand).Namespace)
                                            .ToArray();
            
            var clientEndpointResolver = new RabbitMqConventionEndpointResolver(
                "SagasRabbitMq",
                "messagepack",
                environment: "lykke",
                exclusiveQueuePostfix: "k8s");

            builder.Register(ctx =>
                {
                    return new CqrsEngine(_log,
                        ctx.Resolve<IDependencyResolver>(),
                        messagingEngineMeRabbit,
                        new DefaultEndpointProvider(),
                        true,
                        Register.DefaultEndpointResolver(new RabbitMqConventionEndpointResolver(
                            "SagasRabbitMq",
                            "messagepack",
                            environment: "lykke",
                            exclusiveQueuePostfix: "k8s")),

                        Register.BoundedContext(PushNotifications)
                            .ListeningCommands(pushNotificationsCommands)
                            .On(commandsRoute)
                            .WithEndpointResolver(clientEndpointResolver)
                            .WithCommandsHandler<NotificationCommandsHandler>()
                            .ProcessingOptions(commandsRoute).MultiThreaded(4).QueueCapacity(1024)
                    );
                })
            .As<ICqrsEngine>()
            .SingleInstance()
            .AutoActivate();            
        }
    }
}
