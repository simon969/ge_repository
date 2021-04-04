using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using ge_repository.Models;
using ge_repository.Pages.Shared;
using ge_repository.Authorization;
using ge_repository.Extensions;

namespace ge_repository.Pages.UserOperations
{
    public class IndexModel :  _geFullPagedModel
    {
        public IndexModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager)
            : base(context, authorizationService, userManager, 20)
    {}
        public IList<ge_user_ops> user_ops { get;set; }
        public ge_user user {get;set;}
 
        public async Task<IActionResult>  OnGetAsync(string pageFilter, string pageSort, int? pageIndex, int? pageSize, Guid? groupId, Guid? projectId, Constants.PublishStatus? publishStatus) 
            {
                base.setPaging(pageFilter, pageSort, pageIndex, pageSize, groupId, projectId, publishStatus);
            
                user = await GetUserAsync();
                
                IQueryable<ge_user_ops> var ;

                if (IsUserGroupAdmin() || IsUserProjectAdmin()) {
                var = _context.UserOperationsSearch (null, pageFilter, "", groupId, projectId);
                } else { 
                var = _context.UserOperationsSearch (user.Id, pageFilter, "", groupId, projectId);
                }
                
                int pTotal;

                user_ops= await var.PagedResult(this.pageIndex,this.pageSize, o=>o.createdDT,true, out pTotal).ToListAsync(); 
                
                pageTotal = pTotal;
            
            return Page();

            }
            public bool IsUserGroupAdmin(){
                if (group!=null) {
                return _context.DoesUserHaveOperation(Constants.AdminOperationName, group, GetUserIdAsync().Result);
                }
                return false;
            }
            public bool IsUserProjectAdmin(){
                if (project!=null) {
                return _context.DoesUserHaveOperation(Constants.AdminOperationName, project, GetUserIdAsync().Result);
                }
                return false;
            }
           
        }
    }

