using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.DTOs.Priority;
using server.DTOs.State;
using server.DTOs.Users;
using server.Models;

namespace server.DTOs.Projects
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public StateDto State { get; set; } = null!;
        public double Budget { get; set; }
        public double Spent { get; set; }
        public int OwnerId { get; set; }
        public string Type { get; set; } = "";
        public double Percentage { get; set; }
        public int AssignmentCount { get; set; }
        public string Description { get; set; } = "";
        public PriorityDto Priority { get; set; } = null!;
        public List<ProjectUserDto> Users { get; set; } = new List<ProjectUserDto>();

    }
}