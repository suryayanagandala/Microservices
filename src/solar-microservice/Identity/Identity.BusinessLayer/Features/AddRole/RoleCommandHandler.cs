using BuildingBlocks.CQRS;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.AddRole
{
    public class RoleCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<RoleHandlerRequest, RoleResponse>
    {
        public async Task<RoleResponse> Handle(RoleHandlerRequest request, CancellationToken cancellationToken)
        {
            var roles = new Roles
            {
                RoleId=request.roleId,
                RoleName=request.roleName,
                CreatedAt=DateTimeOffset.Now,
                IsActive=true,
            };
            await unitOfWork.roleInterface.AddRole(roles);

            var save = await unitOfWork.SaveChanges(cancellationToken);
            return new RoleResponse(save);

        }
    }
}
