using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.Extensions.DependencyModel;
using server.DTOs;
using server.DTOs.Assignment;
using server.Interfaces;
using server.Mappers;
using server.Models;

namespace server.Controllers
{
    //treba da se dodaju dependent idjevi tamo gde treba
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentRepository _asign_repo;
        private readonly ITaskGroupRepository _group_repo;
        private readonly IPriorityRepository _prio_repo;
        private readonly IUserRepository _user_repo;
        private readonly IStateRepository _state_repo;
        private readonly IProjectRepository _project_repo;

        public AssignmentController(IAssignmentRepository asign_repo, IUserRepository user_repo
        , ITaskGroupRepository group_repo, IPriorityRepository prio_repo,IProjectRepository project_repo, IStateRepository state_repo)
        {
            _state_repo = state_repo;
            _asign_repo = asign_repo;
            _group_repo = group_repo;
            _project_repo = project_repo;
            _prio_repo = prio_repo;
            _user_repo = user_repo;
        }
        //Mozda ce morati da se ubaci i project ID u dto ?
        [HttpPost]
        [Route("create"), Authorize(Roles = "Project Manager")]
        public async Task<IActionResult> CreateAssignment([FromBody]CreateAssignmentDto dto)
        {
            //da li su datumi dobri
            if(dto.Start >= dto.End)
                return BadRequest("End date comes before start date");

            //da li je grupa validna
            var group = await _group_repo.GetTaskGroupByIdAsync(dto.TaskGroupId);
            if(group==null)
                return NotFound("That task group does not exist");

            //proverava da li su useri validni
            var users = new List<User>();
            var project  = await _project_repo.GetProjectByIdAsync(group.ProjectId);
            if(project==null)
               return NotFound("Project does not exist.ID:" + group.ProjectId);


            var team = await _user_repo.GetUserByProjectId(group.ProjectId);

            foreach (var userId in dto.Assignees)
            {
                var user = team.FirstOrDefault(u=>u.Id==userId);
                if(user==null)
                    return NotFound("User " + userId + " is not on project " + group.ProjectId);
                users.Add(user);
            }

            var dependencies = new List<Assignment>();
            foreach (var dependentId in dto.DependentOn)
            {
                var dependent = await _asign_repo.GetAssignmentByidAsync(dependentId);
                if(dependent==null)
                    return BadRequest("DependentOn Assignments for this Task are not good !!!");
                
                dependencies.Add(dependent);
            }
            
            //da li je prio validan
            var prio = await _prio_repo.GetPriority(dto.PriorityId);
            if(prio==null)
                return NotFound("Priority with "+ dto.PriorityId+ " does not exist");
            var state = await _state_repo.GetStateByIdAsync(dto.StateId);
            if(state==null)
                return NotFound("State with "+ dto.StateId+ " does not exist");
            var owner = await _user_repo.GetUserByIdAsync(dto.Owner);
            if(owner==null)
                return NotFound("Owner with "+ dto.Owner+ " does not exist");
            var ownerDto = owner.toAssignmentUserDto();
            var stateDto = state.toStateDto();
            var teamDto = users.Select(u=>u.toAssignmentUserDto()).ToList();
            var groupdto = group.toTaskGroupDto();
            var prioDto = prio.toPrioDto();
            var a = await _asign_repo.CreateAssignmentAsync(dto.fromCreateDtoToAssignment(users,dependencies,group));
            return Ok(a.toAssignmentDto(teamDto,prioDto,stateDto,ownerDto,groupdto));
        }
        [Authorize]
        [HttpGet("getDependentOnAssignments/{asing_id}")]
        public async Task<IActionResult> GetDependentAssignments([FromRoute] int asing_id)
        {
            var tasks = await _asign_repo.getDependentAssignments(asing_id);

            List<DependentAssignmentDto> res = new List<DependentAssignmentDto>();

            for (int i = 0; i < tasks.Count; i++)
            {
                // var prio = await _prio_repo.GetPriority(tasks[i].PriorityId);
                // if(prio==null)
                //     return NotFound("Priority with "+ tasks[i].PriorityId+ " does not exist");
                // var state = await _state_repo.GetStateByIdAsync(tasks[i].StateId);
                // if(state==null)
                //     return NotFound("State with "+ tasks[i].StateId+ " does not exist");
                // var owner = await _user_repo.GetUserByIdAsync(tasks[i].Owner);
                // if(owner==null)
                //     return NotFound("Owner with "+ tasks[i].Owner+ " does not exist");

                // var group = await _group_repo.GetTaskGroupByIdAsync(tasks[i].TaskGroupId);
                // if(group==null)
                //     return NotFound("That task group does not exist");
                // var team = await _user_repo.GetAssignmentUsersAsync(tasks[i].Id);

                // var ownerDto = owner.toAssignmentUserDto();
                // var stateDto = state.toStateDto();
                // var teamDto = team.Select(u=>u.toAssignmentUserDto()).ToList();
                // var groupdto = group.toTaskGroupDto();
                // var prioDto = prio.toPrioDto();
                // res.Add(tasks[i].toAssignmentDto(teamDto,prioDto,stateDto,ownerDto,groupdto));

                res.Add(tasks[i].toDependentAssignmentDto(tasks[i].TaskGroup.toTaskGroupDto()));
            }

            return Ok(res);

        }


        [Authorize]
        [HttpGet]
        [Route("getByGroup/{group_id}")]

        public async Task<IActionResult> GetAllAssignmentsByGroup([FromRoute] int group_id,[FromQuery] SortDto sort,[FromQuery] AssignmentFilterDto filter)
        {
            var tasks =  await _asign_repo.GetAllGroupAssignmentsAsync(group_id, filter,sort);
            List<AssignmentDto> res = new List<AssignmentDto>();

            for (int i = 0; i < tasks.Count; i++)
            {
                var ownerDto = tasks[i].User.toAssignmentUserDto();
                var stateDto = tasks[i].State.toStateDto();
                var teamDto = tasks[i].Users.Select(u=>u.toAssignmentUserDto()).ToList();
                var groupdto = tasks[i].TaskGroup.toTaskGroupDto();
                var prioDto = tasks[i].Priority.toPrioDto();
                res.Add(tasks[i].toAssignmentDto(teamDto,prioDto,stateDto,ownerDto,groupdto));
            }

            return Ok(res);
        }
        [Authorize]
        [HttpGet]
        [Route("getByUser/{user_id}")]
        public async Task<IActionResult> GetAllAssignmentsByUser([FromRoute] int user_id, [FromQuery] List<int> projects, [FromQuery] SortDto sort, [FromQuery] AssignmentFilterDto filter)
        {
            var user = await _user_repo.GetUserByIdAsync(user_id);
            if (user == null)
                return NotFound("User " + user_id + " does not exist");
            var tasks = await _asign_repo.GetAllUserAssignmentsAsync(user_id, filter, sort, projects);
            if (filter.PageNumber > 0 && filter.PageSize > 0)
            {
                int skip = (filter.PageNumber - 1) * filter.PageSize;
                tasks = tasks.Skip(skip).Take(filter.PageSize).ToList();
            }
            List<AssignmentDto> res = new List<AssignmentDto>();

            for (int i = 0; i < tasks.Count; i++)
            {
                var ownerDto = tasks[i].User.toAssignmentUserDto();
                var stateDto = tasks[i].State.toStateDto();
                var teamDto = tasks[i].Users.Select(u => u.toAssignmentUserDto()).ToList();
                var groupdto = tasks[i].TaskGroup.toTaskGroupDto();
                var prioDto = tasks[i].Priority.toPrioDto();
                res.Add(tasks[i].toAssignmentDto(teamDto, prioDto, stateDto, ownerDto, groupdto));
            }

            return Ok(res);
        }
        
        [Authorize]
        [HttpGet]
        [Route("getPaginatedAssignmentsByUser/{user_id}")]
        public async Task<IActionResult> GetAllPaginatedAssignmentsByUser([FromRoute] int user_id, [FromQuery] List<int> projects, [FromQuery] SortDto sort, [FromQuery] AssignmentFilterDto filter)
        {
            var user = await _user_repo.GetUserByIdAsync(user_id);
            if (user == null)
                return NotFound("User " + user_id + " does not exist");
            var tasks = await _asign_repo.GetAllUserAssignmentsAsync(user_id, filter, sort, projects);
            List<AssignmentDto> res = new List<AssignmentDto>();
            PaginationAssignmentsDto paginationAssignmentsDto = new PaginationAssignmentsDto();
            paginationAssignmentsDto.MaxAssignments = tasks.Count;
            if (filter.PageNumber > 0 && filter.PageSize > 0)
            {
                int skip = (filter.PageNumber - 1) * filter.PageSize;
                tasks = tasks.Skip(skip).Take(filter.PageSize).ToList();
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                var ownerDto = tasks[i].User.toAssignmentUserDto();
                var stateDto = tasks[i].State.toStateDto();
                var teamDto = tasks[i].Users.Select(u => u.toAssignmentUserDto()).ToList();
                var groupdto = tasks[i].TaskGroup.toTaskGroupDto();
                var prioDto = tasks[i].Priority.toPrioDto();
                res.Add(tasks[i].toAssignmentDto(teamDto, prioDto, stateDto, ownerDto, groupdto));
            }
            paginationAssignmentsDto.Assignments = res;
            return Ok(paginationAssignmentsDto);
        }

        [Authorize]
        [HttpGet]
        [Route("getById/{asign_id}")]
        public async Task<IActionResult> GetAssignmentById([FromRoute] int asign_id)
        {
            var task = await _asign_repo.GetAssignmentByidAsync(asign_id);
            if(task==null)
                return NotFound("Assignment does not exist.ID:" + asign_id);

                var ownerDto = task.User.toAssignmentUserDto();
                var stateDto = task.State.toStateDto();
                var teamDto = task.Users.Select(u=>u.toAssignmentUserDto()).ToList();
                var groupdto = task.TaskGroup.toTaskGroupDto();
                var prioDto = task.Priority.toPrioDto();
            
            var dep = task.Assignments.Select(u=>u.Id).ToList();
            
            var dtoAs = task.toAssignmentDto(teamDto,prioDto,stateDto,ownerDto,groupdto);
            dtoAs.DepndentOn = dep;

            return Ok(dtoAs);
        }
        
        [HttpPut("updateAssigmentGantt/{asign_id}"), Authorize(Roles = "Project Manager")]
        public async Task<IActionResult> updateAssignmentGantt([FromRoute] int asign_id,[FromBody] UpdateGanttAssignmentDto dto)
        {
            var task = await _asign_repo.GetAssignmentByidAsync(asign_id);
            if(task==null)
                return NotFound("Assignment not found");


            if(dto.EndTs !=0 && dto.StartTs!=0 && dto.StartTs > dto.EndTs)
            {
               return BadRequest("Start date comes after end date");
            }
            var baseDate = new DateTime (1970, 01, 01);
            var EndTs = task.End.Subtract (baseDate).TotalSeconds;
            var StartTs = task.Start.Subtract (baseDate).TotalSeconds;
            if(dto.StartTs!=0 && dto.EndTs==0 && dto.StartTs > EndTs)
                return BadRequest("Start date comes after end date");
            if(dto.EndTs!=0 && dto.StartTs==0 && dto.EndTs < StartTs)
                return BadRequest("Start date comes after end date");
            
            task = await _asign_repo.UpdateGanttAssignmentAsync(task,dto);

            var ownerDto = task.User.toAssignmentUserDto();
            var stateDto = task.State.toStateDto();
            var teamDto = task.Users.Select(u=>u.toAssignmentUserDto()).ToList();
            var groupdto = task.TaskGroup.toTaskGroupDto();
            var prioDto = task.Priority.toPrioDto();

            var dep = task.Assignments.Select(u=>u.Id).ToList();
            
            var dtoAs = task.toAssignmentDto(teamDto,prioDto,stateDto,ownerDto,groupdto);
            dtoAs.DepndentOn = dep;
            return Ok(dtoAs);

        }

        [HttpPut]
        [Route("update/{asign_id}"), Authorize(Roles = "Project Manager,Project Worker")]
        public async Task<IActionResult> UpdateAssignmentById([FromBody]UpdateAssignmentDto dto,[FromRoute] int asign_id)
        {
            //da li su datumi dobri
            if(dto.Start >= dto.End)
                return BadRequest("End date comes before start date");
            
            var prio = await _prio_repo.GetPriority(dto.PriorityId);
            if(prio==null)
                return NotFound("Priority with "+ dto.PriorityId+ " does not exist");
            var state = await _state_repo.GetStateByIdAsync(dto.StateId);
            if(state==null)
                return NotFound("State with "+ dto.StateId+ " does not exist");

            var group = await _group_repo.GetTaskGroupByIdAsync(dto.TaskGroupId);
            if(group==null)
                return NotFound("That task group does not exist");
            
            var users =  new List<User>();
            var team  = await _user_repo.GetUserByProjectId(group.ProjectId);
            foreach(var userId in dto.UserIds)
            {
                var user = team.Find(u=>u.Id==userId);
                if(user == null)
                    return NotFound("User not found on that project");
                users.Add(user);
            }

            List<Assignment> dependentOn = new List<Assignment>();
            foreach (var taskId in dto.DependentOn)
            {
                var task = await _asign_repo.GetAssignmentByidAsync(taskId);
                if(task == null)
                    return NotFound("Dependent task not found");
                if(task.TaskGroup.ProjectId!=group.ProjectId)
                    return NotFound("Dependent task"+task.Id+" is not on that project");
                dependentOn.Add(task);
            }
            var asignment = await _asign_repo.UpdateAssignmentAsync(dto,asign_id, users,dependentOn);

            if(asignment==null)
                return NotFound("Assignment does not exist.ID:" + asign_id);
            var ownerDto = asignment.User.toAssignmentUserDto();
            var stateDto = state.toStateDto();
            var teamDto = users.Select(u=>u.toAssignmentUserDto()).ToList();
            var groupdto = group.toTaskGroupDto();
            var prioDto = prio.toPrioDto();
            

            var dep = new List<int>();
            var dependent = await _asign_repo.getDependentAssignments(asign_id);
            
            dep = dependent.Select(d=>d.Id).ToList();
            var Rdto = asignment.toAssignmentDto(teamDto,prioDto,stateDto,ownerDto,groupdto);
            Rdto.DepndentOn = dep;
            return Ok(Rdto);
        }
        [Authorize]
        [HttpGet("getAssignmentsByProject/{project_id}")]
        public async Task<IActionResult> GetAllAssignmentsByProject([FromRoute] int project_id,[FromQuery] int user_id,[FromQuery] SortDto sort,[FromQuery] AssignmentFilterDto filter)
        {
            var groups = await _group_repo.GetAllProjectTaskGroupsAsync(project_id);
            var res = new List<AssignmentDto>();
            var user = new List<int>();
            if(user_id > 0)
            {
                user.Add(user_id);
            }
            foreach (var group in groups)
            {
                var tasks = await _asign_repo.GetAllGroupAssignmentsAsync(group.Id, filter,sort, user);
                for (int i = 0; i< tasks.Count;i++)
                {
                    var dep = new List<int>();
                    var dependent = tasks[i].DependentOnAssignments;
                    
                    dep = dependent.Select(d=>d.Id).ToList();
                    var ownerDto = tasks[i].User.toAssignmentUserDto();
                    var stateDto = tasks[i].State.toStateDto();
                    var teamDto = tasks[i].Users.Select(u=>u.toAssignmentUserDto()).ToList();
                    var groupdto = tasks[i].TaskGroup.toTaskGroupDto();
                    var prioDto = tasks[i].Priority.toPrioDto();
                    var dto = tasks[i].toAssignmentDto(teamDto,prioDto,stateDto,ownerDto,groupdto);
                    dto.DepndentOn = dep;
                    res.Add(dto);
                }
            }

            return Ok(res);
        }

        [HttpDelete("deleteAssignmentById/{asign_id}"), Authorize(Roles = "Project Manager")]
        public async Task<IActionResult> DeleteAssignment([FromRoute] int asign_id)
        {
            var asignment = await _asign_repo.DeleteAssignmentByIdAsync(asign_id);
            if(asignment==null)
                return NotFound("Assignment does not exist.ID:" + asign_id);
            
            return Ok("Assignment with id " + asign_id + " has been deleted successfully!"); 
        }

    }
}