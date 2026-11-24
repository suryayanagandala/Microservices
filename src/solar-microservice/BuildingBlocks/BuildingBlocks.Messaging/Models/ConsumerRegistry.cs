using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Models
{
    public sealed class ConsumerRegistry
    {
        private readonly ConcurrentDictionary<string, ConsumerRegistration> _registrations = new();

        public IEnumerable<ConsumerRegistration> Registrations=> _registrations.Values;
        public void Register(string queueName,string routingKey,Type eventType,Type handlerType, Type invokerType)
        {
            _registrations[routingKey] = new ConsumerRegistration
            {
                QueueName = queueName,
                RoutingKey = routingKey,
                EventType = eventType,
                HandlerType = handlerType,
                InvokerType = invokerType
            };
        }

        public bool TryGet(string routingKey,out ConsumerRegistration registration)
        {
            return _registrations.TryGetValue(routingKey,out registration!);
        }

        public int Count => _registrations.Count;
    }
}
