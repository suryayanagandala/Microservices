using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.BusinessLayer.Features.UpdateCart
{
    internal class UpdateCartValidator:AbstractValidator<UpdateCartRequest>
    {
        public UpdateCartValidator()
        {
            RuleFor(x => x.cartItemId).NotEmpty().WithMessage("CartItemId is required");

            RuleFor(x => x.cartItemId).GreaterThan(0);

            RuleFor(x => x.quantity).GreaterThan(0);
        }
    }
}
