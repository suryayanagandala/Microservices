using BuildingBlocks.Utility;
using Cart.ApplicationLayer.Interfaces;
using Cart.BusinessLayer.Features.AddToCart;
using Cart.BusinessLayer.Features.CheckOut;
using Cart.BusinessLayer.Features.ClearCart;
using Cart.BusinessLayer.Features.GetCartItems;
using Cart.BusinessLayer.Features.RemoveCartItem;
using Cart.BusinessLayer.Features.UpdateCart;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CART.API.EndPoint
{
    public class CartEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/test-product/{id}", async (int id,IProductService productService,CancellationToken cancellationToken) =>
            {
                var product = await productService.GetProductByIdAsync(id,cancellationToken);

                return product is null
                    ? Results.NotFound()
                    : Results.Ok(product);
            });

            app.MapPost("/cart/items",
            async ([FromBody] AddToCartCommand command, ISender sender) =>
            {
                var result = await sender.Send(command);

                return ApiResponse<AddToCartResponse>.Success(result);
            }).RequireAuthorization();

            app.MapPost("/test-products",async (List<int> ids,IProductService service,CancellationToken cancellationToken) =>
                {
                    var result = await service.GetProductsAsync(ids,cancellationToken);

                    return Results.Ok(result);
                });

            app.MapGet("/cart/getcartitems",async (ISender sender) =>
               {
                   var result = await sender.Send(new GetCartItemsRequest());

                   return Results.Ok(result);
               }).RequireAuthorization();

            app.MapPatch("/cart/updatecart", async (UpdateCartRequest request,ISender sender) =>
            {
                var result=await sender.Send(request);
                return ApiResponse<UpdateCartResponse>.Success(result);

            }).RequireAuthorization();


            app.MapDelete("/cart/removeCartItem/{cartItemId:int}", async ([FromRouteAttribute] int cartItemId, ISender sender) =>
            {
                var result = await sender.Send(new RemoveCartItemQuery(cartItemId));

                return ApiResponse<RemoveCartItemResponse>.Success(result);

            }).RequireAuthorization().WithName("RemoveCartItem")
              .WithSummary("Remove Cart Item");


            app.MapDelete("/cart/clear",async (ISender sender) =>
            {
                    var result = await sender.Send(
                        new ClearCartCommand());

                    return Results.Ok(result);
            }).RequireAuthorization()
              .WithName("ClearCart")
              .WithSummary("Clear User Cart");

            app.MapPost("/cart/checkout", async ([FromBody] CheckOutCommand request, ISender sender) =>
            {
                var result = await sender.Send(request);

                return Results.Accepted("/orders",result);
            }).RequireAuthorization().WithName("Cart CheckOut");

        }
    }
}
