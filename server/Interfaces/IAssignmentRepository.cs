using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.DTOs;
using server.DTOs.Assignment;
using server.Models;

namespace server.Interfaces
{
    public interface IAssignmentRepository
    {
        // uzmi taskove od koga zavisi zadat task
        Task<List<Assignment>> getDependentAssignments(int asign_id);
        // Uzima task po id-ju
        Task<Assignment?> GetAssignmentByidAsync(int id);
        // uzima sve taskove za nekog usera
        Task<List<Assignment>> GetAllUserAssignmentsAsync(int userId, AssignmentFilterDto? filter = null,SortDto? sort = null, List<int>? project_id = null);
        // uzima sve taskove jedne grupe taskova
        Task<List<Assignment>> GetAllGroupAssignmentsAsync(int group_id, AssignmentFilterDto? filter = null,SortDto? sort = null, List<int>? user_ids = null);
        // update taska
        Task<Assignment> UpdateGanttAssignmentAsync(Assignment assignment,UpdateGanttAssignmentDto dto);
        Task<Assignment?> UpdateAssignmentAsync(UpdateAssignmentDto a,int id, List<User> users,List<Assignment> dependentOn);
        // filtiranje taskova
        Task<List<Assignment>> FilterAssignments(IQueryable<Assignment> assignments, AssignmentFilterDto? dto,SortDto? sort = null);
        //brisanje
        Task<Assignment?> DeleteAssignmentByIdAsync(int asign_id);
        //kreiranje
        Task<Assignment> CreateAssignmentAsync(Assignment a);

    }
}