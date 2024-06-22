using server.DTOs.Permissions;
using server.Models;

namespace server.Mappers
{
    public class UserProjectPermissionMapper
    {
        public static UserProjectPermission MapToEntity(UserProjectPermissionDto dto)
        {
            return new UserProjectPermission
            {
                UserId = dto.UserId,
                ProjectId = dto.ProjectId,
                PermissionId = dto.PermissionId
            };
        }
    }
}
