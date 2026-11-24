using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Contracts
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T message,string routingkey,CancellationToken cancellationToken=default)where T : IntegrationEvent;
    }
}
