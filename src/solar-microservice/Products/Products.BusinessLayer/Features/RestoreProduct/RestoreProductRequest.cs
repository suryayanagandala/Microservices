using BuildingBlocks.CQRS;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.RestoreProduct
{
    public record RestoreProductRequest
    (int productId):ICommand<RestoreProductResponse>;


    public class RestoreProductValidator : AbstractValidator<RestoreProductRequest>
    {
        public RestoreProductValidator()
        {
            RuleFor(x => x.productId).NotEmpty().WithMessage("ProductId is required");
        }
    }
}
