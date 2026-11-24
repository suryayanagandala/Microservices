using BuildingBlocks.Authentication_.Abstractions;
using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using Cart.ApplicationLayer.Interfaces;
using Cart.CoreModels.Entities;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.BusinessLayer.Features.RemoveCartItem
{
    public class RemoveCartItemHandler(IUnitOfWork unitOfWork,ICurrentUser currentUser,ICartNotificationService cartNotification) 
        : ICommandHandler<RemoveCartItemQuery, RemoveCartItemResponse>
    {
        public async Task<RemoveCartItemResponse> Handle(RemoveCartItemQuery request, CancellationToken cancellationToken)
        {
            var userId=currentUser.UserId;

            if (userId == null) throw new UnauthorizedAccessException();

            var cartItem = await unitOfWork.ICartRepository.GetCartItemById(request.cartItemId, cancellationToken);

            if (cartItem is null)
                throw new NotFoundException("Cart item not found.");

            if (cartItem.Cart.UserId != userId)
                throw new UnauthorizedAccessException();

            unitOfWork.ICartRepository.RemoveCartItem(cartItem);

            await unitOfWork.SaveChanges(cancellationToken);

            var count = await unitOfWork.ICartRepository.GetCartCount(userId);

            await cartNotification.SendCartCount(userId, count);

            return new RemoveCartItemResponse(
                true,
                "Cart item removed successfully.");
        }
    }
}
