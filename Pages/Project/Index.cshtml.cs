using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static ge_repository.Authorization.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using ge_repository.Extensions;
using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Pages.Shared;

namespace ge_repository.Pages.Project {
    public class IndexModel : _geFullPagedModel
    {
        public IndexModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager)
            : base(context, authorizationService, userManager, 20)
    {
    }
        public IList<ge_project> projects {get;set;}

      
    public async Task<IActionResult>  OnGetAsync(string pageFilter, string pageSort, int? pageIndex, int? pageSize, Guid? groupId, Guid? projectId, Constants.PublishStatus? pStatus) 
            {
            base.setPaging(pageFilter,pageSort,pageIndex,pageSize,groupId, projectId, pStatus);

            var UserId = GetUserIdAsync().Result;
            
            int pTotal;

            IQueryable<ge_project> lproject = null;

            if (pageFilter !=null) {
            lproject = _context.getuserprojects("Read", UserId, pageFilter);
            } else {
            lproject = _context.getuserprojects("Read", UserId);
            } 
           
            if (projectId != null) {
            lproject = lproject.Where(p=>p.Id == projectId);
            project =  _context.ge_project
                             .AsNoTracking()
                             .FirstOrDefault(p => p.Id == projectId);
            }
            
            if (groupId != null) {
            group =  _context.ge_group
                             .AsNoTracking()
                             .FirstOrDefault(g => g.Id == groupId);
            lproject = lproject.Where(p=>p.groupId == groupId);
            }
           
            projects = await lproject.Include (p=>p.group)
                    .Include (p=>p.data)
                    .Include (p=>p.transform)
                    .PagedResult(pageIndex,pageSize,m=>m.createdDT,false, out pTotal).ToListAsync();
            
            pageTotal = pTotal;

            if (projects == null) {
                return NotFound();
            }
            setViewData();
            return Page();
            }
       protected void setViewData() {
                    if (group != null) {
                    ViewData["GroupName"] = group.name;
                    }
       }             
    }
}
