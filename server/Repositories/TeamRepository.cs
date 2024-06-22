using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs.Team;
using server.Interfaces;
using server.Mappers;
using server.Models;

namespace server.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ProjectManagmentDbContext _context;
        public TeamRepository(ProjectManagmentDbContext context)
        {
            _context = context;
        }
        public async Task<Team> CreateTeamAsync(Team team)
        {
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();

            return team;

        }

        public async Task<Team?> DeleteTeamAsync(int team_id)
        {
            var team = await GetTeamByIdAsync(team_id);
            if(team==null)
                return null;
            
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return team;
        }

        public async Task<List<Team>> GetAllCreatedTeamsByUserAsync(int user_id, string searchText)
        {
            var teams = _context.Teams.Include(t=>t.Owner).Include(t=> t.Users).ThenInclude(u=>u.Role).Where(t=>t.Owner.Id==user_id).AsQueryable();
            if(searchText!="")
            {
                teams = teams.Where(t=>t.Name.ToLower().Contains(searchText.ToLower()));
            }

            return await teams.ToListAsync();
        }

        public async Task<List<Team>> GetAllTeamsAsync()
        {
            return await _context.Teams.Include(t=> t.Users).ThenInclude(u=>u.Role).ToListAsync();
        }

        public async Task<List<Team>> GetAllUserTeams(int userid)
        {
            var teams = await _context.Teams.Include(t=>t.Users).ThenInclude(t=>t.Role).ToListAsync();

            var user_teams = new List<Team>();
            foreach (var team in teams)
            {
                if(team.Users.Any(u=>u.Id==userid))
                    user_teams.Add(team);
            }
            return user_teams;

        }

        public async Task<Team?> GetTeamByIdAsync(int id)
        {
            
            var team = await _context.Teams.Include(t=>t.Users).ThenInclude(u=>u.Role).FirstOrDefaultAsync(t=> t.Id==id);
            return team;
        }

        // public async Task<List<User>> GetTeamUsersByIdAsync(int teamId)
        // {
        //     var team = await GetTeamByIdAsync(teamId);
        //     if(team==null)
        //         return new List<User>();
        //     return team.Users.ToList();
        // }

        public async Task<Team?> UpdateTeamAsync(CreateTeamDto dto, int team_id, List<User> members)
        {
            var team = await GetTeamByIdAsync(team_id);

            if(team==null)
                return null;
            
            if(dto.Name !="")
                team.Name = dto.Name;
            if(members.Count > 0)
                team.Users = members;
            if(dto.Type!="")
                team.Type = dto.Type;
            
            await _context.SaveChangesAsync();

            return team;
        }
    }
}