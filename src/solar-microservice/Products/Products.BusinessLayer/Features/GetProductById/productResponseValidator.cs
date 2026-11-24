using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.GetProductById
{
    public class productResponseValidator : AbstractValidator<GetProductByIdRequest>
    {
        public productResponseValidator()
        {
            RuleFor(product => product.productId).NotEmpty().WithMessage("Product Id is required");
        }
    }
}
