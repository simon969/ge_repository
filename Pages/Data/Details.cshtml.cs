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

using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Pages.Shared;
using ge_repository.Extensions;

namespace ge_repository.Pages.Data
{
    public class DetailsModel :_DataPageModel
    {
         public bool IsAGSML{get;set;}
         public DetailsModel(
           ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
            IOptions<ge_config> config,
            ILogger<_DataPageModel> logger
            )
            : base(context, authorizationService, userManager, config, logger)
        {
            IsAGSML=false;
        }
        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null) {
                return NotFound();
            }

            data = await _context.ge_data
                .Include(g => g.edited)
                .Include(g => g.created)
                .Include(g => g.project).FirstOrDefaultAsync(m => m.Id == id);

            if (data == null) {
                return NotFound();
            }

            var UserAuthorised = await IsUserReadAuthorised();
                      
             if (IsReadAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.DATA_READ_PROHIBITED);
            }

            if (!UserAuthorised.Succeeded) {
               return RedirectToPageMessage (msgCODE.DATA_READ_USER_PROHIBITED);
            }
            if (data.fileext == ".xml") {
            /*    if (!String.IsNullOrEmpty(data.AGSVersion())){
                    IsAGSML=true;
                } */ 
            IsAGSML = true;
            }
           
            return Page();
        }
    }
}
