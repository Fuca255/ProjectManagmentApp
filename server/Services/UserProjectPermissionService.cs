using server.DTOs.Permissions;
using server.Models;

namespace server.Services
{
    public class UserProjectPermissionService:IUserProjectPermissionService
    {
        private readonly IUserProjectPermissionRepository _userProjectPermissionRepository;

        public UserProjectPermissionService(IUserProjectPermissionRepository userProjectPermissionRepository)
        {
            _userProjectPermissionRepository = userProjectPermissionRepository;
        }

        public async Task AddUserProjectPermissions(UserProjectPermissionArrayDto userProjectPermissionArrayDto)
        {
            // Extract individual UserProjectPermissionDto objects
            var userProjectPermissions = userProjectPermissionArrayDto.Permissions.Select(dto => new UserProjectPermission
            {
                UserId = dto.UserId, // Assuming you have the user ID
                ProjectId = dto.ProjectId, // Assuming you have the project ID
                PermissionId = dto.PermissionId
            });

            // Add permissions to the repository (bulk insertion)
            await _userProjectPermissionRepository.AddUserProjectPermissions(userProjectPermissions);
        }
    }
}
