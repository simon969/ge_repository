using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Pages.Shared;
using ge_repository.Extensions;

namespace ge_repository.Pages.Project
{
    public abstract class _ProjectPageModel : _geBaseLocaPageModel 
    {
    [BindProperty] public ge_project project { get; set; }
    public ge_config _config {get; set;}
    public _ProjectPageModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager, 
            IOptions<ge_config> ge_config) : base(context, authorizationService, userManager)
            { 
                 _config = ge_config.Value; 
            }
    public async override Task<AuthorizationResult> IsUserApproveAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,project,geOPS.Approve);
        
        return AuthorizationResult;
    }
    public async override Task<AuthorizationResult> IsUserAdminAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,project,geOPS.Admin);
        return AuthorizationResult;
    }
     public async override Task<AuthorizationResult> IsUserCreateAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,project,geOPS.Create);
        return AuthorizationResult;
    }
    public async override Task<AuthorizationResult> IsUserReadAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,project,geOPS.Read);
        return AuthorizationResult;
    }
    public async override Task<AuthorizationResult> IsUserUpdateAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,project,geOPS.Update);
        return AuthorizationResult;
    }
    public async override Task<AuthorizationResult> IsUserDeleteAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,project,geOPS.Delete);
        return AuthorizationResult;
    }

    public override int IsCreateAllowed() {
    return _context.IsOperationAllowed(Constants.CreateOperationName, project.group, project);
    }
    public override int IsUpdateAllowed() {
    return _context.IsOperationAllowed(Constants.UpdateOperationName, project.group, project);
    }
    public override int IsDeleteAllowed() {
    return _context.IsOperationAllowed(Constants.DeleteOperationName, project.group, project);
    }
    public override int IsReadAllowed() {
    return _context.IsOperationAllowed(Constants.ReadOperationName, project.group, project);
    }
    protected bool projectExists(Guid id)         {
            return _context.ge_project.Any(e => e.Id == id);
    }
    protected void setViewData() {

            Guid[] projectId = _config.templateProjectId_ToGuid(project.Id);
          
            ViewData["selectRUDOperations"]= Constants.RUD_OperationsArray.Select(x => new SelectListItem() { Text = x, Value = x, Selected = x == project.operations });
            ViewData["selectCRUDDOperations"]= Constants.CRUDD_OperationsArray.Select(x => new SelectListItem() { Text = x, Value = x, Selected = x == project.data_operations });
            ViewData["managerId"] = _context.getGroupUsers(project.groupId);
            ViewData["groupId"] = _context.getUserGroups(GetUserIdAsync().Result);
            ViewData["homepageId"] = _context.getProjectTransforms(project.Id);
            ViewData["esriConnectId"] = _context.getProjectData(projectId, AGS.FileExtension.XML);
            ViewData["otherDbConnectId"] = _context.getProjectData(projectId, AGS.FileExtension.XML);
    }
}
}
    