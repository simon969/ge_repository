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
    public class DetailsModel : _TransformPageModel
    {
        public DetailsModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
            IOptions<ge_config> config
            )
            : base(context, authorizationService, userManager, config)
        {
           
        }

     

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null) {
                return NotFound();
            }

            transform = await _context.ge_transform
                .Include(g => g.created)
                .Include(g => g.data)
                .Include(g => g.edited)
                .Include(g => g.project)
                .Include(g => g.style).FirstOrDefaultAsync(m => m.Id == id);
           
            if (transform == null) {
                return NotFound();
            }
            
            var UserAuthorised = await IsUserReadAuthorised();

            if (IsReadAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.TRANSFORM_READ_PROHIBITED);
            }
            if (!UserAuthorised.Succeeded) {
               return RedirectToPageMessage (msgCODE.TRANSFORM_READ_USER_PROHIBITED);
            }


            return Page();
        }
    }
}
