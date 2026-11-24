using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.UpdatePassword
{
    public class UpdatePasswordValidators:AbstractValidator<UpdatePasswordRequest>
    {
        public UpdatePasswordValidators()
        {
            RuleFor(x => x.email).NotEmpty().EmailAddress().WithMessage("Please enter valid email");
            RuleFor(x => x.oldPassword).NotEmpty().NotNull().WithMessage("Please enter valid old password");
            RuleFor(x => x.newPassword).NotEmpty().NotNull().WithMessage("Please enter valid new password");


        }
    }
}
