using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DTOs.Assignment
{
    public class PaginationAssignmentsDto
    {
        public List<AssignmentDto> Assignments { get; set; } = new List<AssignmentDto>();
        public int MaxAssignments { get; set; }
    }
}