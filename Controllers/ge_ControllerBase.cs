using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

using ge_repository.Authorization;
using ge_repository.DAL;
using ge_repository.Models;
using ge_repository.Extensions;

namespace ge_repository.Controllers 
{
    //Base controller without view support
   public class ge_ControllerBase : ControllerBase
    {

        protected ge_DbContext _context { get; }
        protected IAuthorizationService _authorizationService { get; }
        protected UserManager<ge_user> _userManager { get; }
        protected ge_user _user {get;}
        protected IOptions<ge_config> _ge_config {get;}
		protected IHostingEnvironment _env {get;}
		
		private static string LOCAL_HOST ="https://localhost";
       
	   public ge_ControllerBase(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
			IHostingEnvironment env,
            IOptions<ge_config> ge_config) : base()
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
			_env = env;
            _ge_config = ge_config;
        } 
         public  RedirectToPageResult RedirectToPageMessage(string message, string return_URL, logLEVEL log) {
        // RedirectToAction(string actionName, string ge_message, string fragment);
            ge_eventDAL ged = new ge_eventDAL(_context);
            var user= GetUserAsync().Result;
            	ge_event ge = ged.addEvent(user.Id,message,return_URL,log);
                          ged.Save();
            return RedirectToPage ("/Shared/Message",new {Id = ge.Id});
           
        }
        public  RedirectToPageResult RedirectToPageMessage(int msgCODE) {
        // RedirectToAction(string actionName, string ge_message, string fragment);
            return RedirectToPage ("/Shared/Message",new {MsgId = msgCODE});
           
        }
        public string getHostHref() {
           string DisplayUrl = Request.GetDisplayUrl();
           string PathQuery =  Request.GetEncodedPathAndQuery();
           string HostRef = DisplayUrl.Substring(0, DisplayUrl.IndexOf(PathQuery));  
           
           
           // Check for running on local host otherwise add the application folder for Href
           if (HostRef.Contains(LOCAL_HOST)==false) {
           HostRef += "/" + _env.ApplicationName;
           }

           return HostRef;
        }

        public async Task<ge_user>  GetUserAsync() {
          //  RequestContext.Principal.Identity
                var claim = HttpContext.User.Claims.First(c => c.Type == "email");
                string emailAddress = claim.Value;
                var user = await _userManager.FindByEmailAsync(emailAddress);
                return user;
        }
    }
}

