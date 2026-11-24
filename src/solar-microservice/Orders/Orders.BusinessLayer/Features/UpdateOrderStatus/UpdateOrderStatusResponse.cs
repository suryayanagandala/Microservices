using Orders.CoreModels.Enums;

namespace Orders.BusinessLayer.Features.UpdateOrderStatus;

public sealed record UpdateOrderStatusResponse(
    int OrderId,
    OrderStatus PreviousStatus,
    OrderStatus CurrentStatus,
    string Message);
