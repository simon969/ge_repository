using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Pages.Shared;
using ge_repository.Extensions;

namespace ge_repository.Pages.UserOperations
{
    public abstract class _UserOperationsPageModel : _geBaseLocaPageModel 
    {
    [BindProperty] public ge_user_ops user_ops { get; set; }

    [BindProperty] public ge_user user {get;set;}
    public ge_group group {get;set; }
    public ge_project project {get;set; }
    public _UserOperationsPageModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager) : base(context, authorizationService, userManager)
            { }
    public async override Task<AuthorizationResult> IsUserApproveAuthorised() {
            if (project!=null) {
                var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,project,geOPS.Approve);
                return AuthorizationResult;
            }
            if (group!=null) {
                var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,group,geOPS.Approve);
                return AuthorizationResult;
            }
        return null;
    }
    public async override Task<AuthorizationResult> IsUserAdminAuthorised() {
            if (project!=null) {
                var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,project,geOPS.Admin);
                return AuthorizationResult;
            }
            if (group!=null) {
                var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,group,geOPS.Admin);
                return AuthorizationResult;
            }
            return null;
    }
     public async override Task<AuthorizationResult> IsUserCreateAuthorised() {
            if (project!=null) { 
                var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,project,geOPS.Create);
                return AuthorizationResult;
            }
            if (group!=null) { 
                var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,group,geOPS.Create);
                return AuthorizationResult;
            }
        return null;

    }
    public async override Task<AuthorizationResult> IsUserReadAuthorised() {
            if (project!=null) {  
                var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,project,geOPS.Read);
                return AuthorizationResult;
            }
            if (group!=null) {  
                var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,group,geOPS.Read);
                return AuthorizationResult;
            }
        return null;
    }
    public async override Task<AuthorizationResult> IsUserUpdateAuthorised() {
            if (project!=null) { 
                var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,project,geOPS.Update);
                return AuthorizationResult;
            }
            if (group!=null) { 
                var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,group,geOPS.Update);
                return AuthorizationResult;
            }
        return null;
    }
    public async override Task<AuthorizationResult> IsUserDeleteAuthorised() {
            if (project!=null) { 
                var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,project,geOPS.Delete);
                return AuthorizationResult;
            }
            if (group!=null) { 
                var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,group,geOPS.Delete);
                return AuthorizationResult;
            }
        return null;

    }
    public Boolean IsUserLastAdmin(string userId) {
            if (project!=null) { 
                return _context.IsUserLastAdmin(project,userId);
            }
            if (group!=null) {
                return _context.IsUserLastAdmin(group,userId);
            }
            return false;
    }
    public override int IsCreateAllowed() {
    return _context.IsOperationAllowed(Constants.CreateOperationName, user_ops);
    }
    public override int IsUpdateAllowed() {
    return _context.IsOperationAllowed(Constants.UpdateOperationName, user_ops);
    }
    public override int IsDeleteAllowed() {
    return _context.IsOperationAllowed(Constants.DeleteOperationName, user_ops);
    }
    public override int IsReadAllowed() {
    return _context.IsOperationAllowed(Constants.ReadOperationName, user_ops);
    }
    protected bool projectExists(Guid id)         {
            return _context.ge_project.Any(e => e.Id == id);
    }
    protected void setViewData(){
           
           ViewData["selectCRUDDAAOperations"]= Constants.CRUDDAA_OperationsArrayIndividual.Select(x => new SelectListItem() { Text = x, Value = x});
           ViewData["selectGroup"] = _context.getGroupsWhereUserAdmin(GetUserIdAsync().Result);
           ViewData["selectProject"] = _context.getProjectsWhereUserAdmin(GetUserIdAsync().Result);
          
           
    }
}
}