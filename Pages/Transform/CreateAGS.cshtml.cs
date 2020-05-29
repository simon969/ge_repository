using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Newtonsoft.Json;
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
using ge_repository.AGS;

namespace ge_repository.Pages.Transform
{
    public class CreateAGSModel : _TransformPageModel
    {
      [BindProperty] public ge_transform project {get;set;}
      [BindProperty] public ge_transform hole {get;set;}
      [BindProperty] public ge_transform table {get;set;}

       public CreateAGSModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
            IOptions<ge_config> config
            )
            : base(context, authorizationService, userManager, config)
        {
           
        }

        public async Task<IActionResult> OnGetAsync(Guid dataId) {

            if (dataId == null) {
                return NotFound();
            }
        
            var data = await _context.ge_data.Include (d => d.project)
                                                 .Where(d => d.Id == dataId)
                                                 .Where(d=>d.fileext == AGS.FileExtension.XML)
                                                 .FirstOrDefaultAsync();
            if (data == null) {
                return NotFound();
            }

            var task_ags_version = await getAGSVersion(dataId);
            string ags_version = task_ags_version.Value;

            if (String.IsNullOrEmpty(ags_version)) {
                return RedirectToPageMessage (msgCODE.TRANSFORM_AGS_NONE);
            }

             // Create a new transform to check authorisations
            transform =  new ge_transform();
            transform.dataId = dataId; 
            transform.project = data.project;
            transform.projectId = data.projectId;
            
            var UserAuthorised = await IsUserCreateAuthorised();
         
            if (IsCreateAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.TRANSFORM_CREATE_PROHIBITED);
            }

            if (!UserAuthorised.Succeeded) {
               return RedirectToPageMessage (msgCODE.TRANSFORM_CREATE_USER_PROHIBITED);
            }
                        
            ge_transform_parameters ags_transforms =  getAGSTransforms(ags_version);
            
            if (ags_transforms==null) {
                return RedirectToPageMessage (msgCODE.TRANSFORM_AGS_NONE); 
            }
            
         
            
            project =  new ge_transform(); 
            project.Id =  Guid.NewGuid();
            project.projectId = data.projectId;
            project.name ="Project Summaries for " +  data.filename ;
            project.description = "HTML transforms for AGSML Project (" + GetTimestamp(DateTime.Now) + ")";
            project.styleId = Guid.Parse(ags_transforms.project);
            project.operations = data.project.data_operations;
            
            hole =  new ge_transform(); 
            hole.Id = Guid.NewGuid();
            hole.projectId = data.projectId;
            hole.name ="Hole Summaries for " + data.filename;
            hole.description = "HTML transforms for AGSML Hole (" + GetTimestamp(DateTime.Now) + ")";
            hole.styleId = Guid.Parse(ags_transforms.hole);
            hole.operations = data.project.data_operations;

            table = new ge_transform(); 
            table.Id = Guid.NewGuid();
            table.projectId = data.projectId;
            table.name ="Table Summaries for " + data.filename;
            table.description = "HTML transforms for AGSML Tables (" + GetTimestamp(DateTime.Now) +")";
            table.styleId = Guid.Parse(ags_transforms.table);
            table.operations = data.project.data_operations;
            
            ge_transform_parameters tparams = new ge_transform_parameters();
            tparams.project = project.Id.ToString();
            tparams.hole = hole.Id.ToString();
            tparams.table =  table.Id.ToString();
                    
            string json_tparams = JsonConvert.SerializeObject(tparams);
            
            transform.parameters = json_tparams;

        //     /*Add prospective transforms to list incase user wants to rebuilds json string */
        //     ge_transform[] Addtransforms = {project, hole, table};
            
        //     var Alltransforms = transforms.Concat(Addtransforms);

        //     ViewData["styleId"] = new SelectList(xlt_data, "Id", "filename");
        //     ViewData["transformId"] =  new SelectList(Alltransforms, "Id", "name");
        //     string[] p = new [] {"project","hole","table","geol"};
        //     ViewData["selectparameter"] = new SelectList(p); 
            
            setViewData();
            return Page();
        }
        public static String GetTimestamp(DateTime value) {
            return value.ToString("yyyyMMddHHmmssffff");
        }
            
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) {
                return Page();
            }

            var Project = await _context.ge_project
                                        .FirstOrDefaultAsync(p =>p.Id==transform.projectId);
            
            if (Project==null) {
                return Page();
            }
            
            transform.project = Project;

            var UserAuthorised = await IsUserCreateAuthorised();
         
            if (IsCreateAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.TRANSFORM_CREATE_PROHIBITED);
            }
            if (!UserAuthorised.Succeeded) {
               return RedirectToPageMessage (msgCODE.TRANSFORM_CREATE_USER_PROHIBITED);
            }

            if (String.IsNullOrEmpty(transform.parameters)) {
                return Page();
            }   
            
            
             try {
                
                var p = JObject.Parse(transform.parameters);
  
                project.dataId = transform.dataId; 
                project.projectId = transform.projectId;
                project.parameters = transform.parameters;
                UpdateCreated (project);

                hole.dataId = transform.dataId; 
                hole.projectId = transform.projectId;
                hole.parameters = transform.parameters;
                UpdateCreated (hole);

                table.dataId = transform.dataId; 
                table.projectId = transform.projectId;
                table.parameters = transform.parameters;
                UpdateCreated (table);
            
                _context.ge_transform.Add (project);
                _context.ge_transform.Add (hole);
                _context.ge_transform.Add (table);

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index",new {projectId=transform.projectId}); 
                
                } catch (Newtonsoft.Json.JsonReaderException e) {
                        ModelState.AddModelError("transform.parameters", e.Message);
                        return Page();
                } catch (Exception e) {
                        return Page();
                }
        }

         public async Task<ActionResult<String>> getAGSVersion(Guid dataId)
        {
            string ags_version = "";
            
            var connection = _context.Database.GetDbConnection();
            
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "[dbo].[fn_agsml_getagsversion]";
            cmd.CommandType = CommandType.StoredProcedure;

            DbParameter pdataId = cmd.CreateParameter();
            pdataId.ParameterName = "@data";
            pdataId.DbType = DbType.Guid;
            pdataId.Direction = ParameterDirection.Input;
            pdataId.Value = dataId;            
            
            DbParameter pReturn = cmd.CreateParameter();
            pReturn.ParameterName = "@return";
            pReturn.DbType = DbType.String;
            pReturn.Direction = ParameterDirection.ReturnValue;
            
            cmd.Parameters.Add(pdataId);
            cmd.Parameters.Add(pReturn);
            
            try
                {
                    var x = await cmd.ExecuteNonQueryAsync();
                    ags_version = (string) pReturn.Value;
                    return (ags_version);
                }
            catch (Exception ex)
                {
                        return "";
                }
        }

    }
}