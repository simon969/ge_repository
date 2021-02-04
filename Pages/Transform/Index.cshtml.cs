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

using ge_repository.Authorization;
using ge_repository.Models;
using ge_repository.Pages.Shared;
using ge_repository.Extensions;

namespace ge_repository.Pages.Transform
{
    public class IndexModel :  _geFullPagedModel
    {
        public IndexModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager)
            : base(context, authorizationService, userManager, 20)
    {}

        public IList<ge_transform> transform { get;set; }

           
        public async Task<IActionResult>  OnGetAsync(string pageFilter, string pageSort, int? pageIndex, int? pageSize, 
                                                    Guid? groupId, Guid? projectId, Constants.PublishStatus? pStatus) 
            {
            base.setPaging(pageFilter, pageSort, pageIndex, pageSize, groupId, projectId, pStatus);

            var isAuthorized = User.IsInRole(Constants.ge_repositoryManagerRole) ||
                        User.IsInRole(Constants.ge_repositoryAdministratorRole);
            var UserId = GetUserIdAsync().Result;
            
            int pTotal;
            IQueryable<ge_transform> ltransform = null;

            if (pageFilter !=null) {
            ltransform = _context.getusertransform("Read", UserId, pageFilter);
            } else {
            ltransform = _context.getusertransform("Read", UserId);
            } 

            if (projectId != null) {
            ltransform = ltransform.Where(p=>p.projectId == projectId);
          
            }
            
            if (groupId != null) {
            ltransform = ltransform.Where(p=>p.project.groupId == groupId);
            }
            
            
            if (pStatus != null) {
         //   t = t.Where(d=>d.pstatus == pagePublishStatus);
            }

            transform = await ltransform.Include (t => t.project)
                                        .Include (t => t.data)
                                        .Include (t => t.style)
                                        .PagedResult(pageIndex,pageSize,m=>m.editedDT,false, out pTotal).ToListAsync();
            
            pageTotal = pTotal;

            if (transform == null) {
                return NotFound();
            }

            setViewData();
            return Page();
            
            }
            protected void setViewData(){
                    if (project != null) { 
                        ViewData["ProjectName"] = project.name;
                    }
            }

        }
    }
