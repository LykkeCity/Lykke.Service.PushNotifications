using System;
using System.Threading.Tasks;
using Autofac;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.PushNotifications.Core.Domain;
using Lykke.Service.Session.Contracts;

namespace Lykke.Service.PushNotifications.RabbitSubscribers
{
    [UsedImplicitly]
    public class SessionsSubscriber : IStartable, IDisposable
    {
        private readonly string _connectionString;
        private readonly string _exchangeName;
        private readonly IFcmTokensRepository _fcmTokensRepository;
        private readonly ILogFactory _logFactory;
        private RabbitMqSubscriber<SessionRemovedEvent> _subscriber;

        public SessionsSubscriber(
            string connectionString,
            string exchangeName,
            IFcmTokensRepository fcmTokensRepository,
            ILogFactory logFactory
        )
        {
            _connectionString = connectionString;
            _exchangeName = exchangeName;
            _fcmTokensRepository = fcmTokensRepository;
            _logFactory = logFactory;
        }

        public void Start()
        {
            var settings = new RabbitMqSubscriptionSettings
            {
                ConnectionString = _connectionString,
                ExchangeName = _exchangeName,
                QueueName = $"push-notifications-{nameof(SessionsSubscriber)}",
                DeadLetterExchangeName = null,
                RoutingKey = nameof(SessionRemovedEvent),
                IsDurable = true
            };

            _subscriber = new RabbitMqSubscriber<SessionRemovedEvent>(_logFactory,
                    settings,
                    new ResilientErrorHandlingStrategy(_logFactory, settings, TimeSpan.FromSeconds(10)))
                .SetMessageDeserializer(new JsonMessageDeserializer<SessionRemovedEvent>())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .Start();
        }

        public void Dispose()
        {
            _subscriber?.Stop();
            _subscriber?.Dispose();
        }

        private Task ProcessMessageAsync(SessionRemovedEvent message)
        {
            return _fcmTokensRepository.DeleteBySessionIdAsync(message.AuthId);
        }

        private RabbitMqSubscriber<T> CreateSubscriber<T>(Func<T, Task> func)
        {
            var settings = new RabbitMqSubscriptionSettings
            {
                ConnectionString = _connectionString,
                ExchangeName = _exchangeName,
                QueueName = $"push-notifications-{nameof(SessionsSubscriber)}",
                DeadLetterExchangeName = null,
                RoutingKey = nameof(T)
            };

            return new RabbitMqSubscriber<T>(_logFactory,
                    settings,
                    new ResilientErrorHandlingStrategy(_logFactory, settings, TimeSpan.FromSeconds(10)))
                .SetMessageDeserializer(new JsonMessageDeserializer<T>())
                .Subscribe(func)
                .CreateDefaultBinding();
        }
    }
}
