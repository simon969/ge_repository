using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ge_repository.interfaces;


namespace ge_repository.repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;
        
        public Repository(DbContext context)
        {
            this.Context = context;
        }
        public async Task AddAsync(TEntity entity)
        {
           
            await Context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return  await Context.Set<TEntity>().ToListAsync();
        }

        public Task<TEntity> FindByIdAsync(Guid id)
        {
            return Context.Set<TEntity>().FindAsync(id);
        }
        public Task<TEntity> FindNoTrackingAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>()
                        .AsNoTracking()
                        .Where(predicate)
                        .FirstOrDefaultAsync();
        }
        public Task<TEntity> FindByIdAsync(string id)
        {
            return Context.Set<TEntity>().FindAsync(id);
        }
        public Task<TEntity> FindByIdAsync(int id)
        {
            return Context.Set<TEntity>().FindAsync(id);
        }
        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }

        public bool ExistsLocal(TEntity entity)
        {
        return Context.Set<TEntity>().Local.Any(e => e == entity);
        }
        public bool Exists(params object[] keys)
        {
        return (Context.Set<TEntity>().Find(keys) != null);
        }
    }

}