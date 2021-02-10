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
        protected readonly DbContext _context;
        
        public Repository(DbContext context)
        {
            this._context = context;
        }
        public async Task AddAsync(TEntity entity)
        {
           
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return  await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> FindByIdAsync(Guid id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        public Task<TEntity> FindNoTrackingAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>()
                        .AsNoTracking()
                        .Where(predicate)
                        .FirstOrDefaultAsync();
        }
        public async Task<TEntity> FindByIdAsync(string id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        public async Task<TEntity> FindByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }

        public bool ExistsLocal(TEntity entity)
        {
        return _context.Set<TEntity>().Local.Any(e => e == entity);
        }
        public bool Exists(params object[] keys)
        {
            return (_context.Set<TEntity>().FindAsync(keys) != null);
        }
    }

}