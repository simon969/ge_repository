using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;

namespace ge_repository.interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ge_project>> GetAllWithGroup();
        Task<ge_project> GetProjectById(Guid id);
        Task<IEnumerable<ge_project>> GetProjectByGroupId(Guid Id);
        Task<ge_project> CreateProject(ge_project newProject);
        Task UpdateProject(ge_project source, ge_project destination);
        Task DeleteProject(ge_project project);
    }
}