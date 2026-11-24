using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.UpdatePassword
{
public record UpdatePasswordRequest(string email,string oldPassword,string newPassword)
        :ICommand<UpdatePasswordResponse>;
}
