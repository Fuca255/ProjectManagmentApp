using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DTOs.Assignment
{
    public class AssignmentFilterDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchTitle { get; set; } = string.Empty;
        public DateTime? StartFrom { get; set; } = null;
        public DateTime? EndFrom { get; set; } = null;
        public DateTime? StartTo { get; set; } = null;
        public DateTime? EndTo { get; set; } = null;
        public List<int> PriorityFilter { get; set; } = new List<int>(); // po prioritetu >1 and <5
        public List<int> StateFilter { get; set; } = new List<int>(); //po state-u >1 and <7
        public int PercentageFilterFrom { get; set; } = -1;
        public int PercentageFilterTo { get; set; }=-1;
    }
}