using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ge_repository.Models;

namespace ge_repository.interfaces
{
    public interface IADORepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(Guid id);
        Task<TEntity> GetByIdAsync(string id);
        Task<TEntity> GetByIdAsync(int id);
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> FindAsync(string where);
        Task<TEntity> FindSingleAsync(string where);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task<int> UpdateRangeAsync(IEnumerable<TEntity> entities, string exist_where);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}