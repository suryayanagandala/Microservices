using BuildingBlocks.CQRS;
using Orders.BusinessLayer.Features.Common;

namespace Orders.BusinessLayer.Features.GetOrderById;

public sealed record GetOrderByIdQuery(int OrderId) : IQuery<OrderResponse>;
