using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.BusinessLayer.Features.CreatePayment
{
    public sealed class CreatePaymentValidator
    : AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0);

            //RuleFor(x => x.Amount)
            //    .GreaterThan(0);
        }
    }
}
