using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.DTOs.Role;

namespace server.DTOs.Users
{
    public class ProjectUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public string PictureType { get; set; } = string.Empty;
        public RoleDto Role { get; set; } = null!;
    }
}