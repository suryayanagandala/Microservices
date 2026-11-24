using BuildingBlocks.Messaging.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Services
{
    public sealed class EventInvoker<TEvent> : IEventInvoker
     where TEvent : IntegrationEvent
    {
        private readonly IIntegrationEventHandler<TEvent> _handler;

        public EventInvoker(
            IIntegrationEventHandler<TEvent> handler)
        {
            _handler = handler;
        }

        public async Task InvokeAsync(
            object integrationEvent,
            CancellationToken cancellationToken)
        {
            await _handler.HandleAsync(
                (TEvent)integrationEvent,
                cancellationToken);
        }
    }
}
