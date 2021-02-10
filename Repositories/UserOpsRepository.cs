using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ge_repository.Models;
using ge_repository.interfaces;

namespace ge_repository.repositories

{
    public class UserOpsRepository : Repository<ge_user_ops>, IUserOpsRepository
    {
        public UserOpsRepository(ge_DbContext context) 
            : base(context)
        { }
        
        public async Task<IEnumerable<ge_user_ops>> GetAllUserOpsAsync()
        {
            return await ge_DbContext.ge_user_ops
                   .ToListAsync();
        }
        
        public async Task<IEnumerable<ge_user_ops>> GetAllWithProjectAsync()
        {
            return await ge_DbContext.ge_user_ops
                .Include(a => a.project)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<ge_user_ops>> GetAllWithGroupAsync()
        {
            return await ge_DbContext.ge_user_ops
                .Include(a => a.group)
                .ToListAsync();
        }

        public async Task<IEnumerable<ge_user_ops>> GetAllByUserIdAsync(string Id)
        {
            return await ge_DbContext.ge_user_ops
                 .Where (a => a.userId == Id)
                 .ToListAsync();   
        }

        public async Task<ge_user_ops> GetWithProjectById(Guid id)
        {
            return await ge_DbContext.ge_user_ops
                .Include(a => a.project)
                .SingleOrDefaultAsync(a => a.Id == id);
        }
         public async Task<ge_user_ops> GetWithGroupById(Guid id)
        {
            return await ge_DbContext.ge_user_ops
                .Include(a => a.group)
                .SingleOrDefaultAsync(a => a.Id == id);
        }
        public async Task<IEnumerable<ge_user_ops>> GetAllByProjectIdAsync(Guid Id) 
        {
            return await ge_DbContext.ge_user_ops
                .Where(a => a.projectId == Id)
                .ToListAsync();

        }
        public async Task<IEnumerable<ge_user_ops>> GetAllByGroupIdAsync(Guid Id) 
        {
            return await ge_DbContext.ge_user_ops
                .Where(a => a.project.groupId == Id)
                .ToListAsync();

        }
        public async Task<ge_user_ops> GetByUserIdProjectIdIncludeProject(string id, Guid projectId)
        {
            return await ge_DbContext.ge_user_ops
                .Include(a => a.project)
                .SingleOrDefaultAsync(a => a.userId == id && a.projectId == projectId);
        }
          public async Task<ge_user_ops> GetByUserIdGroupIdIncludeGroup(string id, Guid groupId)
        {
            return await ge_DbContext.ge_user_ops
                .Include(a => a.group)
                .SingleOrDefaultAsync(a => a.userId == id && a.groupId == groupId);
        }
        private ge_DbContext ge_DbContext
        {
            get { return _context as ge_DbContext; }
        }
        public Boolean IsOperationAllowed(string operation, _ge_base ge_base) {
            return ge_base.operations.Contains(operation);
        } 

        public async Task<Boolean> DoesUserHaveOperation (string operation, ge_group group, ge_user user) 
        {
            var user_ops = await GetByUserIdGroupIdIncludeGroup (user.Id,group.Id);
            
            if (user_ops==null) {return false;}

            return user_ops.operations.Contains(operation);
        }
        public async Task<Boolean> DoesUserHaveOperation (string operation, ge_project project, ge_user user) 
        {
            var user_ops = await GetByUserIdProjectIdIncludeProject (user.Id,project.Id);
            
            if (user_ops==null) {return false;}

            return user_ops.operations.Contains(operation);
        }
        public async Task<string> GetOperations(string Userd, ge_data data) {
            return null;
        }
      
    }
}