using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.UpdateProfile
{
    public class UpdateProfileValidators:AbstractValidator<UpdateProfileRequest>
    {
        public UpdateProfileValidators()
        {
            RuleFor(x => x.email).NotEmpty().EmailAddress().NotNull().WithMessage("Emai is required");
            RuleFor(x => x.address).NotEmpty().NotNull().WithMessage("Address i srequired");
            RuleFor(x => x.phoneNumber).NotEmpty().NotNull().WithMessage("PhoneNumber is required");
            RuleFor(x => x.userName).NotNull().NotEmpty().WithMessage("Password is required");
        }
    }
}
