using BuildingBlocks.Utility;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.BusinessLayer.Features.CreateProduct;
using Products.BusinessLayer.Features.CreateProductImage;
using Products.BusinessLayer.Features.DeleteProduct;
using Products.BusinessLayer.Features.DeleteProductImage;
using Products.BusinessLayer.Features.GetAllProducts;
using Products.BusinessLayer.Features.GetProductById;
using Products.BusinessLayer.Features.GetProductForCart;
using Products.BusinessLayer.Features.GetProductsForCartQuery;
using Products.BusinessLayer.Features.RestoreProduct;
using Products.BusinessLayer.Features.UpdateProduct;
using Products.CoreModels.Dto_s;
using Products.CoreModels.Entites;
using System.Reflection;

namespace Products.API.Endpoints
{
    public sealed class ProductEndPoint : ICarterModule
    {
        private sealed record ProductImageCommand(int productId,List<IFormFile> image);

        private sealed record UpdateProductImage(int productId,IFormFile image);
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products/addproduct", async (CreateProductRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);

                return ApiResponse<CreateProductResponse>.Success(response, "product created successfully");
            }).RequireAuthorization(policy =>
            {
                policy.RequireRole("Admin");
            });

            app.MapPut("/products/updateproduct/{productId:int}", async ([FromRouteAttribute] int productId, CreateProductRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new UpdateProductRequest(productId, request);

                var response = await sender.Send(command, cancellationToken);

                return ApiResponse<UpdateProductResponse>.Success(response, "productUpdatedSuccesfully");

            }).RequireAuthorization(policy =>
            {
                policy.RequireRole("Admin");
            });


            app.MapGet("/products/getallproducts", async ([AsParameters]GetAllProductsRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                var response=await sender.Send(request, cancellationToken);
                return ApiResponse<IReadOnlyList<GetProductsDto>>.Success(response, "get all products");
            });


            app.MapGet("/products/getproduct/{productId}", async ([FromRouteAttribute] int productId, ISender sender, CancellationToken cancellationToken) =>
            {
                var request = new GetProductByIdRequest(productId);

                var response = await sender.Send(request, cancellationToken);

                return ApiResponse<GetProductByIdResponse>.Success(response, "product retrieved successfully");
            });

            app.MapDelete("/products/deleteproduct/{productId:int}", async ([FromRoute] int productId, string reason, ISender sender, CancellationToken cancellationToken) =>
            {
                var request = new DeleteProductRequest(productId, reason);
                var response = await sender.Send(request, cancellationToken);
                return ApiResponse<DeleteProductResponse>.Success(response, "product deactivated sucessfully");
            }).RequireAuthorization(policy =>
            {
                policy.RequireRole("Admin");
            });

            app.MapPost("/products/restoreproduct/{productId:int}", async (int productId, ISender sender, CancellationToken cancellationToken) =>
            {
                var request = new RestoreProductRequest(productId);

                var response = await sender.Send(request, cancellationToken);

                return ApiResponse<RestoreProductResponse>.Success(response, "product restored succesfully");
            }).RequireAuthorization(policy =>
            {
                policy.RequireRole("Admin");
            });


            app.MapPost("/productImages/addimage", async ([FromFormAttribute] ProductImageCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                var files = command.image.Select(c => new UploadFile(
                    c.OpenReadStream(), c.FileName, c.ContentType, c.Length)).ToList();

                var request = new CreateProductImageRequest(command.productId, files, null);

                var response = await sender.Send(request, cancellationToken);

                return ApiResponse<CreateProductImageResponse>.Success(response, "image is sucesfully created");

            }).DisableAntiforgery().RequireAuthorization(policy =>
            {
                policy.RequireRole("Admin");
            });


            app.MapPut("/productImages/updateimage/{imageId:int}", async ([FromRouteAttribute] int imageId, [FromFormAttribute] UpdateProductImage command, ISender sender, CancellationToken cancellationToken) =>
            {
                var upload = new UploadFile(
                    command.image.OpenReadStream(),
                    command.image.FileName,
                    command.image.ContentType,
                    command.image.Length
                    );

                var request = new CreateProductImageRequest(command.productId,new List<UploadFile> {upload},imageId);

                var response = await sender.Send(request, cancellationToken);

                return ApiResponse<CreateProductImageResponse>.Success(response, "image is sucesfully created");

            }).DisableAntiforgery().RequireAuthorization(policy =>
            {
                policy.RequireRole("Admin");
            });

            app.MapDelete("/productImages/deleteimage/{imageId:int}", async ([FromRouteAttribute] int imageId, ISender sender, CancellationToken cancellationToken) =>
            {
                var response=await sender.Send(new DeleteProductImageRequest(imageId));

                return ApiResponse<DeleteProductImageResponse>.Success(response, "image removed sucesfully");
            }).RequireAuthorization(policy =>
            {
                policy.RequireRole("Admin");
            });

            app.MapGet("/products/cart/{productId:int}",
        async (int productId, ISender sender) =>
        {
            var result = await sender.Send(
                new GetProductForCartQuery(productId));

            return Results.Ok(result);
        })
        .WithName("GetProductForCart")
        .Produces<GetProductForCartResult>()
        .WithSummary("Get Product For Cart");

            app.MapPost("/products/cart",
            async (
                GetProductsForCartQuery query,
                ISender sender) =>
            {
                var result = await sender.Send(query);

                return Results.Ok(result);
            })
        .WithName("GetProductsForCart")
        .WithSummary("Get Products For Cart")
        .Produces<List<GetProductsForCartResponse>>();



        }




    }
}
