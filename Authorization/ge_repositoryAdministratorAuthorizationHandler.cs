using System.Threading.Tasks;
using ge_repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace ge_repository.Authorization
{
    public class ge_repositoryAdministratorAuthorizationHandler
                    : AuthorizationHandler<OperationAuthorizationRequirement, _ge_base>
    {
        protected override Task HandleRequirementAsync(
                                              AuthorizationHandlerContext context,
                                    OperationAuthorizationRequirement requirement, 
                                     _ge_base resource)
        {
            if (context.User == null)
            {
                return Task.FromResult(0);
            }

            // Administrators can do anything.
            if (context.User.IsInRole(Constants.ge_repositoryAdministratorRole))
            {
                context.Succeed(requirement);
            }

            return Task.FromResult(0);
        }
    }
}
