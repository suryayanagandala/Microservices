using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.BusinessLayer.Features.UpdateCart
{
    public record UpdateCartRequest(int cartItemId,int quantity):ICommand<UpdateCartResponse>;
}
