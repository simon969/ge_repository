using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;

namespace ge_repository.interfaces
{
    public interface IGroupService
    {
        Task<IEnumerable<ge_group>> GetAllGroups();
        Task<ge_group> GetGroupById(Guid id);
        Task<ge_group> CreateGroup(ge_group newGroup);
        Task UpdateGroup(ge_group groupToBeUpdated, ge_group group);
        Task DeleteGroup(ge_group group);
    }
}