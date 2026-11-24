using BuildingBlocks.Authentication_.Abstractions;
using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using FluentValidation.Results;
using Orders.ApplicationLayer.Interfaces;
using Orders.BusinessLayer.Features.Common;

namespace Orders.BusinessLayer.Features.GetMyOrders;

public sealed class GetMyOrdersHandler(IUnitOfWork unitOfWork,ICurrentUser currentUser,IProductService productService)
    : IQueryHandler<GetMyOrdersQuery, IReadOnlyList<OrderResponse>>
{
    public async Task<IReadOnlyList<OrderResponse>> Handle(GetMyOrdersQuery query,CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;

        if (string.IsNullOrWhiteSpace(userId))
            throw new UnauthorizedException("Please log in to view orders.");

        var orders = await unitOfWork.Orders.GetByUserIdAsync(userId,cancellationToken);

        if (!orders.Any())
        {
            return [];
        }
        
        var productIds=orders.SelectMany(x=>x.OrderItems).Select(x=>x.ProductId).Distinct().ToList();

        var products = await productService.GetProductsAsync(productIds, cancellationToken);

        var dictonary = products.ToDictionary(x => x.ProductId, x => new ProductSummary(x.ProductId, x.ProductName, x.ImageUrl ?? "/uploads/categories/6d4700da-0952-46ac-ae38-22fe285626d1.jpg"));

        return OrderResponse.FromEntities(orders,dictonary);

        
    }
}
