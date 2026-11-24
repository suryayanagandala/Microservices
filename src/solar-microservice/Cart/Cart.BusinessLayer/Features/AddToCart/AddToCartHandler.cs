using BuildingBlocks.Authentication_.Abstractions;
using BuildingBlocks.CQRS;
using Cart.ApplicationLayer.Interfaces;
using Cart.CoreModels.Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.BusinessLayer.Features.AddToCart
{
    public  class AddToCartHandler(IUnitOfWork unitOfWork,ICurrentUser currentUser,IProductService productService, ICartNotificationService notificationService)
    : ICommandHandler<AddToCartCommand, AddToCartResponse>
    {
        public async Task<AddToCartResponse> Handle(AddToCartCommand command,CancellationToken cancellationToken)
        {
            var userId = currentUser.UserId;

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("User not found.");


            // Validate Product


            var product = await productService.GetProductByIdAsync(command.ProductId,cancellationToken);

            if (product is null || !product.IsActive)
                throw new Exception("Product not found.");


            // Get Cart


            var cart = await unitOfWork.ICartRepository.GetCartByUserId(userId,cancellationToken);

            
            // Create Cart
           

            if (cart is null)
            {
                cart = new Carts
                {
                    UserId = userId,
                    CreatedAt = DateTimeOffset.UtcNow,
                    IsActive = true
                };

                await unitOfWork.ICartRepository.CreateCart(cart,cancellationToken);

                await unitOfWork.SaveChanges(cancellationToken);
            }

           
            // Existing Item?
            

            var cartItem = await unitOfWork.ICartRepository.GetCartItem(cart.CartId, command.ProductId,cancellationToken);

            if (cartItem is not null)
            {
                cartItem.Quantity += command.Quantity;
                cartItem.UpdatedAt = DateTimeOffset.UtcNow;

                unitOfWork.ICartRepository.UpdateCartItem(cartItem);
            }
            else
            {
                await unitOfWork.ICartRepository.AddCartItem(
                    new CartItem
                    {
                        CartId = cart.CartId,
                        ProductId = command.ProductId,
                        Quantity = command.Quantity,
                        UnitPrice = product.Price,
                        IsActive = true
                    },cancellationToken);
            }

           

            await unitOfWork.SaveChanges(cancellationToken);

            var count = await unitOfWork.ICartRepository.GetCartCount(userId);

            await notificationService.SendCartCount(userId, count);

            return new AddToCartResponse(
                true,
                "Item added successfully.");
        }
    }

}
