using BuildingBlocks.Authentication_.Abstractions;
using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using Cart.ApplicationLayer.Interfaces;
using Minio.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.BusinessLayer.Features.UpdateCart
{
    internal class UpdateCartHandler(IUnitOfWork unitOfWork,ICurrentUser currentUser,ICartNotificationService notificationService) : ICommandHandler<UpdateCartRequest, UpdateCartResponse>
    {
        public async Task<UpdateCartResponse> Handle(UpdateCartRequest request, CancellationToken cancellationToken)
        {
           var cartItem=await unitOfWork.ICartRepository.GetCartItemById(request.cartItemId,cancellationToken);

            if(cartItem is null)
            throw new NotFoundException("Cart item not found.");

            if (cartItem.Cart.UserId != currentUser.UserId)
            {
                throw new UnauthorizedAccessException();
            }

            cartItem.Quantity = request.quantity;
            cartItem.UpdatedAt = DateTimeOffset.UtcNow;

            await unitOfWork.SaveChanges(cancellationToken);

            var count = await unitOfWork.ICartRepository.GetCartCount(currentUser.UserId);

            await notificationService.SendCartCount(currentUser.UserId, count);

            return new UpdateCartResponse
            {
                IsSucess=true,
                Message="Cart Updated Succesfully",
                UpdatedAt=cartItem.UpdatedAt.Value,
            };
        }
    }
}
