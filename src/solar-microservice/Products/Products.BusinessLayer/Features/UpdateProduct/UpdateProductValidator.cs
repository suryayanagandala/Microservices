using FluentValidation;
using Products.BusinessLayer.Features.CreateProduct;
using Products.BusinessLayer.Features.CreateProductImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.UpdateProduct
{
    public class UpdateProductValidator:AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.productId).NotEmpty().WithMessage("ProductId is required");

            RuleFor(x => x.request).SetValidator(new CreateProductValidator());
        }
    }
}
