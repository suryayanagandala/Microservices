using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.DeleteProduct
{
    public class DeleteProductValidator:AbstractValidator<DeleteProductRequest>
    {
        public DeleteProductValidator()
        {
            RuleFor(x => x.productId).NotEmpty().WithMessage("ProductId is required");

            RuleFor(x => x.reason).NotEmpty().MinimumLength(8).WithMessage("Reason is required");
        }
    }
}
