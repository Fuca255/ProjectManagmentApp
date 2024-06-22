using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.DTOs.Assignment;
using server.Mappers;
using server.Models;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskGroupController : ControllerBase
    {
        private readonly IProjectRepository _project_repo;
        private readonly ITaskGroupRepository _group_repo;
        private readonly IAssignmentRepository _asign_repo;
        public TaskGroupController(IAssignmentRepository asign_repo,ITaskGroupRepository group_repo, IProjectRepository project_repo)
        {
            _group_repo = group_repo;
            _asign_repo = asign_repo;
            _project_repo = project_repo;
        }


        [HttpPost("createTaskGroup"), Authorize(Roles = "Project Manager")]
        public async Task<IActionResult> Create([FromBody] CreateTaskGroupDto dto)
        {
            //provera da li projekat postoji
            var project = await _project_repo.GetProjectByIdAsync(dto.ProjectId);            
            if(project==null)
                return BadRequest("That project does not exist");

            //provera da li parent_task_group postoji i da li pripada datom projektu
            if(dto.ParentTaskGroupId!=null)
            {
                
                var tg = await _group_repo.GetTaskGroupByIdAsync((int)dto.ParentTaskGroupId);
                if(tg==null)
                    return BadRequest("That parent task group does not exist");
                if(tg.ProjectId!=project.Id)
                    return BadRequest("That parent task does not belong to the given project");
            }

            var group = await _group_repo.CreateAsync(dto.fromCreateTaskGroupDtoToTaskGroup());
            return Ok(group.toTaskGroupDto());
        }
        [Authorize]
        [HttpGet("getTaskGroupById/{task_group_id}")]
        public async Task<IActionResult> GetById([FromRoute] int task_group_id)
        {
            var t = await _group_repo.GetTaskGroupByIdAsync(task_group_id);
            if(t==null)
                return BadRequest("That group does not exist");
            
            return Ok(t.toTaskGroupDto());
        }
        [Authorize]
        [HttpGet("getTaskGroupsByProjectWithTasks/{project_id}")]
        public async Task<IActionResult> GetByProjectId([FromRoute] int project_id, [FromQuery] SortDto sort,[FromQuery] List<int> user_ids,[FromQuery] AssignmentFilterDto filter)
        {
            //provera da li projekat postoji
            var project = await _project_repo.GetProjectByIdAsync(project_id);            
            if(project==null)
                return BadRequest("That project does not exist");

            var initial = await _group_repo.getInitialTaskGroupOfProject(project_id);
            if(initial==null)
                return NotFound("initi group not found");
            var tree = await makeTree(initial,sort,user_ids,filter);

            return Ok(tree);
        }
        [Authorize]
        [HttpGet("getTaskGroupsByProjectId/{project_id}")]
        public async Task<IActionResult> GetAllTaskGroupsByProjectId(int project_id)
        {
            var groups = await _group_repo.GetAllProjectTaskGroupsAsync(project_id);
            var dtos = groups.Select(group => group.toTaskGroupDto());
            return Ok(dtos);
        }


        [NonAction]
        public async Task<TaskGroupTasksDto> makeTree(TaskGroup initial, SortDto sort, List<int> user_ids, AssignmentFilterDto filter)
        {   
            var tasks =  await _asign_repo.GetAllGroupAssignmentsAsync(initial.Id,filter,sort,user_ids);
            var task_group_children = await _group_repo.getChildrenGroups(initial.Id);
            List<AssignmentDto> asignments = new List<AssignmentDto>();
            List<Object> children = new List<Object>();
            foreach (var task in tasks)
            {
                var userDtos = task.Users.Select(x=>x.toAssignmentUserDto()).ToList();
                var dep = task.Assignments.Select(a=>a.Id).ToList();
                var dto = task.toAssignmentDto(userDtos,task.Priority.toPrioDto(),
                task.State.toStateDto(),task.User.toAssignmentUserDto(),initial.toTaskGroupDto());
                dto.DepndentOn = dep;
                asignments.Add(dto);
            }
            
            foreach (var child in task_group_children)
            {
                var dto = await makeTree(child,sort,user_ids,filter);
                children.Add(dto);
            }
            foreach (var asign in asignments)
            {
                children.Add(asign);
            }

            return initial.toTaskGroupTasksDto(children);
            
        }

        [HttpPut("updateTaskGroup"), Authorize(Roles = "Project Manager")]
        public async Task<IActionResult> Update([FromBody] TaskGroupDto dto)
        {

            var group = await _group_repo.GetTaskGroupByIdAsync(dto.Id);
            if(group==null)
                return BadRequest("Task group does not exist");
            group.Title = dto.Title;
            group.ProjectId = dto.ProjectId;
            group.ParentTaskGroupId = dto.ParentTaskGroupId;

            await _group_repo.UpdateTaskGroupAsync(group);
            
            return Ok(group.toTaskGroupDto());

        }
        [HttpDelete("delete/{group_id}"), Authorize(Roles = "Project Manager")]
        public async Task<IActionResult> Delete([FromRoute] int group_id)
        {
            var group = await _group_repo.deleteTaskGroupAsync(group_id);
            if(group==null)
                return NotFound("group not found");
            return Ok("group deleted successfully");
        }
    }
}