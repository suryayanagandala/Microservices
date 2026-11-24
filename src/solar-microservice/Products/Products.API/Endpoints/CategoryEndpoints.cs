using BuildingBlocks.Utility;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.BusinessLayer.Features.CreateCategory;
using Products.BusinessLayer.Features.DeleteCategory;
using Products.BusinessLayer.Features.GetParentCategories;
using Products.BusinessLayer.Features.RestoreCategory;
using Products.CoreModels.Dto_s;
using Products.CoreModels.Entites;

namespace Products.API.Endpoints
{
    public class CategoryEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/categories/addcategory", async ([FromForm]CreateCategoryCommand request, ISender sender) =>
            {
                UploadFile? uploadFile = null;

                if(request.Image is not null)
                {
                    uploadFile = new UploadFile(
                        request.Image.OpenReadStream(),
                        request.Image.FileName,
                        request.Image.ContentType,
                        request.Image.Length
                        );
                }

                var command = new CreateCategoryRequest(
                    request.CategoryName,
                    request.Description,
                    request.ParentCategoryId,
                    
                    uploadFile,
                    request.CategoryId
                    );

                var response = await sender.Send(command);

                return Results.Ok(BuildingBlocks.Utility.ApiResponse<CreateCategoryResponse>.Success(response, "sucessfully created category"));
            }).DisableAntiforgery();

            app.MapPut("/categories/updateCategory/{categoryId:int}", async ([FromRoute] int categoryId,
                      [FromForm] CreateCategoryCommand request,ISender sender) =>
            {
                var upload = request.Image is null
                    ? null
                    : new UploadFile(
                        request.Image.OpenReadStream(),
                        request.Image.FileName,
                        request.Image.ContentType,
                        request.Image.Length);

                var command = new CreateCategoryRequest(
                    request.CategoryName,
                    request.Description,
                    request.ParentCategoryId,

                    upload,
                    categoryId
                    );

                var response = await sender.Send(command);
                return Results.Ok(BuildingBlocks.Utility.ApiResponse<CreateCategoryResponse>.Success(response, "sucessfully updated category"));
            }).DisableAntiforgery();

            app.MapGet("/categories/getparentcategories", async ([AsParameters]GetParentCategoriesRequest request, ISender sender) =>
            {
                var response= await sender.Send(request);

                return Results.Ok(ApiResponse<IReadOnlyList<ParentCategoryResponse>>.Success(response));
            });

            app.MapDelete("/categories/delete/{categoryId:int}",
              async ([FromRoute] int categoryId, [FromBody] string reason, ISender sender,
                    CancellationToken cancellationToken) =>
              {
                  var command = new DeleteCategoryRequest(categoryId, reason);

                  var response = await sender.Send(
                      command,
                      cancellationToken);

                  return Results.Ok(ApiResponse<DeleteCategoryResponse>.Success(
                      response,
                      "Category deleted successfully"));
              }).RequireAuthorization(policy =>
              {
                  policy.RequireRole("Admin");
              });


            app.MapPatch("/categories/restore/{categoryId:int}",
            async (
                [FromRoute] int categoryId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new RestoreCategoryRequest(categoryId);

                await sender.Send(command, cancellationToken);

                return Results.Ok(ApiResponse<object>.Success(
                    null,
                    "Category restored successfully"));
            }).RequireAuthorization(policy =>
            {
                policy.RequireRole("Admin");
            });
        }
    }
}
