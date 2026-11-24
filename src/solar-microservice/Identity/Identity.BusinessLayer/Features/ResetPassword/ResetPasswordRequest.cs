using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.ResetPassword
{
    public record ResetPasswordReq(Guid guid,string password,string resetPassword)
        :ICommand<ResetPasswordResponse>;
}
