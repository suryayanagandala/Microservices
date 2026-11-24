using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.BusinessLayer.Features.GetCartItems
{
    public record GetCartItemsResponse(int cartId,decimal totalAmount,List<CartItemResponse> CartItems);

        public record CartItemResponse(int CartItemId,int ProductId, string ProductName,
          decimal Price,int Quantity,string? ImageUrl);
}
