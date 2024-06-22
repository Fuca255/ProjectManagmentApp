using server.DTOs.Permissions;

namespace server.Interfaces
{
    public interface IUserProjectPermissionService
    {
        Task AddUserProjectPermissions(UserProjectPermissionArrayDto userProjectPermissionArrayDto);
    }
}
