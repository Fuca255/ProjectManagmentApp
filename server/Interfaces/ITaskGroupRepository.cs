using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.DTOs.Assignment;
using server.Models;

namespace server.Interfaces
{
    public interface ITaskGroupRepository
    {
        Task<TaskGroup> CreateAsync(TaskGroup group);
        Task<List<TaskGroup>> GetAllProjectTaskGroupsAsync(int project_id);
        Task<TaskGroup?> GetTaskGroupByIdAsync(int id);
        Task<List<TaskGroup>> getChildrenGroups(int task_group);
        Task<TaskGroup?> getInitialTaskGroupOfProject(int project_id);
        Task<TaskGroup> UpdateTaskGroupAsync(TaskGroup group);
        Task<TaskGroup?> deleteTaskGroupAsync(int id);
    }
}