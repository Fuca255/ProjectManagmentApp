using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DTOs.Assignment
{
    public class DependentAssignmentDto
    {
        public int Id { get; set; }   
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsClosed { get; set; }
        public float Percentage { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TaskGroupDto TaskGroup { get; set; } = null!;
    }
}