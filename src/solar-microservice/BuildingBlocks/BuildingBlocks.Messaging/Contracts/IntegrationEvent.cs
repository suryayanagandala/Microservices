using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Contracts
{
    public abstract record IntegrationEvent
    {
        public Guid EventId { get; set; }= Guid.NewGuid();

        public DateTimeOffset OccuredOn {  get; set; }= DateTimeOffset.UtcNow;
    }
}
