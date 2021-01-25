using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;

namespace ge_repository.interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<ge_user>> GetAll();
        Task<ge_user> GetUserById(string Id);
        Task<ge_user> CreateUser(ge_user newUser);
        Task UpdateUser(ge_user source, ge_user destination);
        Task DeleteUser(ge_user user);
    }
}