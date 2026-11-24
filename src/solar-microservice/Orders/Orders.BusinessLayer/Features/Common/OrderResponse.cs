using Orders.CoreModels.Entities;
using Orders.CoreModels.Enums;
using RabbitMQ.Client;

namespace Orders.BusinessLayer.Features.Common;

public sealed record OrderItemResponse(
    int OrderItemId,
    ProductSummary Product,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice);

public sealed record ProductSummary(
    int ProductId,
    string ProductName,
    string ImageUrl);

public sealed record OrderResponse(int OrderId,string UserId,DateOnly OrderDate,
    OrderStatus Status,decimal TotalAmount,string DeliveryAddress,string? PhoneNumber,
    string? FullName,DateTimeOffset CreatedAt,DateTimeOffset? UpdatedAt,
    IReadOnlyList<OrderItemResponse> Items)
{
    public static OrderResponse FromEntity(Order order,IReadOnlyDictionary<int,ProductSummary> products)
    {
        return new OrderResponse(
            order.OrderId,
            order.UserId,
            order.OrderDate,
            order.Status,
            order.TotalAmount,
            order.DeliveryAddress,
            order.PhoneNumber,
            order.FullName,
            order.CreatedAt,
            order.UpdatedAt,
            order.OrderItems.OrderByDescending(x => x.OrderItemId)
            .Select(item =>
            {
                products.TryGetValue(item.ProductId, out var product);

                var productSummary = product ??new ProductSummary(item.ProductId,"Unknown Product",string.Empty);

                return new OrderItemResponse(
                    item.OrderItemId,
                    productSummary,
                    item.Quantity,
                    item.UnitPrice,
                    item.TotalPrice
                    );

            }).ToList()
            );

         
        
    }

    public static IReadOnlyList<OrderResponse> FromEntities(IEnumerable<Order> orders,
        IReadOnlyDictionary<int,ProductSummary> products)
    {
        return orders.Select(order=>FromEntity(order,products)).ToList();
    }
}
