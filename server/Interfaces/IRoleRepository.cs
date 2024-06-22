using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAllRolesAsync();
        Task<Role?> GetRoleByIdAsync(int roleId);
        Task SaveChangesAsync();
        Task<List<Permission>> GetPermissionsByRoleIdAsync(int roleId);
        Task<Role?> GetUserRoleOnProject(int projectId, int userId);
    }
}