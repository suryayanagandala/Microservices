using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Contracts.Events
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RabbitConsumerAttribute:Attribute
    {
        public string QueueName { get; } = string.Empty;

        public string RoutingKey { get; } = string.Empty;

        public RabbitConsumerAttribute(string queueName,string routingKey)
        {
            QueueName = queueName;
            RoutingKey = routingKey;
        }
    }
}
