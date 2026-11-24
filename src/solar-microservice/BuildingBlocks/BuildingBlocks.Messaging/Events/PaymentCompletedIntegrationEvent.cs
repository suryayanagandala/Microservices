using BuildingBlocks.Messaging.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Events
{
    public sealed record PaymentCompletedIntegrationEvent(
    int PaymentId,
    int OrderId,
    string UserId,
    decimal Amount,
    string TransactionId,
    string PaymentMethod
    ) : IntegrationEvent;
}
