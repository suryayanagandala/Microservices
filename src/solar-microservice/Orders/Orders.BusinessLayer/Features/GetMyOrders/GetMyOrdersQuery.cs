using BuildingBlocks.CQRS;
using Orders.BusinessLayer.Features.Common;

namespace Orders.BusinessLayer.Features.GetMyOrders;

public sealed record GetMyOrdersQuery : IQuery<IReadOnlyList<OrderResponse>>;
