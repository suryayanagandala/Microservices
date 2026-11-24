using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Contracts
{
    public interface IIntegrationEventHandler<in TEvent> where TEvent : IntegrationEvent
    {
        Task HandleAsync(TEvent message, CancellationToken cancellationToken = default);
    }
}
