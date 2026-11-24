using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.LoginUser
{
    public record LoginUserRequest(string Email,string Password)
        :ICommand<LoginUserResponse>;
}
