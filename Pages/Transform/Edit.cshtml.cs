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

using Newtonsoft.Json.Linq;

using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Pages.Shared;
using ge_repository.Extensions;
namespace ge_repository.Pages.Transform
{
    public class EditModel : _TransformPageModel
    {
        
        public EditModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
            IOptions<ge_config> config
            )
            : base(context, authorizationService, userManager, config)
        {
           
        }

       

        public async Task<IActionResult> OnGetAsync(Guid? Id)
        {
            if (Id == null) {
                return NotFound();
            }

            transform = await _context.ge_transform
                .Include(g => g.created)
                .Include(g => g.data)
                .Include(g => g.edited)
                .Include(g => g.project)
                .Include(g => g.style).FirstOrDefaultAsync(m => m.Id == Id);

            if (transform == null) {
                return NotFound();
            }
            
            var UserAuthorised = await IsUserUpdateAuthorised();

            if (IsUpdateAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.TRANSFORM_UPDATE_PROHIBITED);
            }
            if (!UserAuthorised.Succeeded) {
               return RedirectToPageMessage (msgCODE.TRANSFORM_UPDATE_USER_PROHIBITED);
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            /* get parent project so that update authorisation can be confirmed */
            var project = await _context.ge_project
                                   .FirstOrDefaultAsync(m => m.Id == transform.projectId);
            
            if (project == null) {
                return Page();
            }
            
            
            transform.project = project;

            var UserAuthorised = await IsUserUpdateAuthorised();

            if (IsUpdateAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.TRANSFORM_UPDATE_PROHIBITED);
            }
            if (!UserAuthorised.Succeeded) {
               return RedirectToPageMessage (msgCODE.TRANSFORM_UPDATE_USER_PROHIBITED);
            }
            
            if (!String.IsNullOrEmpty(transform.parameters)) {
                try {
                    /* check that transform.parameters is valid json text */
                    var resource = JObject.Parse(transform.parameters);
                 } catch (Newtonsoft.Json.JsonReaderException e) {
                    ModelState.AddModelError("transform.parameters", e.Message);
                    return Page();
                }
            }
            
            UpdateLastEdited (transform);

            _context.Attach(transform).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!transformExists(transform.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

             return RedirectToPage("./Index",new {projectId=transform.projectId});
        }

    }
}
