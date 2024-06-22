using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DTOs.Users
{
    public class UpdateUserDto
    {
        public string Name { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string JobTitle { get; set; } =string.Empty;
        public string Organisation { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public bool IsActivated { get; set; }
    }
}