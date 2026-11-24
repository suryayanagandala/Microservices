using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.BusinessLayer.Features.CancelOrder
{
    internal class CancelOrderValidator:AbstractValidator<CancelOrderRequest>
    {
        public CancelOrderValidator()
        {
            RuleFor(x => x.orderId).GreaterThan(0);
        }
    }
}
