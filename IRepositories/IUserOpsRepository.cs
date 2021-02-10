using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;

namespace ge_repository.interfaces
{
    public interface IUserOpsRepository : IRepository<ge_user_ops>
    {
        
        Task<IEnumerable<ge_user_ops>> GetAllUserOpsAsync();
        Task<IEnumerable<ge_user_ops>> GetAllWithProjectAsync();
        Task<IEnumerable<ge_user_ops>> GetAllWithGroupAsync();
        Task<ge_user_ops> GetWithProjectById(Guid Id);
        Task<ge_user_ops> GetWithGroupById(Guid Id);
        Task<IEnumerable<ge_user_ops>> GetAllByUserIdAsync(string Id);
        Task<IEnumerable<ge_user_ops>> GetAllByProjectIdAsync(Guid Id);
        Task<IEnumerable<ge_user_ops>> GetAllByGroupIdAsync(Guid Id);
        Task<ge_user_ops> GetByUserIdProjectIdIncludeProject(string UserId, Guid ProjectId);
        Task<ge_user_ops> GetByUserIdGroupIdIncludeGroup(string UserId, Guid GroupId);
        Boolean IsOperationAllowed(string operation, _ge_base ge_base); 
        Task<Boolean> DoesUserHaveOperation (string operation, ge_group group, ge_user user);
        Task<Boolean> DoesUserHaveOperation (string operation, ge_project project, ge_user user);
        Task<string> GetOperations(string userId, ge_data data);


    }
}