using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Contracts.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class IntegrationEventHandlerAttribute:Attribute
    {
        public string RoutingKey { get; set; }

        public IntegrationEventHandlerAttribute(string routingkey)
        {
            RoutingKey=routingkey;
        }
    }
}
