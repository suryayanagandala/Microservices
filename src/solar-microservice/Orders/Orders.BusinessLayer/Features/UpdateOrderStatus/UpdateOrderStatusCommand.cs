using BuildingBlocks.CQRS;
using Orders.CoreModels.Enums;

namespace Orders.BusinessLayer.Features.UpdateOrderStatus;

public sealed record UpdateOrderStatusCommand(
    int OrderId,
    OrderStatus Status) : ICommand<UpdateOrderStatusResponse>;
