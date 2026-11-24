using BuildingBlocks.CQRS;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.CreateCategory
{
    public sealed record CreateCategoryRequest
    ( string categoryName,string description,int? parentCatgoryId,
        CoreModels.Entites.UploadFile? image,int? categoryId
        ):ICommand<CreateCategoryResponse>;

    public sealed class CreateCategoryCommand
    {
        public string CategoryName { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;

        public int? ParentCategoryId { get; init; }

        public int? CategoryId { get; init; }

        public IFormFile? Image { get; init; }
    }
}
