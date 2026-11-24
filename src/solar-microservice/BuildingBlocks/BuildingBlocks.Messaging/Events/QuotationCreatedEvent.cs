using BuildingBlocks.Messaging.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Contracts.Events
{
    public record QuotationCreatedEvent( Guid QuotationId,
        Guid CustomerId,decimal Amount) : IntegrationEvent;
}
