using BuildingBlocks.Messaging.Contracts;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Services
{
    public sealed class RabbitMqChannelFactory : IChannelFactory
    {
        private readonly IRabbitMqConnection _connection;

        public RabbitMqChannelFactory(IRabbitMqConnection connection)
        {
            _connection = connection;
        }

        public async Task<IChannel> CreateChannelAsync()
        {
            var conn = await _connection.GetConnectionAsync();
            return await conn.CreateChannelAsync();
        }
    }
}
