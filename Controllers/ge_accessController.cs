using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using ge_repository.Models;
using ge_repository.Authorization;
using ge_repository.Extensions;
using System.Xml.Serialization;

namespace ge_repository.Controllers
{

    public class ge_accessController: ge_ControllerBase  {     
    
     public ge_accessController(

            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager, 
            IHostingEnvironment env,
		 	IOptions<ge_config> ge_config)
            : base(context, authorizationService, userManager, env, ge_config)
        {
            
        }
        
  public async Task<IActionResult> AssertOperations (ge_data data, string[] operations, string format="redirect") {
    
        try {

                var user = await GetUserAsync2();
            
                    if (user == null) {
                    return BadRequest();
                    }

                    if (operations.Contains("Download")) {
                        
                        int IsDownloadAllowed = _context.IsOperationAllowed(Constants.DownloadOperationName, data.project, data);
                        if (IsDownloadAllowed!=geOPSResp.Allowed) {
                            if (format=="redirect") return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_PROHIBITED);
                            return Unauthorized();
                        }

                        Boolean CanUserDownload = _context.DoesUserHaveOperation(Constants.DownloadOperationName, data.project, user.Id);
                        if (!CanUserDownload) {
                            if (format=="redirect") return RedirectToPageMessage (msgCODE.DATA_DOWNLOAD_USER_PROHIBITED);
                            return Unauthorized();
                        }
                    }
                   
                    if (operations.Contains("Create")) {
                        
                        int IsCreateAllowed = _context.IsOperationAllowed(Constants.CreateOperationName, data.project, data);
                        if (IsCreateAllowed!=geOPSResp.Allowed) {
                            if (format=="redirect") return RedirectToPageMessage (msgCODE.DATA_CREATE_PROHIBITED);
                            return Unauthorized(); 
                        }

                        Boolean CanUserCreate = _context.DoesUserHaveOperation(Constants.CreateOperationName, data.project,user.Id);
                        if (!CanUserCreate) {
                            if (format=="redirect") return RedirectToPageMessage (msgCODE.DATA_CREATE_USER_PROHIBITED);
                            return Unauthorized();
                        }
                    }

                    
                return Ok();

            } catch (Exception e) {
            Console.WriteLine (e.Message );
            return BadRequest();
       }

    } 

    public async Task<ge_user>  GetUserAsync2() {
                var claim = HttpContext.User.Claims.First(c => c.Type == "email");
                if (claim!=null) {
                    string emailAddress = claim.Value;
                    var user = await _userManager.FindByEmailAsync(emailAddress);
                    return user;
                }
                return null;
    }
    }
}

