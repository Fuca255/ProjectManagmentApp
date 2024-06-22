using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using server.Data;
using System.Security.Claims;

namespace server.Authorization
{
    public class HasPermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly string _permission;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ProjectManagmentDbContext _context;

        public HasPermissionFilter(string permission, IHttpContextAccessor httpContextAccessor, ProjectManagmentDbContext context)
        {
            _permission = permission;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch user's role and associated permissions
            var role = await _context.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Users.Any(u => u.Id.ToString() == userId));

            // Check if the required permission exists in the role's permissions
            if (role != null && role.RolePermissions.Any(rp => rp.Permission.Name == _permission))
            {
                // User has the required permission
                return;
            }

            // User does not have the required permission
            context.Result = new ForbidResult();
        }
    }
}
