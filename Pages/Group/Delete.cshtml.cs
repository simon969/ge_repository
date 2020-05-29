using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Pages.Shared;
using ge_repository.Extensions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ge_repository.Pages.Group
{
     public class DeleteModel : _geGroupPageModel
    {
        public DeleteModel(
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
                .Include(g => g.projects)
                .Include(g => g.manager).FirstOrDefaultAsync(m => m.Id == id);

            if (group == null)
            {
                return NotFound();
            }
            
            if (group.projects.Any()) {
             ModelState.AddModelError("DeleteWarning","This group contains projects, these will also deleted if this group is deleted");
            }
            

            var UserAuthorised =  await IsUserAdminAuthorised();

            if (IsDeleteAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.GROUP_DELETE_PROHIBITED);
            }
            if (!UserAuthorised.Succeeded) {
               return RedirectToPageMessage (msgCODE.GROUP_DELETE_USER_PROHIBITED);
            }

            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            group = await _context.ge_group.FindAsync(id);

            if (group == null) {
               return NotFound();
            }
            
            var UserAuthorised =  await IsUserAdminAuthorised();
            
            if (IsDeleteAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.GROUP_DELETE_PROHIBITED);
            }
            if (!UserAuthorised.Succeeded) {
               return RedirectToPageMessage (msgCODE.GROUP_DELETE_USER_PROHIBITED);
            }
                      
            _context.ge_group.Remove(group);
            await _context.SaveChangesAsync();
            

            return RedirectToPage("./Index");
        }
        
       
    }
}
