using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.BusinessLayer.Features.GetInternalOrderResponse
{
    public sealed record GetInternalOrderCommand(int orderId):IQuery<GetInternalOrderResult>;
    
}
