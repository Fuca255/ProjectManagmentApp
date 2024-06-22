using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.DTOs.Role;
using server.DTOs.Users;
using server.Models;

namespace server.Mappers
{
    public static class UserMapper
    {
        public static UserDto toUserDto(this User u)
        {
            var user = new UserDto{
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                JobTitle = u.JobTitle,
                Organisation = u.Organisation,
                Department = u.Department,
                Name = u.Name,
                Lastname = u.Lastname,
                IsActivated = u.IsActivated,
            };
            if(u.ProfilePicture != null)
            {
                user.PictureType = u.PictureType;
                user.ProfilePicture = Convert.ToBase64String(u.ProfilePicture);
            }
            return user;
        }
        public static AssignmentUserDto toAssignmentUserDto(this User u)
        {
            var user = new AssignmentUserDto{
                Id = u.Id,
                Username = u.Username,
                Name = u.Name,
                Lastname = u.Lastname
            };
            if(u.ProfilePicture != null)
            {
                user.PictureType = u.PictureType;
                user.ProfilePicture = Convert.ToBase64String(u.ProfilePicture);
            }

            return user;
        }

        public static ProjectUserDto toProjectUserDto(this User u, RoleDto role)
        {
            var user = new ProjectUserDto{
                Id = u.Id,
                Username = u.Username,
                Name = u.Name,
                Lastname = u.Lastname,
                Role = role
            };
            if(u.ProfilePicture != null)
            {
                user.PictureType = u.PictureType;
                user.ProfilePicture = Convert.ToBase64String(u.ProfilePicture);
            }

            return user;
        }

        public static UserRoleDto toUserRoleDto(this User u, RoleDto role)
        {
            var user = new UserRoleDto{
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                JobTitle = u.JobTitle,
                Organisation = u.Organisation,
                Department = u.Department,
                Name = u.Name,
                Lastname = u.Lastname,
                IsActivated = u.IsActivated,
                Role = role
            };

            if(u.ProfilePicture != null)
            {
                user.PictureType = u.PictureType;
                user.ProfilePicture = Convert.ToBase64String(u.ProfilePicture);
            }

            return user;
        }
    }
}