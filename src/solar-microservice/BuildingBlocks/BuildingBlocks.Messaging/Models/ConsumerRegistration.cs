using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Models
{
    

    public sealed class ConsumerRegistration
    {
        public string QueueName { get; init; } = string.Empty;

        public string RoutingKey { get; init; } = string.Empty;

        public Type EventType { get; init; } = null!;

        public Type HandlerType { get; init; } = null!;

        public Type InvokerType { get; init; } = null!;
    }
}
