using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.RefreshToken
{
    internal class RefreshTokenValidators:AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenValidators()
        {
            RuleFor(x => x.refreshToken).NotEmpty().WithMessage("Refresh token is required");
        }
    }
}
