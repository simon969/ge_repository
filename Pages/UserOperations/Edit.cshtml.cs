using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using ge_repository.Models;
using ge_repository.Pages.Shared;
using ge_repository.Authorization;
using ge_repository.Extensions;

namespace ge_repository.Pages.UserOperations
{
   public class EditModel :    _UserOperationsPageModel
        {
             
        public EditModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager) : base(context, authorizationService, userManager)
            { }
        
        public async Task<IActionResult> OnGetAsync(Guid? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            
            user_ops = await _context.ge_user_ops
                                        .Include(o => o.created)
                                        .Include(o => o.edited)
                                        .Include(o => o.group)
                                        .Include(o => o.project)
                                        .Include(o => o.user).FirstOrDefaultAsync(m => m.Id == Id);
            
            if (user_ops == null) {
             RedirectToPageMessage(msgCODE.USER_OPS_NOTFOUND); 
            }
            
            group = user_ops.group;
            project = user_ops.project;

            var userId = GetUserIdAsync().Result; 
            
            if (user_ops.group != null) {
                var authorisationResult = IsUserAdminAuthorised();
                if (!authorisationResult.Result.Succeeded) {
                     return RedirectToPageMessage(msgCODE.GROUP_OPERATION_UPDATE_ADMINREQ); 
                } 
            }

            if (user_ops.project != null) {
                var authorisationResult = IsUserAdminAuthorised();
                if (!authorisationResult.Result.Succeeded) {
                    return RedirectToPageMessage(msgCODE.PROJECT_OPERATION_UPDATE_ADMINREQ);
                }
            } 
            
                setViewData();

                return Page();     
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            if (user_ops.groupId != null && user_ops.projectId !=null) {
                return RedirectToPageMessage(msgCODE.USER_OPS_CREATE_AMBIGUOUS); 
            }
            
            var userId= GetUserIdAsync().Result;
            

            if (user_ops.groupId != null) {

                group =  await _context.ge_group.FirstAsync(g =>g.Id==user_ops.groupId);
                
                var authorisationResult = IsUserAdminAuthorised();
                if (!authorisationResult.Result.Succeeded) {
                     return RedirectToPageMessage(msgCODE.GROUP_OPERATION_UPDATE_ADMINREQ); 
                } 
                
                bool userIsLastAdmin = IsUserLastAdmin(user_ops.userId);
                bool operationHasAdminRole = user_ops.user_operations.Contains(Constants.AdminOperationName);
                
                if (userIsLastAdmin && !operationHasAdminRole) {
                    return RedirectToPageMessage(msgCODE.GROUP_OPERATION_UPDATE_MINADMIN);
                }
            }

            if (user_ops.projectId != null) {

                project =  await _context.ge_project.FirstAsync(p =>p.Id==user_ops.projectId);
                
                var authorisationResult = IsUserAdminAuthorised();
                if (!authorisationResult.Result.Succeeded) {
                    return RedirectToPageMessage(msgCODE.PROJECT_OPERATION_UPDATE_ADMINREQ);
                }
                
                bool userIsLastAdmin = IsUserLastAdmin(user_ops.userId);
                bool operationHasAdminRole = user_ops.user_operations.Contains(Constants.AdminOperationName);
                
                if (userIsLastAdmin && !operationHasAdminRole) {
                     return RedirectToPageMessage(msgCODE.PROJECT_OPERATION_UPDATE_MINADMIN);
                }
            }
                
            user_ops.editedId = userId;
            user_ops.editedDT = DateTime.UtcNow;
              
            _context.Attach(user_ops).State = EntityState.Modified;
              
            await _context.SaveChangesAsync();
            
            if (user_ops.projectId != null) {    
            return RedirectToPage("./Index",  new {projectId= user_ops.projectId}); 
            }
            
            if (user_ops.groupId != null) {    
            return RedirectToPage("./Index",  new {groupId= user_ops.groupId}); 
            }

            return Page();
        }
    }

}
