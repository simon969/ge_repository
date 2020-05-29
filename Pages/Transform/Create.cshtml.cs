using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Pages.Shared;
using ge_repository.Extensions;

namespace ge_repository.Pages.Transform
{
    public class CreateModel : _TransformPageModel
    {
        
       public CreateModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
            IOptions<ge_config> config
            )
            : base(context, authorizationService, userManager, config)
        {
           
        }

        public async Task<IActionResult> OnGetAsync(Guid projectId) {

            if (projectId == null) {
                return NotFound();
            }
        
            var project = await _context.projectFull(projectId).FirstOrDefaultAsync();

            if (project == null) {
                return NotFound();
            }
            
            transform =  new ge_transform();
            transform.project = project;
            transform.projectId = projectId;
            transform.operations = project.data_operations;

            var UserAuthorised = await IsUserCreateAuthorised();
         
            if (IsCreateAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.TRANSFORM_CREATE_PROHIBITED);
            }
            if (!UserAuthorised.Succeeded) {
               return RedirectToPageMessage (msgCODE.TRANSFORM_CREATE_USER_PROHIBITED);
            }
 
            Guid[] libraryId = _config.templateProjectId_ToGuid();
            
            var xml_data = _context.ge_data
                                        .Where(d=>d.projectId==transform.projectId || libraryId.Contains(d.projectId))
                                        .Where(d=>d.fileext == AGS.FileExtension.XML);
            var image_data =   _context.ge_data
                                        .Where(d=>d.projectId==transform.projectId || libraryId.Contains(d.projectId))
                                        .Where(d=>d.filetype.Contains("image"));                          
            var script_data = _context.ge_data
                                        .Where(d=>d.projectId==transform.projectId || libraryId.Contains(d.projectId))
                                        .Where(d=>d.filetype.Contains("script"));    
            var css_data = _context.ge_data
                                        .Where(d=>d.projectId==transform.projectId || libraryId.Contains(d.projectId)) 
                                        .Where(d=>d.filetype.Contains("css"));                        
            var xlt_data =  _context.ge_data
                                        .Where(d=>d.projectId==transform.projectId || libraryId.Contains(d.projectId))
                                        .Where(d=>d.fileext == AGS.FileExtension.XSL);        
            var transforms = _context.ge_transform
                                        .Include (t=>t.style)
                                        .Where(t=>t.projectId==transform.projectId);

            if (!xml_data.Any() || !xlt_data.Any()) {
            return RedirectToPageMessage(msgCODE.TRANSFORM_NO_MATCHING);
            }
                   
            setViewData(xml_data, xlt_data, image_data, script_data, css_data, transforms);

            return Page();
        }

     
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) {
                return Page();
            }

            var project = await _context.ge_project
                                        .FirstOrDefaultAsync(p =>p.Id==transform.projectId);
            
            if (project==null) {
                return Page();
            }
            
            transform.project = project;

            var UserAuthorised = await IsUserCreateAuthorised();
         
            if (IsCreateAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.TRANSFORM_CREATE_PROHIBITED);
            }
            if (!UserAuthorised.Succeeded) {
               return RedirectToPageMessage (msgCODE.TRANSFORM_CREATE_USER_PROHIBITED);
            }
            
            if (transform.storedprocedure == EMPTY_SELECT) {
                transform.storedprocedure = null;
            }
            
            if (!String.IsNullOrEmpty(transform.parameters)) {
                try {
                    var resource = JObject.Parse(transform.parameters);
                } catch (Newtonsoft.Json.JsonReaderException e) {
                    ModelState.AddModelError("related", e.Message);
                    return Page();
                }
            }

            UpdateCreated (transform);
                      
            _context.ge_transform.Add(transform);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index",new {projectId=transform.projectId});
        }
    }
}