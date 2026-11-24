using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.BusinessLayer.Features.GetCartItems
{
    public record GetCartItemsRequest:IQuery<GetCartItemsResponse>;
}
