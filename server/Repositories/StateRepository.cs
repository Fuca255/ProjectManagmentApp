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
    public class StateRepository : IStateRepository
    {
        private readonly ProjectManagmentDbContext _context;
        public StateRepository(ProjectManagmentDbContext context)
        {
            _context = context;
        }
        public async Task<List<State>> GetAllStatesAsync()
        {
            return await _context.States.ToListAsync();
        }

        public async Task<State?> GetStateByIdAsync(int state_id)
        {
            return await _context.States.FirstOrDefaultAsync(s=>s.Id==state_id);
        }
    }
}