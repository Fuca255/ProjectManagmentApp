using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.DTOs.Priority;
using server.DTOs.Projects;
using server.DTOs.Role;
using server.DTOs.State;
using server.DTOs.Users;
using server.Models;

namespace server.Mappers
{
    public static class ProjectMapper
    {
        public static ProjectDto ToProjectDto(this Project p, List<ProjectUserDto> users, StateDto state, PriorityDto priority, int assignmentCount)
        {
            return new ProjectDto{
                Id = p.Id,
                Spent=p.Spent,
                State=state,
                Start=p.Start,
                Percentage = p.Percentage,
                End = p.End,
                Title = p.Title,
                Budget = p.Budget,
                Type = p.Type,
                OwnerId = p.OwnerId,
                Priority = priority,
                Description = p.Description,
                Users = users,
                AssignmentCount = assignmentCount

            };
        }

        public static Project toProjectFromCreateDto(this CreateProjectDto dto)
        {
            return new Project{
                Title = dto.Title,
                Type = dto.Type,
                Start = dto.Start, 
                Spent = 0,
                Percentage = 0,
                StateId = 1,
                Description = dto.Description,
                OwnerId = dto.OwnerId,
                PriorityId = dto.PriorityId,
                End = dto.End,
                Budget = dto.Budget,
            };
        }
        public static PriorityDto toPrioDto(this Priority prio)
        {
            return new PriorityDto{
                Id = prio.Id,
                Description = prio.Description,
                Level = prio.Level
            };
        }

        public static RoleDto toRoleDto(this Role role)
        {
            // if (role == null)
            // {
            //     return null;
            // }

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Authority = role.Authority
            };
        }

        public static StateDto toStateDto(this State state)
        {
            return new StateDto{
                Id = state.Id,
                Name = state.Name
            };
        }
    }
}