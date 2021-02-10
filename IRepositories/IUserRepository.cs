using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;

namespace ge_repository.interfaces
{
    public interface IUserRepository : IRepository<ge_user>
    {
        Task<IEnumerable<ge_user>> GetAllUserAsync();
        Task<ge_user> GetById(string Id);
        Task<ge_user> GetUserByEmailAddress(string emailAddress);
    }
}