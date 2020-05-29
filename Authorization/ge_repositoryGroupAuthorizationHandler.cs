using System.Threading.Tasks;
using ge_repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace ge_repository.Authorization
{
    public class ge_repositoryGroupAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, ge_group>
    {
        ge_DbContext _context;
        UserManager<ge_user> _userManager;
        public ge_repositoryGroupAuthorizationHandler(ge_DbContext Context, UserManager<ge_user> 
            UserManager)
        {
            _context = Context;
            _userManager = UserManager;
        }
       
       
        protected override Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement,
                                   ge_group resource)
        {
            if (context.User == null || resource == null) {
                return Task.FromResult(0);
            }
            var claim = context.User.Claims.First(c => c.Type == "email");
            string emailAddress = claim.Value;
            
            var user = _userManager.FindByEmailAsync(emailAddress);
            
            if (user==null) {
                return Task.FromResult(0);
            }

            string userId = user.Result.Id;
            
            bool userAuthorised = _context.DoesUserHaveOperation(requirement.Name,resource,userId); 
            
            if (userAuthorised==true) {
                context.Succeed(requirement);
            } else {
                context.Fail();
            }

            return Task.FromResult(0); 
        }
        
    }
}
