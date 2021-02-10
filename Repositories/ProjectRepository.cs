using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ge_repository.Models;
using ge_repository.interfaces;

namespace ge_repository.repositories
{
    public class ProjectRepository : Repository<ge_project>, IProjectRepository
    {
        public ProjectRepository(ge_DbContext context) 
            : base(context)
        { }

        public async Task<IEnumerable<ge_project>> GetAllWithGroupAsync()
        {
            return await ge_DbContext.ge_project
                .Include(a => a.group)
                .ToListAsync();
        }
        public async Task<IEnumerable<ge_project>> GetAllWithGroupIdAsync(Guid Id) {

            return await ge_DbContext.ge_project
                .Include(a => a.group)
                .Where (a => a.groupId == Id)
                .ToListAsync();

        }
        public async Task<ge_project> GetWithGroupAsync(Guid Id)
        {
            return await ge_DbContext.ge_project
                .Include(a => a.group)
                .SingleOrDefaultAsync(a => a.Id == Id);
        }

        private ge_DbContext ge_DbContext
        {
            get { return _context as ge_DbContext; }
        }
    }
}