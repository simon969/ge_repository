using System.Threading.Tasks;
using ge_repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace ge_repository.Authorization
{
    public class ge_repositoryManagerAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, _ge_base>
    {
        protected override Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement,
                                   _ge_base resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.FromResult(0);
            }

            // If not asking for approval/reject, return.
            if (requirement.Name != Constants.ApproveOperationName &&
                requirement.Name != Constants.RejectOperationName)
            {
                return Task.FromResult(0);
            }

            // Managers can approve or reject.
            if (context.User.IsInRole(Constants.ge_repositoryManagerRole))
            {
                context.Succeed(requirement);
            }

            return Task.FromResult(0);
        }
    }
}
