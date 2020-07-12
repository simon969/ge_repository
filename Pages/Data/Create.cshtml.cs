using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Pages.Shared;
using ge_repository.Extensions;
using ge_repository.DAL;

namespace ge_repository.Pages.Data
{
    public class CreateModel : _DataPageModel
    {   
              
        
        [BindProperty]
        [Required]
        [Display(Name="Upload Files")]

         public IList<IFormFile> uploadFiles { get; set; }
        
        [BindProperty]
        public string LastModifiedDates {get;set;}

        public CreateModel(

            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
            IOptions<ge_config> config,
            ILogger<_DataPageModel> logger
            )
            : base(context, authorizationService, userManager, config, logger)
        {
           
        }

      public async Task<IActionResult> OnGetAsync(Guid projectId)
        {
             if (projectId == null) {
                return NotFound();
            }
        
            var project = await _context.projectFull(projectId).FirstOrDefaultAsync();

            if (project == null) {
                return NotFound();
            }
            
      
            data = new ge_data();

            data.project = project;

            var UserAuthorised = await IsUserCreateAuthorised(); 
            
            if (IsCreateAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.DATA_CREATE_PROHIBITED);
            }
            if (!UserAuthorised.Succeeded) {
               return RedirectToPageMessage (msgCODE.DATA_CREATE_USER_PROHIBITED);
            }

            _logger.LogInformation ("OnGetAsync called:" + projectId.ToString());
           
            data.locEast=  project.locEast;
            data.locNorth = project.locNorth;
            data.locLevel = project.locLevel;

            data.locName = project.locName;
            data.locAddress = project.locAddress;
            data.locPostcode = project.locPostcode;

            data.locLatitude =project.locLatitude;
            data.locLongitude =project.locLongitude;
            data.locHeight = project.locHeight;

            data.locMapReference = project.locMapReference;
            data.datumProjection = project.datumProjection;

            data.projectId = project.Id;
            data.operations = Constants.RUDD_OperationsArray[Constants.ReadDownloadUpdateDelete];
            
            setViewData();
            return Page();

        } 

    public async Task<IActionResult> OnPostAsync()
    {
            try { 
            
            ViewData["ExceptionMessage"]="No Exceptions";

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
            
            if (!UpdateProjectionLoc(data)) {
                setViewData();
                return Page();
            }
                       
            if (String.IsNullOrEmpty(LastModifiedDates)) {
                setViewData();
                return Page();
            } 



            string[] lastmodified = LastModifiedDates.Split(";");
            int i = 0;
            
            ge_MimeTypes mtypes = new ge_MimeTypes();
            
            foreach (var formFile in uploadFiles)
                {
                    Boolean IsContentText = formFile.IsContentTypeText(true);
     
                    ge_data_big b = new ge_data_big();
                    ge_data d = new ge_data();

                    d.createdId = GetUserIdAsync().Result;    
                    
                     if (IsContentText) { 
                    Boolean IsContentXML = formFile.IsContentTypeXML();
                    if (IsContentXML) { 
                            b.data_xml = await formFile.ProcessFormFileString( ModelState, _config.defaultMaxFileSize,Encoding.UTF8,true);
                            d.SetEncoding(Encoding.UTF8);
                    } else {
                            Encoding encoding = formFile.ReadEncoding (Encoding.UTF8);
                            b.data_string = await formFile.ProcessFormFileString( ModelState, _config.defaultMaxFileSize,encoding, false);
                            d.SetEncoding(encoding); 
                    }
                }  else {
                    b.data_binary = await formFile.ProcessFormFileBinary( ModelState, _config.defaultMaxFileSize);
                    d.SetEncoding(null);
                }

                    // Perform a second check to catch ProcessFormFile method
                    // violations.
                    if (!ModelState.IsValid) {
                    return Page();
                    }
                    
                    d.projectId = project.Id;

                    d.locEast = data.locEast;
                    d.locNorth = data.locNorth;
                    d.locLevel = data.locLevel;

                    d.locLatitude = data.locLatitude;
                    d.locLongitude =data.locLongitude;
                    d.locHeight = data.locHeight;
                    
                    d.locMapReference = data.locMapReference;
                    d.locName = data.locName;
                    d.locAddress = data.locAddress;
                    d.locPostcode = data.locPostcode;
                    
                    d.datumProjection = data.datumProjection ;

                    d.description = data.description;
                    d.keywords = data.keywords;
                    d.operations = data.operations;
                     
                    // Add deatils of uploaded file to new _ge_data record
                    d.data = b ;
                    d.filesize = formFile.Length; 
                    d.filename = getFilenameNoPath(formFile.FileName);
                    d.fileext = formFile.FileExtension();
                   
                   if (mtypes.ContainsKey(d.fileext)) {
                    d.filetype = mtypes[d.fileext];
                    } else {
                    d.filetype = formFile.ContentType;    
                    }

                    if (IsDateTimeFormat(lastmodified[i]))    {
                    d.filedate = Convert.ToDateTime(lastmodified[i]);
                    }
                    
                    if (!UpdateCreated(d)) {
                        return Page();
                    }
                    
                    if (!UpdateLastEdited(d)) {
                        return Page();
                    }
                  
                  _context.ge_data.Add(d);
               }
            
            int DbCommandTimeout = Int32.Parse( _config.defaultEFDBTimeOut);
            
            if (DbCommandTimeout> 0 ) {
            _context.Database.SetCommandTimeout(DbCommandTimeout);     
            }

            await _context.SaveChangesAsync();
            
            return RedirectToPage("./Index",new {projectId=data.projectId});

            } catch (Exception e){

                String msg= e.Message + e.InnerException.Message;
                ViewData["ExceptionMessage"] = "Exception: " + msg;
                return Page();
            }
           
    } 
 
}
}
