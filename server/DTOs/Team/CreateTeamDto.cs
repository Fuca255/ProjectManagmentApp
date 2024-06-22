using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.DTOs.Team
{
    public class CreateTeamDto
    {
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public int CreatedBy { get; set; } = 0;

        public List<int> Members { get; set; } =  new List<int>();
    }
}