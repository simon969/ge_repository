using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Pages.Shared;

namespace ge_repository.Pages.Data
{
    public abstract class _DataPageModel : _geBaseLocaPageModel 
    {
    [BindProperty] public ge_data data { get; set; }
     public ge_config _config {get; set;}
     public ILogger<_DataPageModel> _logger;
    public _DataPageModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
            IOptions<ge_config> ge_config,
            ILogger<_DataPageModel> logger)
             : base(context, authorizationService, userManager)
            {
            _config = ge_config.Value;
            _logger = logger;
             }
    public async override Task<AuthorizationResult> IsUserApproveAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,data,geOPS.Approve);
        return AuthorizationResult;
    }
    public async override Task<AuthorizationResult> IsUserAdminAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,data,geOPS.Admin);
        return AuthorizationResult;
    }
     public async override Task<AuthorizationResult> IsUserCreateAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,data,geOPS.Create);
        return AuthorizationResult;
    }
    public async override Task<AuthorizationResult> IsUserReadAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,data,geOPS.Read);
        return AuthorizationResult;
    }
    public async override Task<AuthorizationResult> IsUserUpdateAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,data,geOPS.Update);
        return AuthorizationResult;
    }
    public async override Task<AuthorizationResult> IsUserDeleteAuthorised() {
        var AuthorizationResult = await _authorizationService.AuthorizeAsync(User,data,geOPS.Delete);
        return AuthorizationResult;
    }

    public override int IsCreateAllowed() {
    return _context.IsOperationAllowed(Constants.CreateOperationName, data.project, data);
    }
    public override int IsUpdateAllowed() {
    return _context.IsOperationAllowed(Constants.UpdateOperationName, data.project, data);
    }
    public override int IsDeleteAllowed() {
    return _context.IsOperationAllowed(Constants.DeleteOperationName, data.project, data);
    }
    public override int IsReadAllowed() {
    return _context.IsOperationAllowed(Constants.ReadOperationName, data.project, data);
    }
    protected bool dataExists(Guid id)         {
            return _context.ge_data.Any(e => e.Id == id);
    }
     public void setViewData(){
         if (data.project!=null) {
            ViewData["ProjectName"] =  data.project.name;
         }
       //     ViewData["selectCRUDDOperations"]= Constants.CRUDD_OperationsArray.Select(x => new SelectListItem() { Text = x, Value = x, Selected = x == data.operations });
            ViewData["selectRUDDOperations"]= Constants.RUDD_OperationsArray.Select(x => new SelectListItem() { Text = x, Value = x, Selected = x == data.operations });
            ViewData["pstatus"] = new SelectList(Enum.GetValues(typeof(Constants.PublishStatus))); 
     
        }
         public string getFilenameNoPath(string filename)
    {
        if (filename.Contains("\\"))
            filename = filename.Substring(filename.LastIndexOf("\\") + 1);

        return filename;
   }
    public Boolean IsDateTimeFormat(string s1) {
        try {
            DateTime dt = Convert.ToDateTime(s1);
            return true;

        } catch (FormatException fe){
            return false;
        } catch (Exception e) {
            return false;
        }
      
    }   
}
}
    