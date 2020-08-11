using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BlazorSvrAppWithClaims.Authorization
{
    public class AdminUserRequirementHandler : AuthorizationHandler<AdminUserRequirement>
    {
        public AdminUserRequirementHandler()
        {
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminUserRequirement requirement)
        {
            if (!context.User.HasClaim(clm => clm.Type == requirement.ClaimType))
                return Task.CompletedTask;

            var adminUserClaims = context.User.Claims.Where(clm => clm.Type == requirement.ClaimType).Select(clm => clm.Value);

            if (adminUserClaims.Any(clm => requirement.ValuesToCheck.Contains(clm)))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
