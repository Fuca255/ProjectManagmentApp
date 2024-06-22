using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.DTOs.Team;
using server.Models;

namespace server.Interfaces
{
    public interface ITeamRepository
    {
        Task<List<Team>> GetAllTeamsAsync();
        Task<List<Team>> GetAllUserTeams(int userid);
        Task<Team?> GetTeamByIdAsync(int id);
        Task<Team> CreateTeamAsync(Team team);
        Task<List<Team>> GetAllCreatedTeamsByUserAsync(int user_id, string searchText);
        Task<Team?> UpdateTeamAsync(CreateTeamDto dto, int team_id, List<User> memebers);
        Task<Team?> DeleteTeamAsync(int team_id);
    }
        // Task<List<int>> GetTeamUsersByIdAsync(int teamId);
}