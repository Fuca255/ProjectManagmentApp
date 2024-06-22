using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs;
using server.DTOs.Assignment;
using server.Interfaces;
using server.Models;

namespace server.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly ProjectManagmentDbContext _context;

        public AssignmentRepository(ProjectManagmentDbContext context)
        {
            _context = context;
        }

        public async Task<Assignment> CreateAssignmentAsync(Assignment a)
        {
            a.LastTimeChanged = DateTime.Now;
            await _context.Assignments.AddAsync(a);
            await _context.SaveChangesAsync();
            
            return a;

        }
        public async Task<List<Assignment>> GetAllGroupAssignmentsAsync(int group_id, AssignmentFilterDto? filter = null,SortDto? sort = null, List<int>? user_ids = null)
        {
            var assingments_query = _context.Assignments.Where(a=>a.TaskGroupId==group_id)
            .Include(a=>a.Users).ThenInclude(u=>u.Role)
            .Include(a=>a.TaskGroup)
            .Include(a=>a.User).ThenInclude(u=>u.Role)
            .Include(a=>a.Priority)
            .Include(a=>a.Assignments)
            .Include(a=>a.DependentOnAssignments)
            .Include(a=>a.State).OrderByDescending(a=>a.LastTimeChanged)
            .AsQueryable();

            if (user_ids !=null && user_ids.Count > 0)
            {
                // System.Console.WriteLine("Ima nestoooooooooooooooooooooooooooooooooooooooooooooooooooooo");
                assingments_query = assingments_query.Where(a => a.Users.Any(b => user_ids.Contains(b.Id)));
            }

            return await FilterAssignments(assingments_query,filter,sort);
        }

        public async Task<List<Assignment>> GetAllUserAssignmentsAsync(int userId, AssignmentFilterDto? filter = null,SortDto? sort = null, List<int>? projects = null)
        {
            var pom = _context.Assignments.Include
            (a=>a.Users)
            .Include(a=>a.TaskGroup)
            .Include(a=>a.User)
            .Include(a=>a.Priority)
            .Include(a=>a.State).Where(a=>!a.IsClosed).OrderByDescending(a=>a.LastTimeChanged).AsQueryable();

            if (projects != null && projects.Count > 0)
            {
                pom = pom.Where(a => projects.Contains(a.TaskGroup.ProjectId));
            }
            var query = pom.Where(t=>t.Users.Any(u=>u.Id==userId));

            return await FilterAssignments(query,filter,sort);
        }

        public async Task<Assignment?> GetAssignmentByidAsync(int id)
        {
            return await _context.Assignments
            .Include(a=>a.Users)
            .Include(a=>a.TaskGroup)
            .Include(a=>a.User)
            .Include(a=>a.Priority)
            .Include(a=>a.State)
            .Include(a=>a.Assignments)
            .FirstOrDefaultAsync(a=>a.Id==id);
        }

        public async Task<Assignment?> UpdateAssignmentAsync(UpdateAssignmentDto a, int id, List<User> users, List<Assignment> dependentOn)
        {
            var assignment = await GetAssignmentByidAsync(id);
            if(assignment==null)
                return null;
            assignment.Assignments = dependentOn;
            assignment.Title = a.Title;
            assignment.Users = users;
            assignment.IsClosed = a.IsClosed;
            assignment.TaskGroupId = a.TaskGroupId;
            assignment.Start = a.Start;
            assignment.End = a.End;
            assignment.Type = a.Type;
            assignment.LastTimeChanged = DateTime.Now;
            assignment.PriorityId = a.PriorityId;
            assignment.StateId = a.StateId;
            assignment.Description = a.Description;
            assignment.Percentage = a.Percentage;

            await _context.SaveChangesAsync();

            return assignment;
        }

        public async Task<List<Assignment>> FilterAssignments(IQueryable<Assignment> assignments, AssignmentFilterDto? dto,SortDto? sort = null)
        {
            if(dto!=null)
            {
                if(dto.StateFilter.Count > 0)
                    assignments = assignments.Where(p=>dto.StateFilter.Contains(p.StateId));
                if(dto.PriorityFilter.Count > 0)
                    assignments = assignments.Where(p=>dto.PriorityFilter.Contains(p.PriorityId));
                
                if(dto.SearchTitle!=string.Empty)
                    assignments = assignments.Where(p=>p.Title.ToLower().Contains(dto.SearchTitle.ToLower()));
                //datumi
                if(dto.StartFrom!=null && dto.StartTo!=null)
                {
                    assignments = assignments.Where(p=>p.Start >=dto.StartFrom && p.Start <=dto.StartTo);
                }
                if(dto.EndFrom!=null && dto.EndTo!=null)
                {
                    assignments = assignments.Where(p => p.End >= dto.EndFrom && p.End <= dto.EndTo);
                }

                if(dto.PercentageFilterTo >=0 && dto.PercentageFilterFrom <= dto.PercentageFilterTo)
                {
                    assignments = assignments.Where(p=>p.Percentage <= dto.PercentageFilterTo && p.Percentage >=dto.PercentageFilterFrom);
                }
            }
            if(sort!=null)
            {
                if(sort.PropertyName.ToLower()=="title")
                {
                    if(sort.SortFlag==1)
                    {
                        assignments = assignments.OrderBy(a=>a.Title);
                    }
                    else
                        assignments = assignments.OrderByDescending(a=>a.Title);

                }
                else if(sort.PropertyName.ToLower()=="state")
                {
                    if(sort.SortFlag==1)
                    {
                        assignments = assignments.OrderBy(a=>a.State.Name);
                    }
                    else
                        assignments = assignments.OrderByDescending(a=>a.State.Name);
                }
                else if(sort.PropertyName.ToLower()=="priority")
                {
                    if(sort.SortFlag==1)
                    {
                        assignments = assignments.OrderBy(a=>a.Priority.Level);
                    }
                    else
                        assignments = assignments.OrderByDescending(a=>a.Priority.Level);
                }
                else if(sort.PropertyName.ToLower()=="start date")
                {
                    if(sort.SortFlag==1)
                    {
                        assignments = assignments.OrderBy(a=>a.Start);
                    }
                    else
                        assignments = assignments.OrderByDescending(a=>a.Start);
                }
                else if(sort.PropertyName.ToLower()=="end date")
                {
                    if(sort.SortFlag==1)
                    {
                        assignments = assignments.OrderBy(a=>a.End);
                    }
                    else
                        assignments = assignments.OrderByDescending(a=>a.End);
                }
                else if(sort.PropertyName.ToLower()=="progress")
                {
                    if(sort.SortFlag==1)
                    {
                        assignments = assignments.OrderBy(a=>a.Percentage);
                    }
                    else
                        assignments = assignments.OrderByDescending(a=>a.Percentage);
                }
                else if(sort.SortFlag==1)
                   assignments =assignments.OrderBy(p=>p.LastTimeChanged);
                else
                   assignments =assignments.OrderByDescending(p=>p.LastTimeChanged);
            }
            //stavlja closed na kraju
            var filtered = await assignments.ToListAsync();
            var completed = filtered.Where(a=>a.IsClosed).ToList();
            var notCompleted = filtered.Where(a=>!a.IsClosed).ToList();
            // System.Console.WriteLine("IDEMOOOOOOOOOOOOMOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
            // System.Console.WriteLine(completed.Count);
            // System.Console.WriteLine(notCompleted.Count);
            notCompleted.AddRange(completed);
            return notCompleted;
        }

        public async Task<Assignment?> DeleteAssignmentByIdAsync(int asign_id)
        {
            var asignment = await GetAssignmentByidAsync(asign_id);
            if(asignment==null)
                return null;
            _context.Assignments.Remove(asignment);
            await _context.SaveChangesAsync();

            return asignment;
            
        }

        public async Task<List<Assignment>> getDependentAssignments(int asign_id)
        {
            var asignment = await _context.Assignments.Include(a=>a.Assignments).ThenInclude(a=>a.TaskGroup).FirstOrDefaultAsync(a=>a.Id==asign_id);
            if(asignment==null)
            {
                return new List<Assignment>();
            }


            return asignment.Assignments.ToList();
            
        }

        public async Task<Assignment> UpdateGanttAssignmentAsync(Assignment assignment, UpdateGanttAssignmentDto dto)
        {
            if(dto.AddDependentOn.Count > 0)
            {
                foreach (var taskId in dto.AddDependentOn)
                {
                    var task = await GetAssignmentByidAsync(taskId);
                    if(task!=null && assignment.Assignments.FirstOrDefault(t=>t.Id==task.Id)==null)
                        assignment.Assignments.Add(task);
                }
            }
            if(dto.RemoveDependentOn.Count > 0)
            {
                foreach (var taskId in dto.RemoveDependentOn)
                {
                    var task = await GetAssignmentByidAsync(taskId);
                    if(task!=null)
                        assignment.Assignments.Remove(task);
                }
            }
            if(dto.EndTs!=0)
            {
                assignment.End = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(dto.EndTs).ToLocalTime();
            }
            if(dto.StartTs!=0)
            {
                assignment.Start = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(dto.StartTs).ToLocalTime();
            }
            if(dto.Type!="")
                assignment.Type = dto.Type;
            if(dto.Percentage >= 0)
                assignment.Percentage = dto.Percentage;
            if(dto.Description!="")
                assignment.Description = dto.Description;
            if(dto.PriorityId > 0)
                assignment.PriorityId = dto.PriorityId;
            if(dto.StateId > 0)
                assignment.StateId = dto.StateId;
            await _context.SaveChangesAsync();
            

            return assignment;
        }
    }
}