using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DTOs.Assignment
{
    public class CreateTaskGroupDto
    {
        public string Title { get; set; } = null!;
        public int ProjectId { get; set; }
        public int? ParentTaskGroupId { get; set; } = null;
    }
}