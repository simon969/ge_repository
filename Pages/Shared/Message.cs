using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Pages.Shared;
using ge_repository.Extensions;
using System.Security.Claims;

namespace ge_repository.Pages.Shared
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class MessageModel :  _geBasePageModel
    {
       public ge_event log {set;get;}
       public ge_messages msg {set;get;}
        public MessageModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager)
            : base(context, authorizationService, userManager)
        {
           msg = new ge_messages();
        }
        public async Task<IActionResult> OnGetAsync(Guid? Id, int? msgId, string Context)
        {
            if (ge_EventExists(Id)) {
                log = _context.ge_event.FirstOrDefault(e => e.Id == Id.Value);
            return Page();   
            }

            if (msgId!=null) {
                string s1 = msg[msgId.Value];  
                var claim = HttpContext.User.Claims.First(c => c.Type == "preferred_username");
                var emailAddress = claim.Value;
                string user_name= User.Identity.Name;
                ViewData ["Msg"] = personaliseMsg(s1,user_name); 
                if (Context!=null) {
                    ViewData ["Context"] = Context;
                }
            return Page();  
            }

            return NotFound();
        }

         private bool ge_EventExists(Guid? Id)
        {
            if (Id == null) {
                return false;
            } else {
                return _context.ge_event.Any(e => e.Id == Id.Value);
            }
        }
        private string personaliseMsg(string msg, string CurrentUser) {
           string s1 = msg.Replace("[CurrentUser]",CurrentUser);
           return s1;
        }
    }
}
