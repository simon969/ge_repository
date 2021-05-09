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
                                   .Include(m => m.project)
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
            //var objectContext = (_context as IObjectContextAdapter).ObjectContext;
            // Sets the command timeout for all the commands
            //objectContext.CommandTimeout = 120;
            
            _context.Database.SetCommandTimeout(120);
           // _context.Database.ExecuteSqlCommand("delete dbo.ge_data where Id = {0}", new object[] { data.Id });
            
            // Create empty ge_data_file with ge_data Id to avoid having to download all data
            ge_data_file empty_file =  new ge_data_file () {Id=data.Id};
            _context.ge_data_file.Attach (empty_file);
            // Now mark the ge_data entity and ge_data_file for deletion
            _context.ge_data_file.Remove (empty_file);
            _context.ge_data.Remove(data);
            //_context.Entry<ge_data_file>(empty_file).State = EntityState.Deleted; 
            //_context.Entry<ge_data>(data).State = EntityState.Deleted; 
            // Apply changes
            await _context.SaveChangesAsync();


            return RedirectToPage("./Index",new {projectId=projectId});
        }
    }
}