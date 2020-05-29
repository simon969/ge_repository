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
using Microsoft.Extensions.Logging;

using ge_repository.Authorization;
using ge_repository.Pages.Shared;
using ge_repository.Models;
using ge_repository.Extensions;

namespace ge_repository.Pages.Data
{
    public class DeleteModel : _DataPageModel
    {
        public DeleteModel(

          ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
            IOptions<ge_config> config,
            ILogger<_DataPageModel> logger
            )
            : base(context, authorizationService, userManager, config, logger)
        {
           
        }


        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            data = await _context.ge_data
                                .Include (d =>d.project)
                                .Include (d =>d.edited)
                                .Include (d =>d.created)
                                .Include (d =>d.project)
                                .SingleOrDefaultAsync(m => m.Id == id);

            if (data == null)
            {
                return NotFound();
            }

            var UserAuthorised = await IsUserDeleteAuthorised();

            if (IsDeleteAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.DATA_DELETE_PROHIBITED);
            }
            if (!UserAuthorised.Succeeded) {
               return RedirectToPageMessage (msgCODE.DATA_DELETE_USER_PROHIBITED);
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null) {
                return NotFound();
            }

            data = await _context.ge_data
                                    .Include (d =>d.project)
                                    .Include (d =>d.data)
                                    .SingleOrDefaultAsync(m => m.Id == id);

            if (data == null) {
                return NotFound();
            }
            
            var UserAuthorised = await IsUserDeleteAuthorised();

            if (IsDeleteAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.DATA_DELETE_PROHIBITED);
            }
            if (!UserAuthorised.Succeeded) {
               return RedirectToPageMessage (msgCODE.DATA_DELETE_USER_PROHIBITED);
            }
            // remember projectId before deleting data
            Guid projectId = data.projectId;

            _context.ge_data.Remove(data);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index",new {projectId=projectId});
        }
    }
}