using Identity.Application.Interfaces;
using Identity.CoreModels.Persistance;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.DbAccessLayer.Infrastructure
{
    public class RoleRepository : IRoleInterface
    {
        private readonly ApplicationDbContext _db;

        public RoleRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddRole(Roles role)
        {
            await _db.Roles.AddAsync(role);
        }

        public async Task<Roles?> GetRoleById(int roleId)
        {
            return await _db.Roles.FirstOrDefaultAsync(x => x.RoleId == roleId);
        }

        public async Task<Roles?> GetRoleByRoleName(string roleName)
        {
            return await _db.Roles.FirstOrDefaultAsync(x => x.RoleName==roleName);

        }

        public async Task<IEnumerable<Roles>> GetRoles(string? roleName, int? roleId)
        {

            IQueryable<Roles> roles = _db.Roles;

            if (roleId.HasValue)
            {
               roles= roles.Where(x=>x.RoleId==roleId);
            }

            if (!string.IsNullOrEmpty(roleName))
            {
               roles= roles.Where(x=>x.Equals(roleName));
            }

            return await roles.ToListAsync();

        }
    }
}
