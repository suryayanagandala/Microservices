using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using Orders.ApplicationLayer.Interfaces;
using Orders.CoreModels.Enums;

namespace Orders.BusinessLayer.Features.UpdateOrderStatus;

public sealed class UpdateOrderStatusHandler(
    IUnitOfWork unitOfWork,
    IOrderNotificationService notificationService)
    : ICommandHandler<UpdateOrderStatusCommand, UpdateOrderStatusResponse>
{
    private static readonly IReadOnlyDictionary<OrderStatus, OrderStatus[]> AllowedTransitions =
        new Dictionary<OrderStatus, OrderStatus[]>
        {
            [OrderStatus.CREATED] =
                [OrderStatus.PAYMENT_PENDING, OrderStatus.COD_PENDING, OrderStatus.CANCELLED],
            [OrderStatus.PAYMENT_PENDING] =
                [OrderStatus.PAID, OrderStatus.PAYMENT_FAILED, OrderStatus.CANCELLED],
            [OrderStatus.PAID] =
                [OrderStatus.CONFIRMED, OrderStatus.CANCELLED],
            [OrderStatus.COD_PENDING] =
                [OrderStatus.CONFIRMED, OrderStatus.CANCELLED],
            [OrderStatus.CONFIRMED] =
                [OrderStatus.SHIPPED, OrderStatus.CANCELLED],
            [OrderStatus.SHIPPED] =
                [OrderStatus.DELIVERED],
            [OrderStatus.DELIVERED] = [],
            [OrderStatus.CANCELLED] = [],
            [OrderStatus.PAYMENT_FAILED] = []
        };

    public async Task<UpdateOrderStatusResponse> Handle(
        UpdateOrderStatusCommand command,
        CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Orders.GetByIdAsync(
            command.OrderId,
            cancellationToken);

        if (order is null)
            throw new NotFoundException("Order not found.");

        var previousStatus = order.Status;

        if (!AllowedTransitions[previousStatus].Contains(command.Status))
        {
            throw new BadRequestException(
                $"Order status cannot change from {previousStatus} to {command.Status}.");
        }

        order.Status = command.Status;
        order.UpdatedAt = DateTimeOffset.UtcNow;

        unitOfWork.Orders.Update(order);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await notificationService.SendOrderStatusUpdatedAsync(
            order.UserId,
            order.OrderId,
            order.Status,
            cancellationToken);

        return new UpdateOrderStatusResponse(
            order.OrderId,
            previousStatus,
            order.Status,
            "Order status updated successfully.");
    }
}
