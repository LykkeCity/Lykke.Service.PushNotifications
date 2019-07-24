using Autofac;
using Lykke.Cqrs;
using Lykke.Cqrs.Configuration;
using Lykke.Messaging;
using Lykke.Messaging.RabbitMq;
using Lykke.Service.PushNotifications.Contract;
using Lykke.Service.PushNotifications.Workflow.CommandHandlers;
using Lykke.SettingsReader;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Cqrs.Middleware.Logging;
using Lykke.Messaging.Contract;
using Lykke.Service.PushNotifications.Contract.Commands;
using Lykke.Service.PushNotifications.Settings;

namespace Lykke.Service.PushNotifications.Modules
{
    [UsedImplicitly]
    public class CqrsModule : Module
    {
        private readonly IReloadingManager<AppSettings> _settings;

        public CqrsModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            string commandsRoute = "commands";
            Messaging.Serialization.MessagePackSerializerFactory.Defaults.FormatterResolver = MessagePack.Resolvers.ContractlessStandardResolver.Instance;
            var rabbitMqClientSettings = new RabbitMQ.Client.ConnectionFactory { Uri = _settings.CurrentValue.SagasRabbitMq.RabbitConnectionString };

            builder.Register(context => new AutofacDependencyResolver(context)).As<IDependencyResolver>();
            builder.RegisterType<NotificationCommandsHandler>().SingleInstance();

            builder.Register(ctx => new MessagingEngine(ctx.Resolve<ILogFactory>(),
                new TransportResolver(new Dictionary<string, TransportInfo>
                {
                    {
                        "SagasRabbitMq",
                        new TransportInfo(rabbitMqClientSettings.Endpoint.ToString(), rabbitMqClientSettings.UserName,
                            rabbitMqClientSettings.Password, "None", "RabbitMq")
                    }
                }),
                new RabbitMqTransportFactory(ctx.Resolve<ILogFactory>()))).As<IMessagingEngine>().SingleInstance();

            //Get all types from Commands namespace
            var pushNotificationsCommands = typeof(PushNotificationsBoundedContext).Assembly
                                            .GetTypes()
                                            .Where(x => x.Namespace == typeof(TextNotificationCommand).Namespace)
                                            .ToArray();

            builder.Register(ctx =>
                {
                    var clientEndpointResolver = new RabbitMqConventionEndpointResolver(
                        "SagasRabbitMq",
                        Messaging.Serialization.SerializationFormat.MessagePack,
                        environment: "lykke",
                        exclusiveQueuePostfix: "k8s");

                    var engine = new CqrsEngine(ctx.Resolve<ILogFactory>(),
                        ctx.Resolve<IDependencyResolver>(),
                        ctx.Resolve<IMessagingEngine>(),
                        new DefaultEndpointProvider(),
                        true,

                        Register.EventInterceptors(new DefaultEventLoggingInterceptor(ctx.Resolve<ILogFactory>())),

                        Register.DefaultEndpointResolver(clientEndpointResolver),

                        Register.BoundedContext(PushNotificationsBoundedContext.Name)
                            .ListeningCommands(pushNotificationsCommands)
                            .On(commandsRoute)
                            .WithEndpointResolver(clientEndpointResolver)
                            .WithCommandsHandler<NotificationCommandsHandler>()
                            .ProcessingOptions(commandsRoute).MultiThreaded(4).QueueCapacity(1024)
                    );

                    engine.StartPublishers();
                    return engine;
                })
                .As<ICqrsEngine>()
                .SingleInstance()
                .AutoActivate();
        }
    }
}
