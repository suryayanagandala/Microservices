using BuildingBlocks.Messaging.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Contracts.Events
{
    public record UserRegisteredIntegrationEvent(
     string userId,  string Name,string Email) : IntegrationEvent;
}
