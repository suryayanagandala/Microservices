using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Contracts
{
    public interface IEventDispatcher
    {
        public Task DispatchAsync(string routingKey, string json, CancellationToken cancellationToken = default);
    }
}
