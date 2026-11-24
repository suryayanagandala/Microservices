using RabbitMQ.Client;

namespace BuildingBlocks.Messaging.Contracts;

public interface IChannelFactory
{
    Task<IChannel> CreateChannelAsync(CreateChannelOptions? options = null);
}
