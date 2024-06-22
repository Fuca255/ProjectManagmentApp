using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.DTOs;
using server.DTOs.Projects;
using server.Models;

namespace server.Interfaces
{
    public interface IProjectRepository
    {
        //dashboard podaci
        Task<List<ProjectStatesDto>> GetAllUserProjectStates(int userId,string period);
        //uzimanje svih projekata
        Task<List<Project>> GetAllProjectsAsync(ProjectFilterDto? filter=null,SortDto? sort = null);
        //filtriranje projekata
        Task<List<Project>> GetAllFilteredProjectsAsync(IQueryable<Project> projects, ProjectFilterDto? dto,SortDto? sort = null);
        //uzimanje projekta po idju
        Task<Project?> GetProjectByIdAsync(int id);
        //kreiranje
        Task<Project> CreateProjectAsync(Project projectModel,List<User> teamMembers, List<Role> projectRoles);
        //update
        Task<Project?> UpdateProjectAsync(UpdateProjectDto projectDto, int project_id, List<User> project_users, List<Role> project_roles);
        // koirniskove projekte
        Task<List<Project>> GetAllUserProjectsAsync(int id,ProjectFilterDto? filter=null,SortDto? sort = null);
        //brisanje
        Task<Project?> DeleteProjectByIdAsync(int project_id);
        Task<List<Project>> getFilterProjectsAsync(int user_id);
        int getAssignemntForProjectCount(Project project);
    }
}