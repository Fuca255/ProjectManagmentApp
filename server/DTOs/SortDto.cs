using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DTOs
{
    public class SortDto
    {
        public string PropertyName { get; set; } = string.Empty;
        public int SortFlag { get; set; }
    }
}