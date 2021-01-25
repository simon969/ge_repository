using System;
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
                .GetByIdAsync(id);
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
    }
}