using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;

namespace ge_repository.interfaces
{
    public interface IUserOpsService
    {
        Task<IEnumerable<ge_user_ops>> GetAllWithProject();
        Task<IEnumerable<ge_user_ops>> GetAllByProjectId(Guid id);
        Task<IEnumerable<ge_user_ops>> GetAllWithGroup();
        Task<IEnumerable<ge_user_ops>> GetAllByGroupId(Guid Id);
        Task<ge_user_ops> CreateUserOps(ge_user_ops user_ops);
        Task UpdateUserOps(ge_user_ops source, ge_user_ops destination);
        Task DeleteUserOps(ge_user_ops user_ops);
        Task<string> GetAllowedOperations (string userId, ge_data data);
        Task<ge_user> GetUserByEmailAddress(string emailAddress);
        
    }
}