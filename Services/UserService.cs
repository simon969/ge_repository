using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;
using ge_repository.interfaces;

namespace ge_repository.services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<ge_user> CreateUser(ge_user newUser)
        {
            await _unitOfWork.User.AddAsync(newUser);
            await _unitOfWork.CommitAsync();
            return newUser;
        }

        public async Task DeleteUser(ge_user user)
        {
            _unitOfWork.User.Remove(user);
            await _unitOfWork.CommitAsync();
        }
        public async Task<ge_user> GetUserById(string id)
        {
            return await _unitOfWork.User
                .GetByIdAsync(id);
        }
      

       public async Task<IEnumerable<ge_user>> GetAll()
        {
            return await _unitOfWork.User
                .GetAllAsync();
        }
        public async Task UpdateUser(ge_user userToBeUpdated, ge_user user)
        {
            userToBeUpdated.FirstName = user.FirstName;
            userToBeUpdated.LastName = user.LastName;

            await _unitOfWork.CommitAsync();
        }
    }
}