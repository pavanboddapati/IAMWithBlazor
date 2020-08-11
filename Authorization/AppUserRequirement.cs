using System;
using Microsoft.AspNetCore.Authorization;

namespace BlazorSvrAppWithClaims.Authorization
{
    public class AppUserRequirement : IAuthorizationRequirement
    {
        public AppUserRequirement(string claimType)
        {
            ClaimType = claimType;
        }

        public string ClaimType { get; }
    }
}
