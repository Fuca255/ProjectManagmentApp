using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Repositories
{
    public class UserProjectRolesRepository : IUserProjectRolesRepository
    {
        private readonly ProjectManagmentDbContext _context;
        public UserProjectRolesRepository(ProjectManagmentDbContext context)
        {
            _context = context;
        }
        public async Task<UserProjectRoles> AddUserProjectRoles(UserProjectRoles userProjectRoles)
        {
            _context.UserProjectRoles.Add(userProjectRoles);
            await _context.SaveChangesAsync();
            return userProjectRoles;
        }

        public async Task<IEnumerable<UserProjectRoles>> GetAllUserProjectRoles()
        {
            return await _context.UserProjectRoles.ToListAsync();
        }

        public async Task<UserProjectRoles?> GetUserProjectRolesById(int id)
        {
            return await _context.UserProjectRoles.FindAsync(id);
        }
    }
}
