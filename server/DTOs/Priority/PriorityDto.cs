using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DTOs.Priority
{
    public class PriorityDto
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public string Description { get; set; } = null!;
    }
}