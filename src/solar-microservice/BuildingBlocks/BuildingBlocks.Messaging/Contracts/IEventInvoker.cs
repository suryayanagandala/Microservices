using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Contracts
{
    public interface IEventInvoker
    {
        Task InvokeAsync(
            object integrationEvent,
            CancellationToken cancellationToken);
    }
}
