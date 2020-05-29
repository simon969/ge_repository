using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using ge_repository.Models;
using ge_repository.Pages.Shared;
using ge_repository.Authorization;
using ge_repository.Extensions;

namespace ge_repository.Pages.UserOperations
{
    public class DeleteModel : _UserOperationsPageModel {
        
        public DeleteModel(
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
                                        .Include(g => g.created)
                                        .Include(g => g.edited)
                                        .Include(g => g.group)
                                        .Include(g => g.user).FirstOrDefaultAsync(m => m.Id == Id);
            
            if (user_ops == null) {
              return NotFound();
            } 
            
            var userId = GetUserIdAsync().Result;
            
            if (user_ops.group != null) {
                bool IsUserGroupAdmin = _context.DoesUserHaveOperation(Constants.AdminOperationName, user_ops.group, userId);
                if (!IsUserGroupAdmin) {
                    return RedirectToPageMessage(msgCODE.GROUP_OPERATION_DELETE_ADMINREQ); 
                } 
                 
                return Page(); 
            }
            
            if (user_ops.project != null) {
                bool IsUserProjectAdmin = _context.DoesUserHaveOperation(Constants.AdminOperationName, user_ops.project, userId);
                if (!IsUserProjectAdmin) {
                    return RedirectToPageMessage(msgCODE.PROJECT_OPERATION_DELETE_ADMINREQ); 
                } 
                return Page(); 
            }
            
            return NotFound();
        }
        
        public async Task<IActionResult> OnPostAsync(Guid Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            user_ops = await _context.ge_user_ops
                                        .Include(g => g.group)
                                        .Include(g => g.project)
                                        .FirstOrDefaultAsync(m => m.Id == Id);

            if (user_ops == null) {
                 return NotFound(); 
            }                            
            
            var userId = GetUserIdAsync().Result;
                        
           
            if (user_ops.group != null) {
                bool IsUserGroupAdmin = _context.DoesUserHaveOperation(Constants.AdminOperationName, user_ops.group, userId);
                if (!IsUserGroupAdmin) {
                    return RedirectToPageMessage(msgCODE.GROUP_OPERATION_DELETE_ADMINREQ); 
                } 
            }
            
            if (user_ops.project != null) {
                bool IsUserProjectAdmin = _context.DoesUserHaveOperation(Constants.AdminOperationName, user_ops.group, userId);
                if (!IsUserProjectAdmin) {
                    return RedirectToPageMessage(msgCODE.PROJECT_OPERATION_DELETE_ADMINREQ); 
                } 
            }
            
                _context.ge_user_ops.Remove(user_ops);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index"); 
           
        }
    }
}
