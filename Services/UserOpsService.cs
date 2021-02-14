using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;
using ge_repository.interfaces;

namespace ge_repository.services
{
    public class UserOpsService : IUserOpsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserOpsService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<ge_user_ops> CreateUserOps(ge_user_ops newUserOps)
        {
            await _unitOfWork.UserOps.AddAsync(newUserOps);
            await _unitOfWork.CommitAsync();
            return newUserOps;
        }

        public async Task DeleteUserOps(ge_user_ops user_ops)
        {
            _unitOfWork.UserOps.Remove(user_ops);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<ge_user_ops>> GetAllWithProject()
        {
            return await _unitOfWork.UserOps
                .GetAllWithProjectAsync();
        }

        public async Task<ge_user_ops> GetUserOpsById(Guid id)
        {
            return await _unitOfWork.UserOps
                .FindByIdAsync(id);
        }
        public async Task<ge_user> GetUserByEmail(string id)
        {
            return await _unitOfWork.User
                .FindByIdAsync(id);
        }
        public async Task<IEnumerable<ge_user_ops>> GetAllWithGroup()
        {
            return await _unitOfWork.UserOps
                .GetAllWithGroupAsync();
        }
        public async Task<IEnumerable<ge_user_ops>> GetAllByProjectId(Guid projectId)
        {
            return await _unitOfWork.UserOps
                .GetAllByProjectIdAsync(projectId);
        }
        public async Task<IEnumerable<ge_user_ops>> GetAllByGroupId(Guid groupId)
        {
            return await _unitOfWork.UserOps
                .GetAllByGroupIdAsync(groupId);
        }

        public async Task UpdateUserOps(ge_user_ops dest, ge_user_ops src)
        {
            dest.projectId = src.projectId;
            
            await _unitOfWork.CommitAsync();
        }
       
        public async Task<Boolean> AreDataOperationsAllowed (string userId, ge_data data, string request_ops) {

            ge_user_ops group_user = await _unitOfWork.UserOps.GetByUserIdGroupId(userId,data.project.groupId);
            ge_user_ops project_user = await _unitOfWork.UserOps.GetByUserIdProjectId(userId,data.projectId);
            
            operation_request req =  new operation_request(data, group_user, project_user);
            
            return req.AreDataOperationsAllowed(request_ops);
        }
        public async Task<Boolean> AreProjectOperationsAllowed(string userId, ge_project project, string request_ops) {

            ge_user_ops group_user = await _unitOfWork.UserOps.GetByUserIdGroupId(userId,project.groupId);
            ge_user_ops project_user = await _unitOfWork.UserOps.GetByUserIdProjectId(userId,project.Id);
            
            operation_request req =  new operation_request(project, group_user, project_user);
            
            return req.AreProjectOperationsAllowed(request_ops);
        }
        public async Task<Boolean> AreGroupOperationsAllowed (string userId, ge_group group, string request_ops) {

            ge_user_ops group_user = await _unitOfWork.UserOps.GetByUserIdGroupId(userId,group.Id);
                       
            operation_request req =  new operation_request(group, group_user);
            
            return req.AreGroupOperationsAllowed(request_ops);
        }
         public async Task<string> GetAllowedOperations (string userId, ge_data data) {

            ge_user_ops group_user = await _unitOfWork.UserOps.GetByUserIdGroupId(userId,data.project.groupId);
            ge_user_ops project_user = await _unitOfWork.UserOps.GetByUserIdProjectId(userId,data.projectId);
            
            operation_request req =  new operation_request(data, group_user, project_user);
           
            return req._effectiveData_ops;

        }
        public async Task<string> GetAllowedOperations (string userId, ge_project project) {
            
            ge_user_ops group_user = await _unitOfWork.UserOps.GetByUserIdGroupId(userId, project.groupId);
            ge_user_ops project_user = await _unitOfWork.UserOps.GetByUserIdProjectId(userId,project.Id);

            operation_request req = new operation_request(project, group_user, project_user);

            return req._effectiveProject_ops;
        }

        public async Task<string> GetAllowedOperations (string userId, ge_group group) {
            
            ge_user_ops group_user = await _unitOfWork.UserOps.GetByUserIdGroupId(userId, group.Id);
            
            operation_request req = new operation_request(group, group_user);

            return req._effectiveGroup_ops;
        }
        public async Task<operation_request> GetOperationRequest (string userId, ge_data data) {
            
            ge_user_ops group_user = await _unitOfWork.UserOps.GetByUserIdGroupId(userId,data.project.groupId);
            ge_user_ops project_user = await _unitOfWork.UserOps.GetByUserIdProjectId(userId,data.projectId);
            
            operation_request req =  new operation_request(data, group_user, project_user);

            return req;

        }
        public async Task<Boolean> IsUserGroupAdmin(string userId, ge_group group) {
            
            ge_user_ops group_user = await _unitOfWork.UserOps.GetByUserIdGroupId(userId, group.Id);
            
            return (group_user.user_operations.Contains("Admin"));
        }
        public async Task<Boolean> IsUserProjectAdmin(string userId, ge_project project) {
            
            ge_user_ops group_user = await _unitOfWork.UserOps.GetByUserIdGroupId(userId, project.groupId);
            
            if (group_user.user_operations.Contains("Admin")) {
                return true;
            };
            
            ge_user_ops project_user = await _unitOfWork.UserOps.GetByUserIdProjectId(userId, project.Id);
            
            return (project_user.user_operations.Contains("Admin"));

        }
       
        public async Task<ge_user> GetUserByEmailAddress(string emailAddress) {

            return await _unitOfWork.User.GetUserByEmailAddress(emailAddress);
        
        }
    }
}