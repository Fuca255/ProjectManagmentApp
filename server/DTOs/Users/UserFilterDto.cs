using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.DTOs.Users
{
    public class UserFilterDto
    {
        public string SearchUser { get; set; } = string.Empty;
        public List<int> RoleFilter { get; set; } = new List<int>(); 
        public List<int> IsActivatedFilter { get; set; } = new List<int>(); 
    }
}