using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;

namespace ge_repository.interfaces
{
    public interface ITransformService
    {
        Task<IEnumerable<ge_transform>> GetAllWithProject();
        Task<IEnumerable<ge_transform>> GetAllByProjectId(Guid id);
        Task<IEnumerable<ge_transform>> GetAllByGroupId(Guid Id);
        Task<ge_transform> GetTransformById(Guid Id);
        Task<ge_transform> CreateTransform(ge_transform newTransform);
        Task UpdateTransform(ge_transform source, ge_transform destination);
        Task DeleteTransform(ge_transform transform);
    }
}