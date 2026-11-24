using BuildingBlocks.Authentication_.Abstractions;
using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using Cart.ApplicationLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.BusinessLayer.Features.GetCartItems
{
    internal class GetCartItemsHandler(IUnitOfWork unitOfWork,ICurrentUser currentUser,IProductService productService) : IQueryHandler<GetCartItemsRequest, GetCartItemsResponse>
    {
        public async Task<GetCartItemsResponse> Handle(GetCartItemsRequest request, CancellationToken cancellationToken)
        {
            var userId = currentUser.UserId;

            if (userId is null) throw new UnauthorizedException("please login first");

            var cart=await unitOfWork.ICartRepository.GetCartWithItems(userId);

            if(cart is null)
            {
                throw new NotFoundException("No Cart Found");
            }

            var productIds=cart.CartItems.Select(x=>x.ProductId).ToList();

            var products = await productService.GetProductsAsync(productIds, cancellationToken);

            var lookup = products.ToDictionary(x => x.ProductId);

            var items = cart.CartItems.Select(item =>
            {
                var product = lookup[item.ProductId];

                return new CartItemResponse(item.CartItemId,
                    item.ProductId,
                    product.ProductName,
                    item.UnitPrice,
                    item.Quantity,
                    product.ImageUrl);

            }).ToList();

            return new GetCartItemsResponse(cart.CartId, items.Sum(x => x.Quantity * x.Price), items);
        }
    }
}
