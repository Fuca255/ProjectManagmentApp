using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Interfaces
{
    public interface IPriorityRepository
    {
        Task<List<Priority>> GetPriorities();
        Task<Priority?> GetPriority(int prio_id);
    }
}