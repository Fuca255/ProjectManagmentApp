using server.Models;

namespace server.Interfaces
{
    public interface IUserProjectRolesRepository
    {
        Task<IEnumerable<UserProjectRoles>> GetAllUserProjectRoles();
        Task<UserProjectRoles?> GetUserProjectRolesById(int id);
        Task<UserProjectRoles> AddUserProjectRoles(UserProjectRoles userProjectRoles);
    }
}
