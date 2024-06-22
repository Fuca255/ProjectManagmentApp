using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.DTOs.Users;
using server.Models;

namespace server.DTOs.Team
{
    public class TeamDto
    {

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int CreatedBy { get; set; }

        public List<UserRoleDto> Members { get; set; } =  new List<UserRoleDto>();
    }
}