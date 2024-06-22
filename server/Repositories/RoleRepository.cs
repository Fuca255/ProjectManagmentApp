using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Interfaces;
using server.Models;

namespace server.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ProjectManagmentDbContext _context;
        public RoleRepository(ProjectManagmentDbContext context)
        {
            _context = context;
        }
        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(int role_id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == role_id);
        }
        public async Task<List<Permission>> GetPermissionsByRoleIdAsync(int roleId)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissions)  // Include RolePermissions navigation property
                    .ThenInclude(rp => rp.Permission)  // Include Permission navigation property within RolePermissions
                .FirstOrDefaultAsync(r => r.Id == roleId);

            return role?.RolePermissions.Select(rp => rp.Permission).ToList() ?? new List<Permission>();
        }

        


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Role?> GetUserRoleOnProject(int projectId, int userId)
        {
            var project_user = await _context.ProjectUsers.Include(pu=>pu.Role).FirstOrDefaultAsync(pu => pu.ProjectId==projectId && pu.UserId==userId);
            
            return project_user?.Role;
        }
    }
}