using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;

namespace ge_repository.interfaces
{
    public interface IProjectRepository : IRepository<ge_project>
    {
        Task<IEnumerable<ge_project>> GetAllWithGroupAsync();
        Task<ge_project> GetWithGroupAsync(Guid id);
        Task<IEnumerable<ge_project>> GetAllWithGroupIdAsync(Guid Id);
    }
}