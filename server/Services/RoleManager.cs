using server.Data;
using server.Models;

namespace server.Services
{
    public class RoleManager
    {
        private readonly ProjectManagmentDbContext _context;

        public RoleManager(ProjectManagmentDbContext context)
        {
            _context = context;
        }

        public void AddRole(string roleName)
        {
            // Create a new Role instance
            var newRole = new Role
            {
                Name = roleName
            };

            // Add the new role to the DbSet
            _context.Roles.Add(newRole);

            // Save changes to the database
            _context.SaveChanges();
        }
    }
}

