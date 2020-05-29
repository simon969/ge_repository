using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ge_repository.Models;

using ge_repository.Authorization;
using ge_repository.Pages.Shared;
using ge_repository.Extensions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ge_repository.Pages.Group
{
    public class CreateModel : _geGroupPageModel
    {
        public CreateModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager)
            : base(context, authorizationService, userManager)
        {
           
        }
        public IActionResult OnGet()
        {
            /*  if (!IsUserAnyGroupAdmin()) {
                return RedirectToPageMessage(msgCODE.GROUP_CREATE_PROHIBITED);
            }
             */
            group =  new ge_group();
            string userId = GetUserIdAsync().Result;
            group.name = "My Group " + (_context.GetUserGroupCount(userId) + 1).ToString();
            group.managerId = userId;
            ViewData["managerId"] = _context.getGroupUsers(null);
            return Page();
        }

 
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
                       
            if (!UpdateCreated(group)) {
                return Page();
            }

            group.operations ="Read;Update;Delete";
            group.project_operations ="Create;Read;Update;Delete";
            
            if (group.datumProjection != Constants.datumProjection.NONE) {
                if (!UpdateProjectionLoc(group)) {
                return Page();
                }
            }

            // Add Admin group_user for new group
            ge_user user = await GetUserAsync();
            group.users = new List<ge_user_ops>();
            group.users.Add(new ge_user_ops { userId = user.Id,
                                                user_operations = "Create;Read;Update;Delete;Approve;Admin",
                                                createdId = user.Id,
                                                createdDT =  DateTime.UtcNow,
                                                operations="Read;Update;Delete"
                                               }
                                               );
            
            _context.ge_group.Add(group);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
       
       
    }
}