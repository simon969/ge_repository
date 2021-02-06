using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ge_repository.interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> FindByIdAsync(Guid id);
        Task<TEntity> FindByIdAsync(string id);
        Task<TEntity> FindByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindNoTrackingAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        bool ExistsLocal(TEntity entity);
        bool Exists(params object[] keys);
        
    }
}