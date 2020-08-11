using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BlazorSvrAppWithClaims.Authorization
{
    public class AppUserRequirementHandler : AuthorizationHandler<AppUserRequirement, int[]>
    {
        public AppUserRequirementHandler()
        {
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AppUserRequirement requirement, int[] appIds)
        {
            if (!context.User.HasClaim(clm => clm.Type == requirement.ClaimType))
                return Task.CompletedTask;

            var appUserClaims = context.User.Claims.Where(clm => clm.Type == requirement.ClaimType).Select(clm => clm.Value);

            if (appUserClaims.Any(clm => appIds.Count(vl => vl.ToString() == clm) > 0))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
