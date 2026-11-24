using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.LoginUser
{
    public record LoginUserResponse(string UserId, string Email, string Role, string AccessToken,
        string RefreshToken);
}
