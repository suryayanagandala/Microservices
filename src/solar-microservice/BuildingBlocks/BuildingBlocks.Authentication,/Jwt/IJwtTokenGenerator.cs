using BuildingBlocks.Authentication.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Authentication_.Jwt
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(JwtUser jwtUser);

        string GenerateRefreshToken();
    }
}
