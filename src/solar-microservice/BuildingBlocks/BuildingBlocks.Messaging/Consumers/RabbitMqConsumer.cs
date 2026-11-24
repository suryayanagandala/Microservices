using BuildingBlocks.Messaging.Configuration;
using BuildingBlocks.Messaging.Contracts;
using BuildingBlocks.Messaging.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Consumers
{
    public sealed class RabbitMqConsumer(
    IChannelFactory channelFactory,
    ConsumerRegistry registry,
    IServiceProvider serviceProvider,
    IOptions<RabbitMqSettings> options,
    ILogger<RabbitMqConsumer> logger)
    {
        private IChannel? _channel;

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            _channel = await channelFactory.CreateChannelAsync();

            // Exchange
            await _channel.ExchangeDeclareAsync(
                exchange: options.Value.ExchangeName,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                cancellationToken: cancellationToken);

            logger.LogInformation(
                "Registered Consumers : {Count}",
                registry.Registrations.Count());

            // Create every queue and bind automatically
            foreach (var registration in registry.Registrations)
            {
                await _channel.QueueDeclareAsync(
                    queue: registration.QueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null,
                    cancellationToken: cancellationToken);

                await _channel.QueueBindAsync(
                    queue: registration.QueueName,
                    exchange: options.Value.ExchangeName,
                    routingKey: registration.RoutingKey,
                    cancellationToken: cancellationToken);

                logger.LogInformation(
                    "Queue {Queue} Bound -> {RoutingKey}",
                    registration.QueueName,
                    registration.RoutingKey);
            }

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (_, args) =>
            {
                try
                {
                    var routingKey = args.RoutingKey;

                    var json = Encoding.UTF8.GetString(args.Body.ToArray());

                    logger.LogInformation(
                        "Received {RoutingKey}",
                        routingKey);

                    using var scope = serviceProvider.CreateScope();

                    var dispatcher =
                        scope.ServiceProvider.GetRequiredService<IEventDispatcher>();

                    await dispatcher.DispatchAsync(
                        routingKey,
                        json,
                        cancellationToken);

                    await _channel.BasicAckAsync(
                        args.DeliveryTag,
                        false,
                        cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Consumer Error");

                    await _channel.BasicNackAsync(
                        args.DeliveryTag,
                        false,
                        true,
                        cancellationToken);
                }
            };

            // Start consuming from every queue
            foreach (var queue in registry.Registrations
                                          .Select(x => x.QueueName)
                                          .Distinct())
            {
                await _channel.BasicConsumeAsync(
                    queue,
                    false,
                    consumer,
                    cancellationToken);

                logger.LogInformation(
                    "Listening Queue : {Queue}",
                    queue);
            }
        }
    }
}
