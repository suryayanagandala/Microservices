using BuildingBlocks.Authentication_.Abstractions;
using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using Orders.ApplicationLayer.Interfaces;
using Orders.CoreModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.BusinessLayer.Features.CancelOrder
{
    internal class CancelOrderHandler(IUnitOfWork unitOfWork,ICurrentUser currentUser) : ICommandHandler<CancelOrderRequest, bool>
    {
        public async Task<bool> Handle(CancelOrderRequest request, CancellationToken cancellationToken)
        {
            var userId = currentUser.UserId;

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("Please login first");

            var order = await unitOfWork.Orders.GetOrderByUserId(userId, cancellationToken);

            if (order is null)
                throw new NotFoundException("Order not found.");

            if (order is null)
                throw new NotFoundException("Order not found.");

            if (order.Status is OrderStatus.SHIPPED
                or OrderStatus.DELIVERED
                or OrderStatus.CANCELLED)
            {
                throw new BadRequestException(
                    "Order cannot be cancelled.");
            }

            order.Status = OrderStatus.CANCELLED;
            order.UpdatedAt = DateTimeOffset.UtcNow;

            unitOfWork.Orders.Update(order);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return true;

        }
    }
}
