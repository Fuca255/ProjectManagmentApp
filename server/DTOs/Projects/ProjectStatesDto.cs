using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DTOs.Projects
{
    public class ProjectStatesDto
    {
        public int StateId { get; set; }

        public string StateName { get; set; } = string.Empty;
        public int count { get; set; }

        public float Percentage { get; set; }
    }
}