using server.DTOs.Permissions;
using server.Models;
namespace server.Mappers
{
    public class UserProjectRolesMapper
    {
        public static UserProjectRolesDto MapToDto(UserProjectRoles upr)
        {
            return new UserProjectRolesDto
            {
                Id = upr.Id,
                UserId = upr.UserId,
                ProjectId = upr.ProjectId,
                RoleId = upr.RoleId,
            };
        }

        public static UserProjectRoles MapToEntity(UserProjectRolesDto userProjectRolesDto)
        {
            return new UserProjectRoles
            {
                // Map properties from DTO to entity
                UserId = userProjectRolesDto.UserId,
                ProjectId = userProjectRolesDto.ProjectId,
                RoleId = userProjectRolesDto.RoleId
                // Map other properties as needed
            };
        }
    }
}