using BuildingBlocks.Messaging.Configuration;
using BuildingBlocks.Messaging.Consumers;
using BuildingBlocks.Messaging.Contracts;
using BuildingBlocks.Messaging.Models;
using BuildingBlocks.Messaging.Publishers;
using BuildingBlocks.Messaging.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Messaging.DependencyInjection
{
    public static class MessagingExtensions
    {
        public static IServiceCollection AddMessaging(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<RabbitMqSettings>(
                configuration.GetSection(RabbitMqSettings.SectionName));

            // Registry
            services.AddSingleton<ConsumerRegistry>(sp =>
            {
                var registry = new ConsumerRegistry();

                foreach (var registration in sp.GetServices<ConsumerRegistration>())
                {
                    registry.Register(
                        registration.QueueName,
                        registration.RoutingKey,
                        registration.EventType,
                        registration.HandlerType,
                        registration.InvokerType);
                }

                return registry;
            });

            // RabbitMQ
            services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
            services.AddSingleton<IChannelFactory, RabbitMqChannelFactory>();

            // Publisher
            services.AddSingleton<IEventBus, RabbitMqEventBus>();

            // Dispatcher
            services.AddScoped<IEventDispatcher, RabbitMqEventDispatcher>();

            // Consumer
            services.AddSingleton<RabbitMqConsumer>();
            services.AddHostedService<RabbitMqHostedService>();

            return services;
        }

        public static IServiceCollection AddIntegrationEvent<TEvent, THandler>(
    this IServiceCollection services,
    string queueName,
    string routingKey)
    where TEvent : IntegrationEvent
    where THandler : class, IIntegrationEventHandler<TEvent>
        {
            services.AddScoped<IIntegrationEventHandler<TEvent>, THandler>();
            services.AddScoped<THandler>();
            services.AddScoped<EventInvoker<TEvent>>();

            services.AddSingleton(new ConsumerRegistration {
                QueueName= queueName,
                RoutingKey= routingKey,
                EventType= typeof(TEvent),
                HandlerType= typeof(THandler),
                InvokerType= typeof(EventInvoker<TEvent>) });

            return services;
        }
    }
}
