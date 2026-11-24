using BuildingBlocks.Messaging.Contracts;
using RabbitMQ.Client;

namespace BuildingBlocks.Messaging.Services;

public sealed class RabbitMqChannelFactory(IRabbitMqConnection connection) : IChannelFactory
{
    public async Task<IChannel> CreateChannelAsync(CreateChannelOptions? options = null)
    {
        var rabbitConnection = await connection.GetConnectionAsync();
        return await rabbitConnection.CreateChannelAsync(options);
    }
}
