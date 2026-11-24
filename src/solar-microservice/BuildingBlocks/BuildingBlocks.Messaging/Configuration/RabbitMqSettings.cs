using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Configuration
{
    public sealed class RabbitMqSettings
    {
        public const string SectionName = "RabbitMQ";

        public string HostName { get; init; } = string.Empty;

        public int Port { get; init; } = 5672;

        public string UserName { get; init; } = string.Empty;

        public string Password { get; init; } = string.Empty;

        public string VirtualHost { get; init; } = "/";

        public string ExchangeName { get; init; } = string.Empty;
    }
}
