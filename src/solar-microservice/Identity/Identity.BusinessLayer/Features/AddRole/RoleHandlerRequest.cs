using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.AddRole
{
    public record RoleHandlerRequest(int roleId,string roleName):ICommand<RoleResponse>;
}
