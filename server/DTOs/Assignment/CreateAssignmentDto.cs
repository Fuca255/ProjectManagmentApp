using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DTOs.Assignment
{
    public class CreateAssignmentDto
    {
        public int Owner { get; set; }
        public int TaskGroupId { get; set; }
        public List<int> Assignees { get; set; } = new List<int>();
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<int> DependentOn { get; set; } = new List<int>();
        public string Title { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int PriorityId { get; set; }
        public int StateId { get; set; }
        
    }
}