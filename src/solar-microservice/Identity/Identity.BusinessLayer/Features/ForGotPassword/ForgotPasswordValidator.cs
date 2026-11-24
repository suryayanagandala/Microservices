using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.ForGotPassword
{
    public class ForgotPasswordValidator:AbstractValidator<ForgotPasswordReq>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.email).NotEmpty().NotNull().EmailAddress().WithMessage("Please enter email");
            
        }
    }
}
