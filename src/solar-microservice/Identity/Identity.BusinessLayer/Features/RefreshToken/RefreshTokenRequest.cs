using BuildingBlocks.CQRS;
using Identity.BusinessLayer.Features.LoginUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.RefreshToken
{
    public record RefreshTokenRequest(string refreshToken)
        :ICommand<LoginUserResponse>;
}
