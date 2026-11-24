using BuildingBlocks.Authentication_.Abstractions;
using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using Orders.ApplicationLayer.Interfaces;
using Orders.BusinessLayer.Features.Common;

namespace Orders.BusinessLayer.Features.GetOrderById;

public sealed class GetOrderByIdHandler(IUnitOfWork unitOfWork,ICurrentUser currentUser,IProductService productService): IQueryHandler<GetOrderByIdQuery, OrderResponse>
{
    public async Task<OrderResponse> Handle(GetOrderByIdQuery query,CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;

        if (string.IsNullOrWhiteSpace(userId))
            throw new UnauthorizedException("Please log in to view the order.");

        var order = await unitOfWork.Orders.GetByIdAsync(query.OrderId,cancellationToken);

        if (order is null || !string.Equals(order.UserId, userId, StringComparison.Ordinal))
            throw new NotFoundException("Order not found.");

        var productIds = order.OrderItems.Select(x => x.ProductId).Distinct().ToList();

        var products = await productService.GetProductsAsync(productIds,cancellationToken);

        var dictionary = products.ToDictionary(x => x.ProductId,x => new ProductSummary(x.ProductId,x.ProductName,x.ImageUrl??"not found"));

        return OrderResponse.FromEntity(order,dictionary);
    }
}
