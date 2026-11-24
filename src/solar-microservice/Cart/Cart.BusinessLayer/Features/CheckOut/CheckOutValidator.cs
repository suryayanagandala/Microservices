using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.BusinessLayer.Features.CheckOut
{
    internal class CheckOutValidator:AbstractValidator<CheckOutCommand>
    {
        public CheckOutValidator()
        {
            RuleFor(x=>x.FullName).NotEmpty().WithMessage("Name is required");

            RuleFor(x=>x.PhoneNumber).MaximumLength(10).MinimumLength(0).NotEmpty();

            RuleFor(x => x.DeliveryAddress).NotEmpty().WithMessage("Addres is required");
           
            
        }
    }
}
