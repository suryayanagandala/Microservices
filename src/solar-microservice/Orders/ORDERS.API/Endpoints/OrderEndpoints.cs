using BuildingBlocks.Utility;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.BusinessLayer.Features.CancelOrder;
using Orders.BusinessLayer.Features.CreateOrder;
using Orders.BusinessLayer.Features.GetInternalOrderResponse;
using Orders.BusinessLayer.Features.GetMyOrders;
using Orders.BusinessLayer.Features.GetOrderById;
using Orders.BusinessLayer.Features.UpdateOrderStatus;
using Orders.CoreModels.Enums;

namespace ORDERS.API.Endpoints;

public sealed record UpdateOrderStatusRequest(OrderStatus Status);

public sealed class OrderEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var orders = app.MapGroup("/orders")
            .WithTags("Orders");

        orders.MapPost("/", async (
                [FromBody] CreateOrderCommand command,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(command, cancellationToken);
                return Results.Created($"/orders/{result.OrderId}", result);
            })
            .RequireAuthorization()
            .WithName("CreateOrder")
            .WithSummary("Create an order using current Products API prices");

        orders.MapGet("/my-orders", async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetMyOrdersQuery(),
                    cancellationToken);

                return ApiResponse<object>.Success(result);
            })
            .RequireAuthorization()
            .WithName("GetMyOrders")
            .WithSummary("Get orders for the authenticated user");

        orders.MapGet("/{orderId:int}", async (
                [FromRoute] int orderId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetOrderByIdQuery(orderId),
                    cancellationToken);

                return Results.Ok(result);
            })
            .RequireAuthorization()
            .WithName("GetOrderById")
            .WithSummary("Get one order belonging to the authenticated user");

        orders.MapPatch("/{orderId:int}/status", async (
                [FromRoute] int orderId,
                [FromBody] UpdateOrderStatusRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new UpdateOrderStatusCommand(orderId, request.Status),
                    cancellationToken);

                return Results.Ok(result);
            })
            .RequireAuthorization(policy => policy.RequireRole("Admin"))
            .WithName("UpdateOrderStatus")
            .WithSummary("Update an order status using a valid transition");

        orders.MapPatch("/cancelOrder/{orderId:int}", async ([FromRouteAttribute] int orderId, ISender sender) =>
        {
            var response = await sender.Send(new CancelOrderRequest(orderId));

            return ApiResponse<bool>.Success(response);

        }).RequireAuthorization()
        .WithName("Camcel Order")
        .WithDescription("Cancel order using a valid orderId and userId");

        app.MapGet("/orders/internal/{orderId:int}",
async (int orderId, ISender sender) =>
{
    var result = await sender.Send(
        new GetInternalOrderCommand(orderId));

    return Results.Ok(result);
})
.RequireAuthorization();
    }
}
