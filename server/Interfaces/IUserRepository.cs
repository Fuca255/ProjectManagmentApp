using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.DTOs;
using server.DTOs.Projects;
using server.DTOs.Users;
using server.Models;

namespace server.Interfaces
{
    public interface IUserRepository
    {

        Task AddUserAsync(User user);

        Task<List<User>> GetUserByProjectId(int project_id);
        Task<List<User>> GetAssignmentUsersAsync(int task_id);

        Task<Role?> GetRoleByNameAsync(string roleName);

        Task <List<User>> GetAllUsersAsync(UserFilterDto? filter=null,SortDto? sort = null);

        Task<User?> GetUserByIdAsync(int id);

        Task<List<User>> GetUsersByRoleAsync(string roleName);

        Task<User?> GetUserByUsernameAsync(string username);

        Task<User?> GetUserByEmailAsync(string email);

        Task SaveChangesAsync();

        Task<User?> GetUserByResetTokenAsync(string resetToken);

        Task<bool> DeleteUserByIdAsync(int userId);

        Task<User?> UpdateUserAsync(UpdateUserDto dto,int user_id);

        //Task DeleteProfilePictureAsync(int userId);

    }
}