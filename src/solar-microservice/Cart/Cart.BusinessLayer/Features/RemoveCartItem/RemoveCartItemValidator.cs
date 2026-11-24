using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.BusinessLayer.Features.RemoveCartItem
{
    public class RemoveCartItemValidator:AbstractValidator<RemoveCartItemQuery>
    {
        public RemoveCartItemValidator()
        {
            RuleFor(x=>x.cartItemId).NotEmpty();

            RuleFor(x => x.cartItemId).GreaterThan(0);
        }
    }
}
