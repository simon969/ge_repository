using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;
using ge_repository.OtherDatabase;

namespace ge_repository.interfaces
{
    public interface IGintRepository<T> : IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetWhereAsync(string where);
       
        Task<int> CommitAsync();
    }
}