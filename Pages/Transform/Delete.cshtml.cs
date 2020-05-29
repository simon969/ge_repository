using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Pages.Shared;
using ge_repository.Extensions;

namespace ge_repository.Pages.Transform
{
    public class DeleteModel : _TransformPageModel
    {
        public DeleteModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
            IOptions<ge_config> config
            )
            : base(context, authorizationService, userManager, config) {
            }
     
        public async Task<IActionResult> OnGetAsync(Guid? id) {
            
            if (id == null)
            {
                return NotFound();
            }

            transform = await _context.ge_transform
                .Include(g => g.created)
                .Include(g => g.data)
                .Include(g => g.edited)
                .Include(g => g.project)
                .Include(g => g.style).FirstOrDefaultAsync(m => m.Id == id);

            if (transform == null)
            {
                return NotFound();
            }
            
            var UserAuthorised = await IsUserDeleteAuthorised();

            if (IsDeleteAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.TRANSFORM_DELETE_PROHIBITED);
            }
            if (!UserAuthorised.Succeeded) {
               return RedirectToPageMessage (msgCODE.TRANSFORM_DELETE_USER_PROHIBITED);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            transform = await _context.ge_transform
                                    .Include(t => t.project)
                                    .FirstOrDefaultAsync(m => m.Id == id);; 
            
            if (transform == null) {
                return NotFound();
            }
            
            var UserAuthorised = await IsUserDeleteAuthorised();

            if (IsDeleteAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.TRANSFORM_DELETE_PROHIBITED);
            }
            if (!UserAuthorised.Succeeded) {
               return RedirectToPageMessage (msgCODE.TRANSFORM_DELETE_USER_PROHIBITED);
            }

            _context.ge_transform.Remove(transform);
            await _context.SaveChangesAsync();
            
            return RedirectToPage("./Index",new {projectId=transform.projectId});
        }
    }
}
