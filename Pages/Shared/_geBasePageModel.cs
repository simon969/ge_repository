using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Linq;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.DAL;
namespace ge_repository.Pages.Shared
{


    public abstract class _ge_baseLayout {

        public Guid? groupId {get;set;}
        public ge_group group {get;set;}
        public Guid? projectId {get;set;}
        public ge_project project {get;set;}



    }

    public abstract class _geBasePageModel : PageModel 
    {
        protected ge_DbContext _context { get; }
        protected IAuthorizationService _authorizationService { get; }
        protected UserManager<ge_user> _userManager { get; } 
        protected ge_user _user {get;set;}
        static protected string EMPTY_SELECT = "--- SELECT ---";

        public _geBasePageModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager) : base()
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }  

        public  RedirectToPageResult RedirectToPageMessage(string message, string return_URL, logLEVEL log) {
        // RedirectToAction(string actionName, string ge_message, string fragment);
            ge_eventDAL ged = new ge_eventDAL(_context);
            string userId= _userManager.GetUserId(User);
           	ge_event ge = ged.addEvent(userId,message,return_URL,log);
                          ged.Save();
            return RedirectToPage ("/Shared/Message",new {Id = ge.Id});
           
        }
        public  RedirectToPageResult RedirectToPageMessage(int msgCODE) {
        // RedirectToAction(string actionName, string ge_message, string fragment);
            return RedirectToPage ("/Shared/Message",new {MsgId = msgCODE});
           
        }

        public virtual async Task<AuthorizationResult> IsUserAdminAuthorised() {return null;}
        public virtual async Task<AuthorizationResult> IsUserApproveAuthorised() {return null;}
        public virtual async Task<AuthorizationResult> IsUserCreateAuthorised() {return null;}
        public virtual async Task<AuthorizationResult> IsUserReadAuthorised() {return null;}
        public virtual async Task<AuthorizationResult> IsUserUpdateAuthorised() {return null;}
        public virtual async Task<AuthorizationResult> IsUserDownloadAuthorised() {return null;}
        public virtual async Task<AuthorizationResult> IsUserDeleteAuthorised() {return null;}
        public bool IsUserAdmin() {
            var AuthorisationResult = IsUserAdminAuthorised();
            return AuthorisationResult.Result.Succeeded;
        }
        public bool IsUserApprover() {
            var AuthorisationResult = IsUserApproveAuthorised();
            return AuthorisationResult.Result.Succeeded;
        }
        public virtual int IsCreateAllowed() {return geOPSResp.InvalidInput;}
        public virtual int IsReadAllowed() {return geOPSResp.InvalidInput;}
        public virtual int IsUpdateAllowed() {return geOPSResp.InvalidInput;}
        public virtual int IsDeleteAllowed() {return geOPSResp.InvalidInput;}
        public virtual int IsDownloadAllowed() {return geOPSResp.InvalidInput;}

        public async Task<string> GetUserIdAsync() {

               if (_user==null) {
                _user  = await GetUserAsync();
               }
               
               return _user.Id;

        }

        public async Task<ge_user>  GetUserAsync() {
                var claim = HttpContext.User.Claims.First(c => c.Type == "email");
                string emailAddress = claim.Value;
                var user = await _userManager.FindByEmailAsync(emailAddress);
                return user;
        }
       
        public bool UpdateLastEdited(_ge_base geb) {
            
            string userId = "";

            try {
                if (_user==null) {
                    userId =  GetUserIdAsync().Result; 
                }

            geb.editedId = _user.Id;
            geb.editedDT = DateTime.UtcNow;
            
            return true;
            
            } catch (Exception e) {
            return false;
            }
        }
        public bool UpdateCreated(_ge_base geb) {
            
            string userId = "";

            try {
                if (_user==null) {
                    userId =  GetUserIdAsync().Result; 
                }

            geb.createdId =_user.Id;
            geb.createdDT = DateTime.UtcNow;
            return true;
            } catch (Exception e) {
            return false;
            }
        }
    }
    public abstract class _geBaseLocaPageModel: _geBasePageModel {

        [BindProperty] public string locSelect {get;set;}

        public _geBaseLocaPageModel(
                            ge_DbContext context,
                            IAuthorizationService authorizationService,
                            UserManager<ge_user> userManager)
                            : base (context, authorizationService, userManager)
        {
            locSelect = "";
        } 

    public bool UpdateProjectionLoc(_ge_location loc) {
            
            bool retvar= false;

            if (loc.datumProjection==Constants.datumProjection.NONE) {
                retvar=true;
                ViewData["locMessage"] = "No datumProjection";
            } else {  
                
                ProjectionSystem ps = new ProjectionSystem();
                ige_projectionDAL pd = ps.getProjectionDAL(loc);
                
                if (pd == null) {
                    retvar = false;
                } else {
                 retvar = pd.updateAll(locSelect); 
                 ViewData["locMessage"] = pd.getMessage();  
                }
            } 

            return retvar; 
    }


    }

    public class _geBasePagedModel: _geBasePageModel
    {
        public int pageSize {get;set;}
        public int pageIndex {get;set;}
        public int pageTotal{get;set;}
        public string pageSort {get;set;}
        
        
        public string pageFilter {get;set;}

        public _geBasePagedModel (
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
            int defaultPageSize) : base(context, authorizationService, userManager)
        {
         pageSize = defaultPageSize;
         pageIndex = 1;
         pageTotal = 1;
         } 
        public bool HasPreviousPage
        {
            get
            {
                return (pageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
               return (pageIndex < pageTotal);
              
            }
        }

        public void setPaging (string pageFilter, string pageSort, int? pageIndex, int? pageSize) {
        
            this.pageFilter = pageFilter;
            this.pageSort = pageSort;
            
            if (pageIndex != null) {
            this.pageIndex = pageIndex.Value ;
            }

            if (pageSize !=null) {
            this.pageSize = pageSize.Value;    
            }
        }
    }
    public abstract class _geFullPagedTypedModel<T>: _geFullPagedModel {
        public OrderCoordinator<T> OrderCoordinator { get; private set; } = new OrderCoordinator<T>();

        public IList<T> items { get; set; }

    //    public Expression<Func<T, TResult>> orderByProperty {get; set;}
        public _geFullPagedTypedModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
            int defaultPageSize) : base(context, authorizationService, userManager, defaultPageSize)
            {}
    
    }

    public abstract class _geFullPagedModel : _geBasePagedModel {
        public Guid? groupId {get;set;}
        public ge_group group {get;set;}
        public Guid? projectId {get;set;}
        public ge_project project {get;set;}

        public Constants.PublishStatus pStatus {get;set;}
    
        public _geFullPagedModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
            int defaultPageSize) : base(context, authorizationService, userManager, defaultPageSize)
            {
            groupId = null;
            projectId = null;
            pStatus = Constants.PublishStatus.Uncontrolled;
            } 
        public void setPaging (string pageFilter, string pageSort, int? pageIndex, int? pageSize, Guid? groupId, Guid? projectId, Constants.PublishStatus? pStatus) {
            base.setPaging(pageFilter, pageSort,pageIndex, pageSize);
         
            if (groupId != null) {
                group = _context.ge_group.Find(groupId);
                if (group!=null) {
                    this.groupId = groupId.Value ;
                }
            }

            if (projectId !=null) {
                project = _context.ge_project.Find(projectId);
                if (project!=null) {
                    this.projectId = projectId.Value;
                }    
            }
                         
            if (pStatus != null) {
                this.pStatus = pStatus.Value;
            }

        }
    }

}


