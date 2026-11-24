using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.ResetPassword
{
    public class ResetPasswordValidators:AbstractValidator<ResetPasswordReq>

    {
        public ResetPasswordValidators()
        {
            RuleFor(x => x.guid).NotEmpty().NotNull().WithMessage("Token is required");
            RuleFor(x => x.password).NotEmpty().NotNull().WithMessage("Passwprd is required");
            RuleFor(x => x.resetPassword).NotEmpty().NotNull().WithMessage("Reset Password is required");
        }
    }
}
