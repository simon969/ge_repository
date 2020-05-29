using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Pages.Shared;
using ge_repository.Extensions;
using Microsoft.Extensions.Options;

using static ge_repository.Authorization.Constants;

namespace ge_repository.Pages.Project
{
    public class DetailsModel : _ProjectPageModel
    {
        public DetailsModel(
        ge_DbContext context,
        IAuthorizationService authorizationService,
        UserManager<ge_user> userManager,
        IOptions<ge_config> ge_config) : base(context, authorizationService, userManager, ge_config){}

        public async Task<IActionResult> OnGetAsync(Guid? Id) {
            project = await _context.ge_project
                                    .Include (p=>p.created)
                                    .Include (p=>p.edited)
                                    .Include (p=>p.manager)
                                    .Include (p=>p.group)
                                    .FirstOrDefaultAsync(m => m.Id == Id);

            if (project == null){
                return NotFound();
            }

            var UserAuthorize =  await IsUserReadAuthorised();      
            
            if (IsReadAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.DATA_READ_PROHIBITED);
            }
            if (!UserAuthorize.Succeeded) {
               return RedirectToPageMessage (msgCODE.DATA_READ_USER_PROHIBITED);
            }
            
            return Page();

        }

    public async Task<IActionResult> OnPostAsync(Guid? id, PublishStatus status)
    {
        var project = await _context.ge_project
                                .Include(p =>p.group)
                                .FirstOrDefaultAsync(m => m.Id == id);

            if (project == null){
                return NotFound();
            }

            var userId = GetUserIdAsync().Result;
            var UserAuthorized =  await IsUserReadAuthorised();   
            var UserApprover = await IsUserApproveAuthorised();

            if (IsUpdateAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.PROJECT_UPDATE_PROHIBITED);
            }
            if (!UserAuthorized.Succeeded || !UserApprover.Succeeded) {
               return RedirectToPageMessage (msgCODE.PROJECT_UPDATE_USER_PROHIBITED);
            }
            
            project.pstatus = status;
            _context.ge_project.Update(project);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
    }

}

}
