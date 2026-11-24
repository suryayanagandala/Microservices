using Orders.CoreModels.Enums;

namespace Orders.BusinessLayer.Features.CreateOrder;

public sealed record CreateOrderResponse(
    int OrderId,
    OrderStatus Status,
    decimal TotalAmount,
    string Message);
