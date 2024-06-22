using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs.Assignment;
using server.Models;

namespace server.Repositories
{
    public class TaskGroupRepository : ITaskGroupRepository
    {
        private readonly ProjectManagmentDbContext _context;
        public TaskGroupRepository(ProjectManagmentDbContext context)
        {
            _context = context;
        }
        public async Task<TaskGroup> CreateAsync(TaskGroup group)
        {
            await _context.TaskGroups.AddAsync(group);
            await _context.SaveChangesAsync();
            
            return group;
        }

        public async Task<List<TaskGroup>> GetAllProjectTaskGroupsAsync(int project_id)
        {
            return await _context.TaskGroups.Where(tg=>tg.ProjectId==project_id).ToListAsync();
        }

        public async Task<TaskGroup?> GetTaskGroupByIdAsync(int id)
        {
            var t = await _context.TaskGroups.Include(tg=>tg.Assignments).Include(tg=>tg.InverseParentNavigation).FirstOrDefaultAsync(tg=> tg.Id==id);
            return t;
        }

        public async Task<TaskGroup> UpdateTaskGroupAsync(TaskGroup group)
        {
            await _context.SaveChangesAsync();
            return group;
        }




        public async Task<TaskGroup?> getInitialTaskGroupOfProject(int project_id)
        {
            return await _context.TaskGroups.Include(tg=>tg.InverseParentNavigation).FirstOrDefaultAsync(tg=>tg.ProjectId==project_id && tg.ParentTaskGroupId==null);
        }
        public async Task<List<TaskGroup>> getChildrenGroups(int task_group)
        {
            var group = await GetTaskGroupByIdAsync(task_group);
            if(group==null)
                return new List<TaskGroup>();
            
            var children = group.InverseParentNavigation.ToList();

            return children;
        }

        public async Task<TaskGroup?> deleteTaskGroupAsync(int id)
        {
            var group = await GetTaskGroupByIdAsync(id);
            if(group==null)
                return null;
            
            _context.TaskGroups.Remove(group);
            await _context.SaveChangesAsync();
            return group;
        }
    }
}