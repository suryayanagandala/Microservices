using Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Interfaces
{
    public interface IRoleInterface
    {
        Task<Roles?> GetRoleById(int roleId);

        Task<Roles?> GetRoleByRoleName(string roleName);

        Task<IEnumerable<Roles>> GetRoles(string? roleName, int? roleId);

        Task AddRole(Roles role);
    }
}
