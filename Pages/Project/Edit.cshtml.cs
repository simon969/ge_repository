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
using Microsoft.Extensions.Options;
using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Pages.Shared;
using ge_repository.Extensions;

using static ge_repository.Authorization.Constants;

namespace ge_repository.Pages.Project
{
    public class EditModel :  _ProjectPageModel
    {
        public EditModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
            IOptions<ge_config> ge_config) : base(context, authorizationService, userManager, ge_config)
        {
        }

        public async Task<IActionResult> OnGetAsync(Guid? Id)
        {

            project = await _context.ge_project
                        .Include(p =>p.group)
                        .FirstOrDefaultAsync(m => m.Id == Id);

            if (project == null) {
                return NotFound();
            }
            
            int UpdateStatus = IsUpdateAllowed();
            var UserAuthorized =  await IsUserUpdateAuthorised();
            var UserAdmin = await IsUserAdminAuthorised();   
            var UserApprover = await IsUserApproveAuthorised();
            
            if (UpdateStatus!=geOPSResp.Allowed) {
                if (UpdateStatus!=geOPSResp.ProjectApproved && !UserApprover.Succeeded) {
                    return RedirectToPageMessage (msgCODE.PROJECT_UPDATE_PROHIBITED);
                }
            }
            if (!UserAuthorized.Succeeded && !UserAdmin.Succeeded) {
               return RedirectToPageMessage (msgCODE.PROJECT_UPDATE_USER_PROHIBITED);
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
            var group = await _context.ge_group
                            .FirstOrDefaultAsync(m => m.Id == project.groupId);
           
            if (group==null) {
                setViewData();
                return Page();
            }
            
            project.group = group;

            int UpdateStatus = IsUpdateAllowed();
            var UserAuthorized =  await IsUserUpdateAuthorised();
            var UserAdmin = await IsUserAdminAuthorised();   
            var UserApprover = await IsUserApproveAuthorised();
            
            if (UpdateStatus!=geOPSResp.Allowed) {
                if (UpdateStatus!=geOPSResp.ProjectApproved && !UserApprover.Succeeded) {
                    return RedirectToPageMessage (msgCODE.PROJECT_UPDATE_PROHIBITED);
                }
            }
            if (!UserAuthorized.Succeeded && !UserAdmin.Succeeded) {
               return RedirectToPageMessage (msgCODE.PROJECT_UPDATE_USER_PROHIBITED);
            }

            if (!UpdateProjectionLoc(project)) {
                setViewData();
                return Page();
            }
                       
            project.editedId = GetUserIdAsync().Result ;
            project.editedDT = DateTime.UtcNow;

            _context.Attach(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
}
