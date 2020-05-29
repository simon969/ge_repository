using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ge_repository.Pages.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Extensions;

namespace ge_repository.Pages.Project
{
    public class DeleteModel : _ProjectPageModel
    {
        public DeleteModel(
        ge_DbContext context,
        IAuthorizationService authorizationService,
        UserManager<ge_user> userManager,
        IOptions<ge_config> ge_config) : base(context, authorizationService, userManager, ge_config)
    {
    }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            project = await _context.ge_project
                    .Include(p => p.manager)
                    .Include(p => p.created)
                    .Include(p => p.data)
                    .Include(p => p.group)
                    .Include(p => p.edited).FirstOrDefaultAsync(m => m.Id == id);
                    

            if (project == null)
            {
                return NotFound();
            }
            
            if (project.data.Any()) {
              ModelState.AddModelError("DeleteWarning","This project contains data, this will also be deleted if this project is deleted");
            
            }
            var UserAuthorize =  await IsUserDeleteAuthorised();      
            
            if (IsDeleteAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.DATA_DELETE_PROHIBITED);
            }
            if (!UserAuthorize.Succeeded) {
               return RedirectToPageMessage (msgCODE.DATA_DELETE_USER_PROHIBITED);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            project = await _context.ge_project
                                .Include(p => p.group)
                                .FirstOrDefaultAsync(m => m.Id == id);

            if (project == null) {
                return NotFound();
            }
            
            var UserAuthorize =  await IsUserDeleteAuthorised();      
            
            if (IsDeleteAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.DATA_DELETE_PROHIBITED);
            }
            if (!UserAuthorize.Succeeded) {
               return RedirectToPageMessage (msgCODE.DATA_DELETE_USER_PROHIBITED);
            }
            
            _context.ge_project.Remove(project);
            await _context.SaveChangesAsync();
            
            return RedirectToPage("./Index");
        }
    }
}
