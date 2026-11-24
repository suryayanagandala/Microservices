using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.VerifyOtp
{
    public class VerifyOtpValidator:AbstractValidator<VerifyOtpRequest>
    {
        public VerifyOtpValidator()
        {
            RuleFor(x => x.email).NotEmpty().EmailAddress().WithMessage("Email is required").NotNull();

            RuleFor(x => x.otptext).NotEmpty().NotNull().WithMessage("Otp is required");
        }
    }
}
