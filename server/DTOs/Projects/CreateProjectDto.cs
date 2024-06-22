using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DTOs.Projects
{
    public class CreateProjectDto
    {
        public int OwnerId { get; set; }
        public List<int> UserIds { get; set; } = new List<int>();
        public List<int> UserProjectRoleIds { get; set; } = new List<int>();
        public int PriorityId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public double Budget { get; set; }
        public string Description { get; set; } = "";
        public string Type { get; set; } = null!;
    }
}