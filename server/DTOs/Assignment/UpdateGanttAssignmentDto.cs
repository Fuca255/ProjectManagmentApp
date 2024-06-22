using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DTOs.Assignment
{
    public class UpdateGanttAssignmentDto
    {
        public long StartTs { get; set; } = 0;
        public long EndTs { get; set; } = 0;
        public List<int> AddDependentOn { get; set; } = new List<int>();
        public List<int> RemoveDependentOn { get; set; } = new List<int>();
        public int StateId { get; set; } = 0;
        public float Percentage { get; set; } = -1;
        public string Title { get; set; } = "";
        public string Type { get; set; } ="";
        public string Description { get; set; } = "";
        public int PriorityId { get; set; } = 0;
    }
}