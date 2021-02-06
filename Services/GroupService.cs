using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;
using ge_repository.interfaces;

namespace ge_repository.services
{
    public class GroupService : IGroupService
    { 
        private readonly IUnitOfWork _unitOfWork;
        public GroupService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ge_group>> GetAllGroups() {
            return await _unitOfWork.Group
                .GetAllGroupAsync();
        }
        public async Task<ge_group> GetGroupById(Guid id) {
            return await _unitOfWork.Group
                .FindByIdAsync(id);
        }
        public async Task<ge_group> CreateGroup(ge_group newGroup) {
            await _unitOfWork.Group.AddAsync(newGroup);
            await _unitOfWork.CommitAsync();
            return newGroup;
        }

         public async Task UpdateGroup(ge_group from, ge_group to) {
            to.name = from.name;
            to.managerId = from.managerId;
            await _unitOfWork.CommitAsync();
        }
         public async Task DeleteGroup(ge_group group) {
            _unitOfWork.Group.Remove(group);
            await _unitOfWork.CommitAsync();

        }
   
    }
}
