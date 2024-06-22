using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DTOs.Role
{
    public class RoleDto
    {
        public int Id { get; set; }
        public int Authority { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}