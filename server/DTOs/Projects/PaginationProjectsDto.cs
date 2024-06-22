using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DTOs.Projects
{
    public class PaginationProjectsDto
    {
        public List<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
        public int MaxProjects { get; set; }
    }
}