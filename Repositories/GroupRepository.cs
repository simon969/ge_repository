using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ge_repository.Models;
using ge_repository.interfaces;

namespace ge_repository.repositories
{
    public class GroupRepository : Repository<ge_group>, IGroupRepository
    {
        public GroupRepository(ge_DbContext context) 
            : base(context)
        { }

        public async Task<IEnumerable<ge_group>> GetAllGroupAsync()
        {
            return await ge_DbContext.ge_group
                .ToListAsync();
        }
        public async Task<ge_group> GetWithGroupIdAsync(Guid Id) {

            return await ge_DbContext.ge_group
                 .SingleOrDefaultAsync(a => a.Id == Id);

        }
       
        private ge_DbContext ge_DbContext
        {
            get { return _context as ge_DbContext; }
        }
    }
}