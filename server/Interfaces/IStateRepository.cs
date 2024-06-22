using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Interfaces
{
    public interface IStateRepository
    {
        Task<State?> GetStateByIdAsync(int state_id);
        Task<List<State>> GetAllStatesAsync();
    }
}