using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ge_repository.Pages.Shared;
using ge_repository.Models;
using ge_repository.Extensions;
using ge_repository.Authorization;

namespace ge_repository.Pages.Group
{
    public class IndexModel : _geFullPagedModel
    {
        public IndexModel(

            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager)
            : base(context, authorizationService, userManager, 10)
        {
           
        }

        public IList<ge_group> groups { get;set; }

       public async Task<IActionResult>  OnGetAsync(string pageFilter, string pageSort, int? pageIndex, int? pageSize, Guid? groupId, Guid? projectId, Constants.PublishStatus? pStatus) 
            {
            base.setPaging(pageFilter, pageSort, pageIndex, pageSize, groupId, projectId, pStatus);

            
            var UserId = GetUserIdAsync().Result;
            
            int pTotal;
            
            var lgroup = _context.GroupSearchByUserOperation (UserId,"Read",pageFilter);
                                                                      
           
            if (groupId != null) {
            lgroup = lgroup.Where(g=>g.Id == groupId);
            }
            
            groups = await lgroup.Include (g=>g.projects)
                                .PagedResult(pageIndex,pageSize,o=>o.createdDT,true, out pTotal).ToListAsync();
            
            pageTotal= pTotal;
                     
            if (groups == null) {
                return NotFound();
            }

            return Page();
            }
        public bool IsUserAnyAdmin() {
            return _context.IsUserAnyGroupAdmin(GetUserIdAsync().Result);

        }           
    }
}

