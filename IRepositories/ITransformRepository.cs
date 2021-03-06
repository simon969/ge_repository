using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;

namespace ge_repository.interfaces
{
    public interface ITransformRepository : IRepository<ge_transform>
    {
        Task<IEnumerable<ge_transform>> GetAllTransformAsync();
        Task<IEnumerable<ge_transform>> GetAllWithProjectAsync();
        Task<ge_transform> GetWithProjectAsync(Guid id);
        Task<IEnumerable<ge_transform>> GetAllByProjectIdAsync(Guid Id);
        Task<IEnumerable<ge_transform>> GetAllByGroupIdAsync(Guid Id);
    }
}