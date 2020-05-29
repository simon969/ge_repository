using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using ge_repository.Extensions;
using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Pages.Shared;

namespace ge_repository.Pages.User
{
    public class IndexModel : _geFullPagedModel
    {
        public IndexModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager)
            : base(context, authorizationService, userManager, 20)
    {
    }

        public IList<ge_user> users { get;set; }

       public async Task  OnGetAsync(string pageFilter, string pageSort, int? pageIndex, int? pageSize, Guid? pageOfficeId, Guid? projectId, Constants.PublishStatus? pStatus) 
            {
            base.setPaging(pageFilter,pageSort,pageIndex,pageSize,pageOfficeId,projectId, pStatus);
            int pTotal;
            users = await _context.UserSearch(pageFilter)
                            .PagedResult(pageIndex,pageSize,u=>u.LastLoggedIn,true, out pTotal).ToListAsync();
        }
    }
}
