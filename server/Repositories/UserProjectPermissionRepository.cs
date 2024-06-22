using server.Data;
using server.Models;

namespace server.Repositories
{
    public class UserProjectPermissionRepository : IUserProjectPermissionRepository
    {
        private readonly ProjectManagmentDbContext _context;

        public UserProjectPermissionRepository(ProjectManagmentDbContext context)
        {
            _context = context;
        }
        public async Task AddUserProjectPermission(UserProjectPermission userProjectPermission)
        {
            _context.UserProjectPermission.Add(userProjectPermission);
            await SaveChangesAsync();
        }

        public async Task AddUserProjectPermissions(IEnumerable<UserProjectPermission> userProjectPermissions)
        {
            _context.UserProjectPermission.AddRange(userProjectPermissions);
            await SaveChangesAsync();
        }

        public async Task<UserProjectPermission> GetUserProjectPermissionById(int id)
        {
            return await _context.UserProjectPermission.FindAsync(id);
        }

        public async Task RemoveUserProjectPermission(UserProjectPermission userProjectPermission)
        {
            _context.UserProjectPermission.Remove(userProjectPermission);
            await SaveChangesAsync();
        }
        private async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
