using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Pages.Shared;
using ge_repository.Extensions;
namespace ge_repository.Pages.Group
{
    public class EditModel :  _geGroupPageModel
    {
        public EditModel(
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
                .Include(g => g.manager)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            var UserAuthorised =  await IsUserUpdateAuthorised();
            var UserAdmin = await IsUserAdminAuthorised();

            if (IsUpdateAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.GROUP_UPDATE_PROHIBITED);
            }
            if (!UserAuthorised.Succeeded && !UserAdmin.Succeeded) {
               return RedirectToPageMessage (msgCODE.GROUP_UPDATE_USER_PROHIBITED);
            }

            setViewData();
          
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                setViewData();
                return Page();
            }
            
            var UserAuthorised =  await  IsUserUpdateAuthorised();
            var UserAdmin = await IsUserAdminAuthorised();

            if (IsUpdateAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.GROUP_UPDATE_PROHIBITED);
            }
            if (!UserAuthorised.Succeeded && !UserAdmin.Succeeded) {
               return RedirectToPageMessage (msgCODE.GROUP_UPDATE_USER_PROHIBITED);
            }

            if (!UpdateProjectionLoc(group)) {
                return Page();
            }

            if (!UpdateLastEdited(group)) {
                return Page();
            }

            _context.Attach(group).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!groupExists(group.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

    }
}
