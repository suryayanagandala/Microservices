using BuildingBlocks.Authentication_.Abstractions;
using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using Cart.ApplicationLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.BusinessLayer.Features.ClearCart
{
    internal class ClearCartHandler(IUnitOfWork unitOfWork,ICurrentUser currentUser,ICartNotificationService notificationService)
    : ICommandHandler<ClearCartCommand, ClearCartResult>
    {
        public async Task<ClearCartResult> Handle(ClearCartCommand request,CancellationToken cancellationToken)
        {
            var userId = currentUser.UserId;

            if (userId is null) throw new UnauthorizedAccessException();

            var cart = await unitOfWork.ICartRepository.GetCartByUserId(userId,cancellationToken);

            if (cart is null)
                throw new NotFoundException("Cart not found.");

            var activeItems = cart.CartItems.Where(x => x.IsActive).ToList();

            foreach (var item in activeItems)
            {
                item.IsActive = false;
                item.UpdatedAt = DateTimeOffset.UtcNow;
            }

            unitOfWork.ICartRepository.UpdateCartItems(activeItems);

            await unitOfWork.SaveChanges(cancellationToken);

            await notificationService.SendCartCount(userId, 0);

            return new ClearCartResult(true,"Cart cleared successfully.");
        }
    }
}
