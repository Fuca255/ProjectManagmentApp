using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Authorization;
using server.DTOs;
using server.DTOs.Projects;
using server.DTOs.Users;
using server.Interfaces;
using server.Mappers;
using server.Models;

namespace server.Controllers
{
    //TO-DO treba se odraditi validacija podataka id-jevi svih vezanih modela
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _repos;
        private readonly IUserRepository _user_repo;
        private readonly IPriorityRepository _prio_repo;
        private readonly ITaskGroupRepository _group_repo;
        private readonly IStateRepository _state_repo;
        private readonly IRoleRepository _role_repo;
        public ProjectController(IRoleRepository role_repo,IPriorityRepository prio_repo,IProjectRepository repos,IUserRepository user_repo,ITaskGroupRepository group_repo, IStateRepository stateRepository)
        {
            _repos = repos;
            _prio_repo = prio_repo;
            _role_repo = role_repo;
            _user_repo = user_repo;
            _group_repo = group_repo;
            _state_repo = stateRepository;

        }

        //treba filter
        [Authorize]
        [HttpGet("getProjects")]
        public async Task<IActionResult> GetAll([FromQuery]ProjectFilterDto dto,[FromQuery]SortDto sort)
        {
            var projects = await _repos.GetAllProjectsAsync(dto,sort);
            var dtos = new List<ProjectDto>();
            foreach (var project in projects)
            {
                var users =  project.ProjectUsers.Select(u=>u.User).ToList();
                var project_roles = project.ProjectUsers.Select(u=>u.Role).ToList();

                var userDtos = new List<ProjectUserDto>();
                for(int i =0;i<users.Count;i++)
                {
                    userDtos.Add(users[i].toProjectUserDto(project_roles[i].toRoleDto()));
                }
                int asignCount = _repos.getAssignemntForProjectCount(project);
                dtos.Add(project.ToProjectDto(userDtos,project.State.toStateDto(),project.Priority.toPrioDto(),asignCount));
            }
            return Ok(dtos);
        }
        
        [Authorize]
        [HttpGet("userProjects/{userId}")]
        public async Task<IActionResult> GetAllUserProjects([FromRoute]int userId,[FromQuery] ProjectFilterDto filter,[FromQuery] SortDto sort)
        {
            var projects = await _repos.GetAllUserProjectsAsync(userId,filter,sort);
            if(filter.PageNumber!= 0 && filter.PageSize!= 0)
            {
                projects = projects.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
            }
            var dtos = new List<ProjectDto>();
            foreach (var project in projects)
            {
                var users =  project.ProjectUsers.Select(u=>u.User).ToList();
                var project_roles = project.ProjectUsers.Select(u=>u.Role).ToList();

                var userDtos = new List<ProjectUserDto>();
                for(int i =0;i<users.Count;i++)
                {
                    userDtos.Add(users[i].toProjectUserDto(project_roles[i].toRoleDto()));
                }
                int asignCount = _repos.getAssignemntForProjectCount(project);
                dtos.Add(project.ToProjectDto(userDtos,project.State.toStateDto(),project.Priority.toPrioDto(),asignCount));
            }
            return Ok(dtos);
        }
        [Authorize]
        [HttpGet("paginatedUserProjects/{userId}")]
        public async Task<IActionResult> GetAllPaginatedUserProjects([FromRoute]int userId,[FromQuery] ProjectFilterDto filter,[FromQuery] SortDto sort)
        {
            var projects = await _repos.GetAllUserProjectsAsync(userId,filter,sort);
            PaginationProjectsDto pag = new PaginationProjectsDto();
            pag.MaxProjects = projects.Count;
            var dtos = new List<ProjectDto>();
            if(filter.PageNumber!= 0 && filter.PageSize!= 0)
            {
                projects = projects.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();
            }
            foreach (var project in projects)
            {
                var users =  project.ProjectUsers.Select(u=>u.User).ToList();
                var project_roles = project.ProjectUsers.Select(u=>u.Role).ToList();

                var userDtos = new List<ProjectUserDto>();
                for(int i =0;i<users.Count;i++)
                {
                    userDtos.Add(users[i].toProjectUserDto(project_roles[i].toRoleDto()));
                }
                int asignCount = _repos.getAssignemntForProjectCount(project);
                dtos.Add(project.ToProjectDto(userDtos,project.State.toStateDto(),project.Priority.toPrioDto(),asignCount));
            }
            pag.Projects = dtos;
            return Ok(pag);
        }
        [Authorize]
        [HttpGet("getProjectLimits/{user_id}")]
        public async Task<IActionResult> getProjectLimits(int user_id)
        {
            var projects = await _repos.GetAllUserProjectsAsync(user_id);
            if(projects.Count != 0)
            {
                var limitDto =  new {
                    budgetMax = projects.Max(p=>p.Budget),
                    budgetMin = projects.Min(p=>p.Budget),
                    spentMax = projects.Max(p=>p.Spent),
                    spentMin = projects.Min(p=>p.Spent)
                };
                return Ok(limitDto);
            }
            return Ok(new {
                budgetMax = 0,
                budgetMin = 0,
                spentMax = 0,
                spentMin = 0
            });
        }
        [Authorize]
        [HttpGet("getFilterProjectsByUser/{user_id}")]
        public async Task<IActionResult> getFilterProjects([FromRoute] int user_id)
        {
                var projects = await _repos.getFilterProjectsAsync(user_id);
            var dtos = new List<ProjectDto>();
            foreach (var project in projects)
            {
                var users =  project.ProjectUsers.Select(u=>u.User).ToList();
                var project_roles = project.ProjectUsers.Select(u=>u.Role).ToList();

                var userDtos = new List<ProjectUserDto>();
                for(int i =0;i<users.Count;i++)
                {
                    userDtos.Add(users[i].toProjectUserDto(project_roles[i].toRoleDto()));
                }
                int asignCount = _repos.getAssignemntForProjectCount(project);
                dtos.Add(project.ToProjectDto(userDtos,project.State.toStateDto(),project.Priority.toPrioDto(),asignCount));
            }
            return Ok(dtos);
        }
        [Authorize]
        [HttpGet("getProject/{id}")]
        public async Task<IActionResult> getById([FromRoute] int id)
        {
            var project = await _repos.GetProjectByIdAsync(id);
            if(project==null)
                return NotFound("Project with Id:"+id + " was not found !!!");
            
            var dto = new ProjectDto();

            var users =  project.ProjectUsers.Select(u=>u.User).ToList();
            var project_roles = project.ProjectUsers.Select(u=>u.Role).ToList();

            var userDtos = new List<ProjectUserDto>();
            for(int i =0;i<users.Count;i++)
            {
                userDtos.Add(users[i].toProjectUserDto(project_roles[i].toRoleDto()));
            }
            int asignCount = _repos.getAssignemntForProjectCount(project);
            dto = project.ToProjectDto(userDtos,project.State.toStateDto(),project.Priority.toPrioDto(),asignCount);
    
            return Ok(dto);
        }

        [HttpPost("create"), Authorize(Roles = "Project Manager")]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto projectDto)
        {
            
            List<User> teamMembers = new List<User>();
            List<Role> projectRoles = new List<Role>();
            //da li su datumi dobri
            System.Console.WriteLine(projectDto.UserProjectRoleIds.Count);
            System.Console.WriteLine(projectDto.UserIds.Count);
            if(projectDto.Start >= projectDto.End)
                return BadRequest("End date comes before start date.");
            
            if(projectDto.UserProjectRoleIds.Count!=projectDto.UserIds.Count)
                return BadRequest("Roles and user do not match.");
            //da li je prioritet validan
            var prio = await _prio_repo.GetPriority(projectDto.PriorityId);
            if(prio==null)
                return NotFound("Priority not found.ID:" + projectDto.PriorityId);
            
            //da li su korinsici validni
            for (int i = 0;i<projectDto.UserIds.Count;i++)
            {
                var userId = projectDto.UserIds[i];
                var roleId = projectDto.UserProjectRoleIds[i];

                var role =  await _role_repo.GetRoleByIdAsync(roleId);
                var user = await _user_repo.GetUserByIdAsync(userId);
                if(role==null)
                    return NotFound("Role not found.ID:" + roleId);
                if(user==null)
                    return NotFound("User not found.ID:" + userId);

                if(user.Role.Authority > role.Authority)
                    return BadRequest("Project role higher then the role on a platform.");
                teamMembers.Add(user);
                projectRoles.Add(role);
            }
            var projectModel = projectDto.toProjectFromCreateDto();
            var response = await _repos.CreateProjectAsync(projectModel,teamMembers,projectRoles);
            
            
            //kreiranje initial grupe - zove se isto kao i projekat
            var group = new TaskGroup{ Title = projectDto.Title, ParentTaskGroupId = null, ProjectId = response.Id};


            var users =  projectModel.ProjectUsers.Select(u=>u.User).ToList();
            var project_roles = projectModel.ProjectUsers.Select(u=>u.Role).ToList();

            var userDtos = new List<ProjectUserDto>();
            for(int i =0;i<users.Count;i++)
            {
                userDtos.Add(users[i].toProjectUserDto(project_roles[i].toRoleDto()));
            }
            var state = await _state_repo.GetStateByIdAsync(response.StateId);
            if(state==null)
                return NotFound("error");
            
            await _group_repo.CreateAsync(group);

            return Ok(response.ToProjectDto(userDtos,state.toStateDto(),prio.toPrioDto(),0));
        }

        [HttpPut]
        [Route("update/{project_id}"), Authorize(Roles = "Project Manager")]
        public async Task<IActionResult> Update([FromBody] UpdateProjectDto projectDto, [FromRoute] int project_id)
        {
            //da li su datumi dobri
            if(projectDto.Start >= projectDto.End)
                return BadRequest("End date comes before start date");
            
            if(projectDto.ProjectRoles.Count!=projectDto.Members.Count)
                return BadRequest("Roles and user do not match.");

            var users =  new List<User>();
            var roles = new List<Role>();
            for (int i =0;i< projectDto.Members.Count;i++)
            {
                var userId = projectDto.Members[i];
                var roleId = projectDto.ProjectRoles[i];
                var role = await _role_repo.GetRoleByIdAsync(roleId);
                var user = await _user_repo.GetUserByIdAsync(userId);
                if(user==null)
                    return NotFound("user not found");
                if(role == null)
                    return NotFound("Role not found");
                
                if(role.Authority < user.Role.Authority)
                    return BadRequest("Project role higher then the role on a platform.");
                
                roles.Add(role);
                users.Add(user);
            }
            var prio = await _prio_repo.GetPriority(projectDto.PriorityId);
            if(prio==null)
                return NotFound("Priority with "+ projectDto.PriorityId+ " does not exist");
            var state = await _state_repo.GetStateByIdAsync(projectDto.StateId);
            if(state==null)
                return NotFound("State with "+ projectDto.StateId+ " does not exist");

            var project = await _repos.UpdateProjectAsync(projectDto, project_id, users,roles);

            if(project==null)
                return NotFound("Project with Id:"+project_id + " was not found !!!");
            var dto = new ProjectDto();
            
            users =  project.ProjectUsers.Select(u=>u.User).ToList();
            var project_roles = project.ProjectUsers.Select(u=>u.Role).ToList();

            var userDtos = new List<ProjectUserDto>();
            for(int i =0;i<users.Count;i++)
            {
                userDtos.Add(users[i].toProjectUserDto(project_roles[i].toRoleDto()));
            }
            int asignCount = _repos.getAssignemntForProjectCount(project);
            dto = project.ToProjectDto(userDtos, state.toStateDto(),prio.toPrioDto(),asignCount);
    
            return Ok(dto);
        }

        //Salje se id project_managera za kojeg hocemo plus se salje period string vrednost (week,month)
        [Authorize]
        [HttpGet("userProjectStates/{userId}/{period}")]
        public async Task<IActionResult> GetAllUserStatesProjects([FromRoute]int userId,[FromRoute] string period)
        {
            var res = await _repos.GetAllUserProjectStates(userId,period);
            return Ok(res);
        }


        [HttpDelete("deleteProjectById/{project_id}"), Authorize(Roles = "Project Manager")]
        public async Task<IActionResult> DeleteProject([FromRoute] int project_id)
        {
            var project = await _repos.DeleteProjectByIdAsync(project_id);
            if(project==null)
                return BadRequest("project does not exist");
    
            return Ok("Project deleted successfully!");
        }
    }
}