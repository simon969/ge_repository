using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ge_repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using ge_repository.Pages.Shared;
using ge_repository.Extensions;
using ge_repository.Authorization;
namespace ge_repository.Pages.Group
{
    public class DetailsModel :  _geGroupPageModel
    {
        public DetailsModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager)
            : base(context, authorizationService, userManager)
        {
        }
        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            group = await _context.ge_group
                .Include(g => g.created)
                .Include(g => g.edited)
                .Include(g => g.manager).FirstOrDefaultAsync(m => m.Id == id);

            if (group == null)
            {
                return NotFound();
            }
            
            var UserAuthorised =  await IsUserReadAuthorised();    
           
            if (IsReadAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.GROUP_READ_PROHIBITED);
            }
            if (!UserAuthorised.Succeeded) {
               return RedirectToPageMessage (msgCODE.GROUP_READ_USER_PROHIBITED);
            }
           
            return Page();
        }
       
    }
}
