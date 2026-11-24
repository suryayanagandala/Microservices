using BuildingBlocks.CQRS;

namespace Orders.BusinessLayer.Features.CreateOrder;

public sealed record CreateOrderItemRequest(int ProductId, int Quantity);

public sealed record CreateOrderCommand(
    string DeliveryAddress,
    string? PhoneNumber,
    string? FullName,
    IReadOnlyList<CreateOrderItemRequest> Items) : ICommand<CreateOrderResponse>;
