using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Pages.Shared;
using ge_repository.Extensions;

namespace ge_repository.Pages.Data
{
    public class EditModel : _DataPageModel
    {
       
    [BindProperty]
        [Required]
        [Display(Name="Upload Files")]

         public IList<IFormFile> uploadFiles { get; set; }
        
        [BindProperty]
        public string LastModifiedDates {get;set;}
      
        public EditModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
            IOptions<ge_config> config,
            ILogger<_DataPageModel> logger
            )
            : base(context, authorizationService, userManager, config, logger)
        {
           
        }


    
        public async Task<IActionResult> OnGetAsync(Guid? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            data = await _context.ge_data
                .Include(g => g.edited)
                .Include(g => g.created)
                .Include(g => g.project).FirstOrDefaultAsync(m => m.Id == Id);

            if (data == null) {
                return NotFound();
            }

            int isUpdateAllowed = IsUpdateAllowed();
            var UserAuthorised = await IsUserUpdateAuthorised();

            if (isUpdateAllowed!=geOPSResp.Allowed) {
                if (isUpdateAllowed==geOPSResp.Data && !IsUserAdmin()) {
                    return RedirectToPageMessage (msgCODE.DATA_UPDATE_PROHIBITED);
                }
                if (isUpdateAllowed==geOPSResp.ProjectData && !IsUserAdmin()) {
                    return RedirectToPageMessage (msgCODE.DATA_UPDATE_PROHIBITED);
                }
                if (isUpdateAllowed==geOPSResp.DataApproved && !IsUserApprover()) {
                    return RedirectToPageMessage (msgCODE.DATA_UPDATE_PROHIBITED);
                }
                if (isUpdateAllowed==geOPSResp.ProjectApproved && !IsUserApprover()) {
                    return RedirectToPageMessage (msgCODE.DATA_UPDATE_PROHIBITED);
                }
            }

            if (!UserAuthorised.Succeeded && !IsUserAdmin()) {
               return RedirectToPageMessage (msgCODE.DATA_UPDATE_USER_PROHIBITED);
            }
                      
            setViewData();       
            return Page();
            
        }
       
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) {
                return Page();
            }
            
            var project = await _context.ge_project
                                   .FirstOrDefaultAsync(m => m.Id == data.projectId);
            
            if (project == null) {
                return Page();
            }
            
            data.project = project;

            int isUpdateAllowed = IsUpdateAllowed();
            var UserAuthorised = await IsUserUpdateAuthorised();

            if (isUpdateAllowed!=geOPSResp.Allowed) {
                if (isUpdateAllowed==geOPSResp.Data && !IsUserAdmin()) {
                    return RedirectToPageMessage (msgCODE.DATA_UPDATE_PROHIBITED);
                }
                if (isUpdateAllowed==geOPSResp.ProjectData && !IsUserAdmin()) {
                    return RedirectToPageMessage (msgCODE.DATA_UPDATE_PROHIBITED);
                }
                if (isUpdateAllowed==geOPSResp.DataApproved && !IsUserApprover()) {
                    return RedirectToPageMessage (msgCODE.DATA_UPDATE_PROHIBITED);
                }
                if (isUpdateAllowed==geOPSResp.ProjectApproved && !IsUserApprover()) {
                    return RedirectToPageMessage (msgCODE.DATA_UPDATE_PROHIBITED);
                }
            }
            
            if (!UserAuthorised.Succeeded && !IsUserAdmin()) {
               return RedirectToPageMessage (msgCODE.DATA_UPDATE_USER_PROHIBITED);
            }

            if (!UpdateProjectionLoc(data)) {
                return Page();
            }
                       
            if (!UpdateLastEdited(data)) {
                return Page();
            }
            

            if (uploadFiles.Count>0) {
                
                ge_MimeTypes mtypes = new ge_MimeTypes();
                string[] lastmodified = LastModifiedDates.Split(";");
                var formFile = uploadFiles[0];  
                var b = await _context.ge_data_big.FirstOrDefaultAsync(m => m.Id == data.Id);
                
                Boolean IsContentText = formFile.IsContentTypeText(true);
               
                if (IsContentText) { 
                        Boolean IsContentXML = formFile.IsContentTypeXML();
                        Encoding encoding = formFile.GetEncoding (Encoding.ASCII);
                        data.SetEncoding(encoding);  
                        if (IsContentXML) { 
                            b.data_xml = await formFile.ProcessFormFileString( ModelState, _config.defaultMaxFileSize,encoding,true);
                        } else {
                            b.data_string = await formFile.ProcessFormFileString( ModelState, _config.defaultMaxFileSize,encoding, false);
                        }
                    }  else {
                       b.data_binary = await formFile.ProcessFormFileBinary( ModelState, _config.defaultMaxFileSize);
                        data.SetEncoding(null);
                    }
                
                if (!ModelState.IsValid) {
                return Page();
                }
           
                data.data = b ;
                data.filesize = formFile.Length; 
                data.filename =  getFilenameNoPath(formFile.FileName);
                data.fileext = formFile.FileExtension();
                
                if (mtypes.ContainsKey(data.fileext)) {
                data.filetype = mtypes[data.fileext];
                } else {
                data.filetype = formFile.ContentType;    
                }

                if (IsDateTimeFormat(lastmodified[0])) {
                data.filedate = Convert.ToDateTime(lastmodified[0]);
                }
            }

            _context.Attach(data).State = EntityState.Modified;

            try {
                int DbCommandTimeout = Int32.Parse( _config.defaultEFDBTimeOut);
            
                if (DbCommandTimeout> 0 ) {
                _context.Database.SetCommandTimeout(DbCommandTimeout);     
                }
                
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!dataExists(data.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            catch (Exception e) {

                String msg= e.Message + e.InnerException.Message;
                ViewData["ExceptionMessage"] = "Exception: " + msg;
                return Page();
            }
            
           return RedirectToPage("./Index",new {projectId=data.projectId});
        }
      
    }
}
