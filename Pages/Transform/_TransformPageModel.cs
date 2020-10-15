using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Pages.Shared;
using ge_repository.Extensions;

namespace ge_repository.Pages.Transform
{
    public abstract class _TransformPageModel : _geBaseLocaPageModel 
    {
    [BindProperty] public ge_transform transform { get; set; }
    public ge_config _config {get; set;}

    public _TransformPageModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
            IOptions<ge_config> ge_config) : base(context, authorizationService, userManager)
            {
            _config = ge_config.Value;
             }
    public async override Task<AuthorizationResult> IsUserApproveAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,transform,geOPS.Approve);
        return AuthorizationResult;
    }
    public async override Task<AuthorizationResult> IsUserAdminAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,transform,geOPS.Admin);
        return AuthorizationResult;
    }
     public async override Task<AuthorizationResult> IsUserCreateAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,transform,geOPS.Create);
        return AuthorizationResult;
    }
    public async override Task<AuthorizationResult> IsUserReadAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,transform,geOPS.Read);
        return AuthorizationResult;
    }
    public async override Task<AuthorizationResult> IsUserUpdateAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,transform,geOPS.Update);
        return AuthorizationResult;
    }
    public async override Task<AuthorizationResult> IsUserDeleteAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,transform,geOPS.Delete);
        return AuthorizationResult;
    }

    public override int IsCreateAllowed() {
    return _context.IsOperationAllowed(Constants.CreateOperationName, transform.project, transform);
    }
    public override int IsUpdateAllowed() {
    return _context.IsOperationAllowed(Constants.UpdateOperationName, transform.project, transform);
    }
    public override int IsDeleteAllowed() {
    return _context.IsOperationAllowed(Constants.DeleteOperationName, transform.project, transform);
    }
    public override int IsReadAllowed() {
    return _context.IsOperationAllowed(Constants.ReadOperationName, transform.project, transform);
    }
    protected bool transformExists(Guid id)         {
            return _context.ge_transform.Any(e => e.Id == id);
    }

   
   

    public void setViewData() {

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
             // .xq, .xql, .xqm, .xqy, and .xquery.
            var xqy_data =  _context.ge_data
                                        .Where(d=>d.projectId==transform.projectId || libraryId.Contains(d.projectId))
                                        .Where(d=>d.filetype.Contains("xq"));      
            
            var xlt_data =  _context.ge_data
                                        .Where(d=>d.projectId==transform.projectId || libraryId.Contains(d.projectId))
                                        .Where(d=>d.fileext == AGS.FileExtension.XSL);        
            var transforms = _context.ge_transform
                                        .Include (t=>t.style)
                                        .Where(t=>t.projectId==transform.projectId);
        
        setViewData(xml_data, xlt_data, xqy_data, image_data, script_data, css_data, transforms);                                   

    }
     public void setViewData(   IQueryable<ge_data> xml_data, 
                                IQueryable<ge_data> xlt_data, 
                                IQueryable<ge_data> xqy_data,
                                IQueryable<ge_data> image_data, 
                                IQueryable<ge_data> script_data,
                                IQueryable<ge_data> css_data, 
                                IQueryable<ge_transform> transforms){

            ViewData["dataId"] = new SelectList(xml_data, "Id","filename");
            ViewData["styleId"] = new SelectList(xlt_data, "Id", "filename");
            ViewData["queryId"] = new SelectList(xqy_data, "Id", "filename");
            ViewData["selectId"] = getMergedList(image_data, script_data, css_data, transforms);
            ViewData["selectSP"] = new SelectList(_config.stored_procedures);
            string[] p = new [] {"project","projectId","hole","table","group","groupId","data","Id","image","dictionary","script","css"};
            ViewData["selectParam"] = new SelectList(p);
     }

     public ge_transform_parameters getAGSTransforms(string version) {
         foreach (var ags_transform in _config.transform_parameters) {
             if (ags_transform.version == version) {
                return ags_transform;
             }
         }
         return null;
     }
     public SelectList getMergedList(   IQueryable<ge_data> image_data, 
                                        IQueryable<ge_data> script_data,
                                        IQueryable<ge_data> css_data,
                                        IQueryable<ge_transform> transforms) {
    
        List<SelectListItem> list= new List<SelectListItem>();

        foreach (var t in transforms) {
                string details; 
                if (t.style==null) {
                    details = t.storedprocedure;
                } else {
                    details = t.style.filename;
                }
                SelectListItem item =  new SelectListItem(t.name + "," + t.description + "(" + details + ")" ,t.Id.ToString());
                list.Add (item);
        }

        foreach (var i in image_data) {
                SelectListItem item =  new SelectListItem(i.description + "(" + i.filename + ")" ,i.Id.ToString());
                list.Add (item);
        }
        
        foreach (var i in script_data) {
                SelectListItem item =  new SelectListItem(i.description + "(" + i.filename + ")" ,i.Id.ToString());
                list.Add (item);
        }

        foreach (var i in css_data) {
                SelectListItem item =  new SelectListItem(i.description + "(" + i.filename + ")" ,i.Id.ToString());
                list.Add (item);
        }

        return new Microsoft.AspNetCore.Mvc.Rendering.SelectList(list,"Value","Text", null);


     }
}
}
    