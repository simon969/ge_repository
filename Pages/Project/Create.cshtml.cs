using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Pages.Shared;
using ge_repository.Extensions;
using Microsoft.Extensions.Options;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ge_repository.Pages.Project
{
    public class CreateModel : _ProjectPageModel
    {
       
        public CreateModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
            IOptions<ge_config> ge_config) : base(context, authorizationService, userManager, ge_config)
            { 
            }

        public async Task<IActionResult> OnGetAsync(Guid groupId)
            {

            var userId = GetUserIdAsync().Result; 
            var group = await _context.ge_group.FindAsync(groupId);
            
            if (group==null) {
             return NotFound();
            }
            
            project = new ge_project();
            project.name = "My Project " + (_context.GetUserProjectCount(userId) + 1).ToString();
            project.managerId = userId;
            project.createdId = userId;
            project.group = group;
            project.groupId = groupId;
            project.start_date = DateTime.UtcNow;
            project.operations = "Read;Update;Delete";
            project.data_operations = "Create;Read;Download;Update;Delete";
            
            if (IsCreateAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.PROJECT_CREATE_PROHIBITED);
            }
            var UserAuthorize =  await IsUserCreateAuthorised();      
            if (!UserAuthorize.Succeeded) {
                return RedirectToPageMessage (msgCODE.PROJECT_CREATE_USER_PROHIBITED);
            }
            var groups = await _context.GroupSearchByOperation(userId,"Create","").ToListAsync(); 
            var groupList = new SelectList(groups, "Id","name",groupId.ToString());
            ViewData["groupId"] = groupList;
            ViewData["managerId"] = _context.getGroupUsers(groups.FirstOrDefault().Id);
            ViewData["selectRUDOperations"]= Constants.RUD_OperationsArray.Select(x => new SelectListItem() { Text = x, Value = x});
            ViewData["selectCRUDDOperations"]= Constants.CRUDD_OperationsArray.Select(x => new SelectListItem() { Text = x, Value = x});

            return Page();
     }



   public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid){
        return Page();
        }

            var userId = GetUserIdAsync().Result; 
            var group = await _context.ge_group.FindAsync(project.groupId);
            
            if (group==null) {
                return Page();
            } 
            
            project.group = group;

            var UserAuthorize =  await IsUserCreateAuthorised();    
            
            if (IsCreateAllowed()!=geOPSResp.Allowed) {
                return RedirectToPageMessage (msgCODE.PROJECT_CREATE_PROHIBITED);
            }
            if (!UserAuthorize.Succeeded) {
               return RedirectToPageMessage (msgCODE.PROJECT_CREATE_USER_PROHIBITED);
            }
            
            if (String.IsNullOrEmpty(project.operations)) {
            project.operations = group.project_operations;
            }
            
            if (!UpdateProjectionLoc(project)) {
                return Page();
            }
                       
            project.createdId = userId;
            project.createdDT = DateTime.UtcNow;

            // Add Admin project_user for new project
            project.users = new List<ge_user_ops>();
            project.users.Add(new ge_user_ops { userId = userId,
                                                    user_operations = "Create;Read;Update;Delete;Download;Approve;Admin",
                                                    createdId = userId,
                                                    createdDT =  DateTime.UtcNow,
                                                    operations="Read;Update;Delete"
                                                }
                                                );
                
            _context.ge_project.Add(project);
            
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
    }
    }
}