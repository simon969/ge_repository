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
        private string ParentWithMatchedChildItems(string parent, string child) {
            // get all child items that exist in the parent
           string[] child_split = child.Split(",");
           string resp = "";

           foreach (string c in child_split) {
               if (parent.Contains(c)) {
                   if (resp.Length>0) resp += ",";
                   resp += "c";
               }
           }

           return resp;
        }
        
        public async Task<string> GetAllowedOperations (string userId, ge_data data) {
            
            string UserGroupOps = "";
            string GroupOps = "";
            string GroupUserOps = "";
            string UserProjectOps = "";
            string ProjectOps = "";
            string ProjectUserOps = "";
            string GroupProjectUserOps = "";

            ge_user_ops group_ops = await _unitOfWork.UserOps.GetByUserIdGroupIdIncludeGroup(userId, data.project.group.Id);
          
            if (group_ops != null ) {
                GroupOps =  group_ops.group.operations;
                UserGroupOps = group_ops.operations;
                GroupUserOps = ParentWithMatchedChildItems(GroupOps, UserGroupOps);
            } else {
                GroupUserOps =  data.project.group.operations;
            }
            
            ge_user_ops project_ops = await _unitOfWork.UserOps.GetByUserIdProjectIdIncludeProject(userId, data.project.Id);
            
            if (project_ops != null ) { 
                ProjectOps = project_ops.project.operations;
                UserProjectOps = project_ops.operations;
                ProjectUserOps = ParentWithMatchedChildItems(ProjectOps, UserProjectOps);
            } else {
                ProjectUserOps =  data.project.operations;
            }

            GroupProjectUserOps = ParentWithMatchedChildItems(GroupUserOps,ProjectUserOps);

            return GroupProjectUserOps;

        }
        public async Task<ge_user> GetUserByEmailAddress(string emailAddress) {

            return await _unitOfWork.User.GetUserByEmailAddress(emailAddress);
        
        }
    }
}