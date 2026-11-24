using BuildingBlocks.Authentication_.Abstractions;
using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using Orders.ApplicationLayer.Interfaces;
using Orders.CoreModels.Entities;
using Orders.CoreModels.Enums;

namespace Orders.BusinessLayer.Features.CreateOrder;

public sealed class CreateOrderHandler(
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    IProductService productService,
    IOrderNotificationService notificationService)
    : ICommandHandler<CreateOrderCommand, CreateOrderResponse>
{
    public async Task<CreateOrderResponse> Handle(
        CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;

        if (string.IsNullOrWhiteSpace(userId))
            throw new UnauthorizedException("Please log in before creating an order.");

        var requestedItems = command.Items
            .GroupBy(item => item.ProductId)
            .Select(group => new CreateOrderItemRequest(
                group.Key,
                group.Sum(item => item.Quantity)))
            .ToList();

        var productIds = requestedItems
            .Select(item => item.ProductId)
            .ToArray();

        var products = await productService.GetProductsAsync(
            productIds,
            cancellationToken);

        var productsById = products
            .Where(product => product.IsActive)
            .ToDictionary(product => product.ProductId);

        var unavailableProductIds = productIds
            .Where(productId => !productsById.ContainsKey(productId))
            .ToArray();

        if (unavailableProductIds.Length > 0)
        {
            throw new BadRequestException(
                $"Products are unavailable: {string.Join(", ", unavailableProductIds)}.");
        }

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Status = OrderStatus.CREATED,
            DeliveryAddress = command.DeliveryAddress.Trim(),
            PhoneNumber = string.IsNullOrWhiteSpace(command.PhoneNumber)
                ? null
                : command.PhoneNumber.Trim(),
            FullName = string.IsNullOrWhiteSpace(command.FullName)
                ? null
                : command.FullName.Trim(),
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow
        };

        foreach (var requestedItem in requestedItems)
        {
            var product = productsById[requestedItem.ProductId];

            order.OrderItems.Add(new OrderItem
            {
                ProductId = requestedItem.ProductId,
                Quantity = requestedItem.Quantity,
                UnitPrice = product.Price
            });
        }

        order.TotalAmount = order.OrderItems.Sum(
            item => item.Quantity * item.UnitPrice);

        await unitOfWork.Orders.AddAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await notificationService.SendOrderCreatedAsync(
            order.UserId,
            order.OrderId,
            order.TotalAmount,
            cancellationToken);

        return new CreateOrderResponse(
            order.OrderId,
            order.Status,
            order.TotalAmount,
            "Order created successfully.");
    }
}
