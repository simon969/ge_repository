using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Pages.Shared;
using ge_repository.Extensions;
using ge_repository.DAL;
using ge_repository.Controllers;
using ge_repository.AGS;

namespace ge_repository.Pages.Data
{
    public class CreateXMLModel : _DataPageModel
    {   
              
        
       public CreateXMLModel(

           ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
            IOptions<ge_config> config,
            ILogger<_DataPageModel> logger)
            : base(context, authorizationService, userManager, config, logger)
        {
           
        }


      public async Task<IActionResult> OnGetAsync(Guid Id)
        {
             if (Id == null) {
                return NotFound();
            }
        
            data = await _context.ge_data
                            .Include (d => d.project)
                            .Where (d =>d.Id == Id).FirstOrDefaultAsync();

            if (data == null) {
                return NotFound();
            }
            
            var UserAuthorised = await IsUserCreateAuthorised(); 
            
            if (IsCreateAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.DATA_CREATE_PROHIBITED);
            }
            if (!UserAuthorised.Succeeded) {
               return RedirectToPageMessage (msgCODE.DATA_CREATE_USER_PROHIBITED);
            }
            
            return Page();

        } 

    public async Task<IActionResult> OnPostAsync()
    {
        
            if (!ModelState.IsValid) {
               setViewData();
               return Page();
            }
            
            var project = await _context.ge_project
                                        .FirstOrDefaultAsync(p =>p.Id==data.projectId);
            
            if (project==null) {
                setViewData();
                return Page();
            }

            data.project = project;

            var UserAuthorised = await IsUserCreateAuthorised(); 
            
            if (IsCreateAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.DATA_CREATE_PROHIBITED);
            }
            if (!UserAuthorised.Succeeded) {
               return RedirectToPageMessage (msgCODE.DATA_CREATE_USER_PROHIBITED);
            }

           // data.pflag = pflagCODE.PROCESSING;
           // _context.Attach(data).State = EntityState.Modified;
           // await _context.SaveChangesAsync();
   
          //  var result = new ge_agsController(
          //                  _context,_authorizationService, 
          //                  _userManager,
          //                   null).CreateXML (data.Id);
          //  
            return RedirectToAction( "CreateXML", "ge_ags", data.Id);

            // return RedirectToPage("./Index",new {projectId=project.Id});
    } 
           
    }
}