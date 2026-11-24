using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Authentication.Jwt
{
    public static class JwtClaimNames
    {
        public const string UserId = "sub";

        public const string Email = "email";

        public const string Role = "role";

        public const string JwtId = "jti";
    }
}
