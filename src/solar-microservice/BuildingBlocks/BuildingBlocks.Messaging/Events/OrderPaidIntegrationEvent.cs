using BuildingBlocks.Messaging.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Events
{
    public sealed record OrderPaidIntegrationEvent(
    int OrderId,
    string UserId,
    decimal TotalAmount,
    DateTimeOffset PaidAt)
    : IntegrationEvent;
}
