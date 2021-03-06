using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;

namespace ge_repository.interfaces
{
    public interface IGroupRepository : IRepository<ge_group>
    {
        Task<IEnumerable<ge_group>> GetAllGroupAsync();
        Task<ge_group> GetWithGroupIdAsync(Guid id);
        
    }
}