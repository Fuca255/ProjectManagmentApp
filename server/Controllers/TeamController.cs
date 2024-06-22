using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.DTOs.Team;
using server.Interfaces;
using server.Mappers;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamRepository _team_repo;
        private readonly IUserRepository _user_repo;
        public TeamController(ITeamRepository team_repo, IUserRepository user_repo)
        {
            _user_repo = user_repo;
            _team_repo = team_repo;
        }

        [Authorize]
        [HttpGet("getTeams")]
        public async Task<IActionResult> GetAllTeams()
        {
            var teams = await _team_repo.GetAllTeamsAsync();

            var teamDtos = new List<TeamDto>();
            foreach (var team in teams)
            {
                var members = team.Users.Select(u=>u.toUserRoleDto(u.Role.toRoleDto())).ToList();
                teamDtos.Add(team.ToTeamDto(members));
            }


            return Ok(teamDtos);
        }
        [Authorize]
        [HttpGet("getAllCreatedTeamsByUser/{user_id}")]
        public async Task<IActionResult> GetAllCreatedTeamsByUser([FromRoute] int user_id,[FromQuery] string searchText = "")
        {
            var teams = await _team_repo.GetAllCreatedTeamsByUserAsync(user_id, searchText);

            var teamDtos = new List<TeamDto>();
            foreach (var team in teams)
            {
                var members = team.Users.Select(u=>u.toUserRoleDto(u.Role.toRoleDto())).ToList();
                teamDtos.Add(team.ToTeamDto(members));
            }

            return Ok(teamDtos);
        }
        [Authorize]
        [HttpGet]
        [Route("getTeam/{teamId}")]

        public async Task<IActionResult> GetTeam([FromRoute] int teamId)
        {
            var team = await _team_repo.GetTeamByIdAsync(teamId);
            if(team==null)
                return NotFound("Team not found!");

            var members = team.Users.Select(u=>u.toUserRoleDto(u.Role.toRoleDto())).ToList();

            return Ok(team.ToTeamDto(members));
        }
        [Authorize]
        [HttpGet]
        [Route("getUserTeams/{userId}")]
        public async Task<IActionResult> getUserTeams([FromRoute] int userId)
        {
            var teams = await _team_repo.GetAllUserTeams(userId);
            
            var timovi = new List<TeamDto>();
            foreach (var team in teams)
            {
                var members = team.Users.Select(u=>u.toUserRoleDto(u.Role.toRoleDto())).ToList();
                timovi.Add(team.ToTeamDto(members));
            }

            return Ok(timovi);
        }

        [HttpPost("createTeam"), Authorize(Roles = "Project Manager")]
        public async Task<IActionResult> CreateTeam([FromBody]CreateTeamDto dto)
        {
            if(dto.CreatedBy <= 0)
                return BadRequest("that user cannot create team");
            if(dto.Members.Count() <= 0)
                return BadRequest("memeber list is empty");
            var team = dto.fromCreateDtoToTeam();
            var users =  new List<User>();
            foreach (var userId in dto.Members)
            {
                var user = await _user_repo.GetUserByIdAsync(userId);
                if(user==null)
                    return NotFound("User not found.ID:" + userId);
                users.Add(user);
            }
            team.Users = users;
            team = await _team_repo.CreateTeamAsync(team);

            var members = users.Select(u=>u.toUserRoleDto(u.Role.toRoleDto())).ToList();
            
            return Ok(team.ToTeamDto(members));
            
        }

        [HttpPut("update/{team_id}"), Authorize(Roles = "Project Manager")]
        public async Task<IActionResult> UpdateTeam([FromBody] CreateTeamDto dto, [FromRoute] int team_id)
        {
            var users =  new List<User>();
            foreach (var userId in dto.Members)
            {
                var user = await _user_repo.GetUserByIdAsync(userId);
                if(user==null)
                    return NotFound("User not found.ID:" + userId);
                users.Add(user);
            }

            var response = await _team_repo.UpdateTeamAsync(dto, team_id,users);
            if(response==null)
                return NotFound("team update failed");

            var user_dtos = users.Select(u=>u.toUserRoleDto(u.Role.toRoleDto())).ToList();
            return Ok(response.ToTeamDto(user_dtos));
        }

        [HttpDelete("delete/{team_id}"), Authorize(Roles = "Project Manager")]
        public async Task<IActionResult> DeleteTeam([FromRoute] int team_id)
        {
            var team = await _team_repo.DeleteTeamAsync(team_id);
            if(team==null)
                return NotFound("team not found");

            return NoContent();
        }
        

    }
}