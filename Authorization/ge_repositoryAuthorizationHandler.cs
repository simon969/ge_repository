using System.Threading.Tasks;
using ge_repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace ge_repository.Authorization
{
    public class ge_repositoryAuthorizationHandler
                : AuthorizationHandler<OperationAuthorizationRequirement, _ge_base>
    {
        UserManager<ge_user> _userManager;

        public ge_repositoryAuthorizationHandler(UserManager<ge_user>
            userManager)
        {
            _userManager = userManager;
        }

        protected override Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement,
                                   _ge_base resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.FromResult(0);
            }

            // If we're not asking for CRUD permission, return.

            if (requirement.Name != Constants.CreateOperationName &&
                requirement.Name != Constants.ReadOperationName   &&
                requirement.Name != Constants.UpdateOperationName &&
                requirement.Name != Constants.DeleteOperationName &&
                requirement.Name != Constants.DownloadOperationName)
            {
                return Task.FromResult(0);
            }
            
            context.Succeed(requirement);
          

            return Task.FromResult(0);
        }
    }
}
