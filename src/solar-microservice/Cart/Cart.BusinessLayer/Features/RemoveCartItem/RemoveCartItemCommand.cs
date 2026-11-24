using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.BusinessLayer.Features.RemoveCartItem
{
    public record RemoveCartItemQuery(int cartItemId):ICommand<RemoveCartItemResponse>;

    public record RemoveCartItemResponse(bool isSucess,string message);
}
