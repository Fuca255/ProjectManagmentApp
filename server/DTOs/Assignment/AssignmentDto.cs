using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.DTOs.Priority;
using server.DTOs.State;
using server.DTOs.Users;
using server.Models;

namespace server.DTOs.Assignment
{
    public class AssignmentDto
    {
        public int Id { get; set; }
        public List<AssignmentUserDto> Assignees {get; set;} = new List<AssignmentUserDto>();
        public List<int> DepndentOn { get; set; } = new List<int>();   
        public string Title { get; set; } = null!;
        public AssignmentUserDto Owner { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public StateDto State { get; set; } = null!;
        public float Percentage { get; set; }
        public PriorityDto Priority { get; set; } = null!;
        public TaskGroupDto TaskGroup { get; set; } = null!;
        public string Type { get; set; } = null!;
        public bool IsClosed { get; set; }
    }
}