using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
using server.Data;
using server.DTOs;
using server.DTOs.Projects;
using server.DTOs.Users;
using server.Interfaces;
using server.Models;

namespace server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ProjectManagmentDbContext _context;
        public UserRepository(ProjectManagmentDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllUsersAsync(UserFilterDto? filter = null, SortDto? sort = null)
        {
            var user_query = _context.Users
            .Include(u => u.Role)
            .AsQueryable();
            return await GetAllFilteredUsersAsync(user_query, filter, sort);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.Include(u=>u.Role).FirstOrDefaultAsync(u=>u.Id==id);
        }

        public async Task<List<User>> GetAllFilteredUsersAsync(IQueryable<User> usersQuery, UserFilterDto? filter,SortDto? sort = null)
        {
                if (filter != null)
                {
                    if (!string.IsNullOrEmpty(filter.SearchUser))
                    {
                        var searchTerms = filter.SearchUser.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var term in searchTerms)
                        {
                            usersQuery = usersQuery.Where(u => EF.Functions.Like(u.Name, $"%{term}%") ||
                                                                EF.Functions.Like(u.Lastname, $"%{term}%"));
                        }
                    }

                    if (filter.RoleFilter != null && filter.RoleFilter.Count > 0)
                    {
                        usersQuery = usersQuery.Where(u => filter.RoleFilter.Contains(u.Role.Id));
                    }

                    if (filter.IsActivatedFilter != null && filter.IsActivatedFilter.Count > 0)
                    {
                        usersQuery = usersQuery.Where(u => filter.IsActivatedFilter.Contains(u.IsActivated ? 1 : 0));
                    }
                }
                if (sort != null)
                {
                    switch (sort.PropertyName?.ToLower())
                    {
                        case "firstname":
                            usersQuery = sort.SortFlag == 1? usersQuery.OrderBy(u => u.Name) : usersQuery.OrderByDescending(u => u.Name);
                            break;
                        case "lastname":
                            usersQuery = sort.SortFlag == 1? usersQuery.OrderBy(u => u.Lastname) : usersQuery.OrderByDescending(u => u.Lastname);
                            break;
                        case "role":
                            usersQuery = sort.SortFlag == 1? usersQuery.OrderByDescending(u => u.Role.Name) : usersQuery.OrderBy(u => u.Role.Name);
                            break;
                        case "isactive":
                            usersQuery = sort.SortFlag == 1? usersQuery.OrderByDescending(u => u.IsActivated) : usersQuery.OrderBy(u => u.IsActivated);
                            break;
                        default:
                            usersQuery = sort.SortFlag == 1? usersQuery.OrderByDescending(u => u.Id) : usersQuery.OrderBy(u => u.Id);
                            break;
                    }
                }
                else
                {
                    usersQuery = usersQuery.OrderByDescending(u => u.Id);
                }

                return await usersQuery.ToListAsync();
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<List<User>> GetUsersByRoleAsync(string roleName)
        {
            return await _context.Users
                .Where(u=> u.Role!= null && u.Role.Name==roleName)
                .ToListAsync();
        }

        public async Task<Role?> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        // public async Task<User?> GetUserByUsernameAsync(string username)
        // {
        //     return await _context.Users.SingleOrDefaultAsync(user => user.Username == username);
        // }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByResetTokenAsync(string resetToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == resetToken);
        }

        public async Task<bool> DeleteUserByIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false; // User with the specified ID not found
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true; // User deleted successfully
        }

        public async Task<User?> UpdateUserAsync(UpdateUserDto dto, int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if(user == null)
                return null;
            user.Organisation =dto.Organisation;
            user.Department = dto.Department;
            user.IsActivated = dto.IsActivated;
            user.Username = dto.Username;
            user.Name = dto.Name;
            user.Lastname = dto.Lastname;
            user.RoleId = dto.RoleId;
            var role = await _context.Roles.FindAsync(dto.RoleId);
            if (role != null)
            {
                user.JobTitle = role.Name;
            }
            await _context.SaveChangesAsync();

            return user;
        }
        /*
        public async Task DeleteProfilePictureAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return; // Or throw an exception or handle accordingly
            }

            user.ProfilePicturePath = string.Empty; // Set the profile picture path to empty string to remove it
            await _context.SaveChangesAsync();
        }
        */
        public async Task<List<User>> GetUserByProjectId(int project_id)
        {
            var users = await _context.Users.Include(u=>u.ProjectUsers).ThenInclude(u=>u.Role).ToListAsync();
            List<User> chosenUsers = new List<User>();
            foreach (var user in users)
            {
                var project_users = user.ProjectUsers.Where(u=>u.ProjectId==project_id).ToList();
                if(project_users.Count > 0)
                    chosenUsers.Add(user);
            }
            return chosenUsers;
        }

        public async Task<List<User>> GetAssignmentUsersAsync(int task_id)
        {
            var asign = await _context.Assignments.Include(a=>a.Users).ThenInclude(u=>u.Role).FirstOrDefaultAsync(a=>a.Id==task_id);
            if (asign == null)
                return new List<User>();
            
            return asign.Users.Where(u=>u.IsActivated).ToList();
        }
    }
}