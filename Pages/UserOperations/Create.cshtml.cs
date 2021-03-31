using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;

using ge_repository.Models;
using ge_repository.Pages.Shared;
using ge_repository.Authorization;
using ge_repository.Extensions;

namespace ge_repository.Pages.UserOperations
{
       public class CreateModel :  _UserOperationsPageModel
        {

        [Display(Name = "Password")] 

        [BindProperty] public string new_user_password {get;set;} 
        [Display(Name = "Find User")] [BindProperty] public string search_user {get;set;}
      //  [BindProperty] public string create_userId {get;set;}
        private readonly IEmailSender _emailSender;
        public CreateModel(
            ge_DbContext context,
            IAuthorizationService authorizationService,
            UserManager<ge_user> userManager,
            IEmailSender emailSender) : base(context, authorizationService, userManager)
        {
            _emailSender = emailSender;
        }
        
        public async Task<IActionResult> OnGetAsync(Guid? groupId, Guid? projectId)
        {
                var CurrentUserId = GetUserIdAsync().Result;
                
                user = new ge_user();
               
                user_ops = new ge_user_ops();
       
                if (groupId != null) {
                    user_ops.group =  _context.ge_group                                        
                                        .Where(o=>o.Id==groupId).FirstOrDefault();
                    if (user_ops.group!=null) {
                        bool IsUserGroupAdmin = _context.DoesUserHaveOperation(Constants.AdminOperationName, user_ops.group, CurrentUserId);
                            if  (!IsUserGroupAdmin) {
                            return RedirectToPageMessage(msgCODE.GROUP_OPERATION_CREATE_ADMINREQ); 
                            }
                        user_ops.groupId = user_ops.group.Id;
                        setViewData();                      
                        return Page(); 
                    }
                    if (projectId != null) {
                            return RedirectToPageMessage(msgCODE.USER_OPS_CREATE_AMBIGUOUS); 
                    }
                }

                if (projectId !=null) { 
                    user_ops.project = _context.ge_project
                                           .Where(p=>p.Id==projectId).FirstOrDefault();
                    if (user_ops.project!=null) {
                        bool IsUserProjectAdmin = _context.DoesUserHaveOperation(Constants.AdminOperationName, user_ops.project, CurrentUserId);
                            if  (!IsUserProjectAdmin) {
                            return RedirectToPageMessage(msgCODE.PROJECT_OPERATION_CREATE_ADMINREQ); 
                            }
                        user_ops.projectId = user_ops.project.Id;
                        setViewData();                      
                        return Page();         
                    }
                }

                return NotFound();      

                   
        }
 

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var CurrentUserId = GetUserIdAsync().Result;
            
                      
            if (user_ops.groupId!=null) {
                    user_ops.group =  _context.ge_group                                        
                                        .Where(o=>o.Id==user_ops.groupId).FirstOrDefault();
                    if (user_ops.group!=null) {
                        bool IsUserGroupAdmin = _context.DoesUserHaveOperation(Constants.AdminOperationName, user_ops.group, CurrentUserId);
                        if (!IsUserGroupAdmin) {
                            return RedirectToPageMessage(msgCODE.GROUP_OPERATION_CREATE_ADMINREQ); 
                        }
                    }
            }
            if (user_ops.projectId!=null) {
                    user_ops.project = _context.ge_project
                                            .Where(p=>p.Id==user_ops.projectId).FirstOrDefault();
                    if (user_ops.project!=null) {
                            bool IsUserProjectAdmin = _context.DoesUserHaveOperation(Constants.AdminOperationName, user_ops.project, CurrentUserId);
                            if (!IsUserProjectAdmin){
                                return RedirectToPageMessage(msgCODE.PROJECT_OPERATION_CREATE_ADMINREQ); 
                            }
                    }
                    
            }
                       
                       
            if (!String.IsNullOrEmpty(search_user) && String.IsNullOrEmpty(user.Id)) {
                ViewData["create_userId"] = _context.getUsers(search_user);
                setViewData();
                return Page();
            }
                      
            var createUserId= await ensureUser();

            if (createUserId==null) {
                return RedirectToPageMessage(msgCODE.USER_CREATE_NOTFOUND); 
            }

            user_ops.userId = createUserId;
            user_ops.createdId = CurrentUserId;
            user_ops.createdDT= DateTime.UtcNow;
            
            _context.ge_user_ops.Add(user_ops);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
        
        private async Task<string> ensureUser()
        {
            
            var u = await _userManager.FindByIdAsync(user.Id);

            if (u == null) {
                if (String.IsNullOrEmpty(user.FirstName) || 
                    String.IsNullOrEmpty(user.LastName) || 
                    String.IsNullOrEmpty(user.Email) ||
                    String.IsNullOrEmpty(user.PhoneNumber) ||
                    String.IsNullOrEmpty(new_user_password)
                    ) {
                    return null;
                } 

                u = new ge_user(user.FirstName, 
                                user.LastName,
                                user.Email,
                                user.PhoneNumber);

                var ir = await _userManager.CreateAsync(u,  new_user_password);
                
                if (ir!=IdentityResult.Success){
                    return null;
                }

                // var code = await _userManager.GenerateEmailConfirmationTokenAsync(u);
                //     var callbackUrl = Url.Page(
                //         "/Account/ConfirmEmail",
                //         pageHandler: null,
                //         values: new { userId = u.Id, code = code },
                //         protocol: Request.Scheme);

                //     await _emailSender.SendEmailAsync(u.Email, "GE Repository Confirm your email",
                //         $"<p>An account in the Ground Engineering Repository has been created for you. </p>" + 
                //         $"<p>Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a></p>" +
                //         $"<p>Your temporary password is [{new_user_password}] please change this when you first login.</p>"            
                //         );
            }
                        
            return u.Id;
        }
    static  private string GeneratePassword()
    {
  
    bool requireNonLetterOrDigit =true;
    bool requireDigit = true;
    bool requireLowercase = true;
    bool requireUppercase = true;
    int passwordLength = 8;
    
    string randomPassword = string.Empty;

    

    Random random = new Random();
    while (randomPassword.Length != passwordLength)
    {
        int randomNumber = random.Next(48, 122);  // >= 48 && < 122 
        if (randomNumber == 95 || randomNumber == 96) continue;  // != 95, 96 _'

        char c = Convert.ToChar(randomNumber);

        if (requireDigit)
            if (char.IsDigit(c))
                requireDigit = false;

        if (requireLowercase)
            if (char.IsLower(c))
                requireLowercase = false;

        if (requireUppercase)
            if (char.IsUpper(c))
                requireUppercase = false;

        if (requireNonLetterOrDigit)
            if (!char.IsLetterOrDigit(c))
                requireNonLetterOrDigit = false;

        randomPassword += c;
    }

    if (requireDigit)
        randomPassword += Convert.ToChar(random.Next(48, 58));  // 0-9

    if (requireLowercase)
        randomPassword += Convert.ToChar(random.Next(97, 123));  // a-z

    if (requireUppercase)
        randomPassword += Convert.ToChar(random.Next(65, 91));  // A-Z

    if (requireNonLetterOrDigit)
        randomPassword += Convert.ToChar(random.Next(33, 48));  // symbols !"#$%&'()*+,-./

    return randomPassword;
    }

    }
}