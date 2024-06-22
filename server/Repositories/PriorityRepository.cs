using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Interfaces;
using server.Models;

namespace server.Repositories
{
    public class PriorityRepository : IPriorityRepository
    {
        private readonly ProjectManagmentDbContext _context;
        public PriorityRepository(ProjectManagmentDbContext context)
        {
            _context = context;
        }
        public async Task<List<Priority>> GetPriorities()
        {
            return await _context.Priorities.ToListAsync();
        }

        public async Task<Priority?> GetPriority(int prio_id)
        {
            return await _context.Priorities.FirstOrDefaultAsync(p=> p.Id == prio_id);
        }
    }
}