using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;

using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Pages.Shared;
using ge_repository.Extensions;

namespace ge_repository.Pages.Group
{
    public abstract class _geGroupPageModel : _geBaseLocaPageModel 
    { 
        [BindProperty] public ge_group group { get; set; }
    public _geGroupPageModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager) : base(context, authorizationService, userManager)
            { }
    public async override Task<AuthorizationResult> IsUserApproveAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,group,geOPS.Approve);
        return AuthorizationResult;
    }
    public async override Task<AuthorizationResult> IsUserAdminAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,group,geOPS.Admin);
        return AuthorizationResult;
    }
     public async override Task<AuthorizationResult> IsUserCreateAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,group,geOPS.Create);
        return AuthorizationResult;
    }
    public async override Task<AuthorizationResult> IsUserReadAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,group,geOPS.Read);
        return AuthorizationResult;
    }
    public async override Task<AuthorizationResult> IsUserUpdateAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,group,geOPS.Update);
        return AuthorizationResult;
    }
    public async override Task<AuthorizationResult> IsUserDeleteAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,group,geOPS.Delete);
        return AuthorizationResult;
    }
    public bool IsUserAnyGroupAdmin() {    
        return _context.IsUserAnyGroupAdmin(GetUserIdAsync().Result);
    }
    public override int IsCreateAllowed() {
    return _context.IsOperationAllowed(Constants.CreateOperationName, group);
    }
    public override int IsUpdateAllowed() {
    return _context.IsOperationAllowed(Constants.UpdateOperationName,group);
    }
    public override int IsDeleteAllowed() {
    return _context.IsOperationAllowed(Constants.DeleteOperationName, group);
    }
    public override int IsReadAllowed() {
    return _context.IsOperationAllowed(Constants.ReadOperationName, group);
    }
    protected bool groupExists(Guid id)         {
            return _context.ge_group.Any(e => e.Id == id);
    }
    protected void setViewData(){

       

            ViewData["selectRUDOperations"]= Constants.RUD_OperationsArray.Select(x => new SelectListItem() { Text = x, Value = x , Selected = x == group.operations });
            ViewData["selectCRUDDOperations"]= Constants.CRUDD_OperationsArray.Select(x => new SelectListItem() { Text = x, Value = x, Selected = x == group.project_operations });
            ViewData["locMessage"]= "";
            ViewData["managerId"] = _context.getGroupUsers(group.Id);
            ViewData["homepageId"] = _context.getGroupTransforms(group.Id);
    }
}
}
    