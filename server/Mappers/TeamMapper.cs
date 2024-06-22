using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.DTOs.Team;
using server.DTOs.Users;
using server.Models;

namespace server.Mappers
{
    public static class TeamMapper
    {
        public static TeamDto ToTeamDto(this Team t, List<UserRoleDto> members)
        {
            return new TeamDto{
                Id = t.Id,
                Name = t.Name,
                Type = t.Type,
                Members = members,
                CreatedBy = t.CreatedBy
            };
        }

        public static Team fromCreateDtoToTeam(this CreateTeamDto dto)
        {
            return new Team{
                Name = dto.Name,
                Type = dto.Type,
                CreatedBy = dto.CreatedBy
            };
        }
    }
}