using System;
using Microsoft.AspNetCore.Authorization;

namespace BlazorSvrAppWithClaims.Authorization
{
    public class AdminUserRequirement : IAuthorizationRequirement
    {
        public AdminUserRequirement(string claimType, params string[] valuesToCheck)
        {
            ClaimType = claimType;
            ValuesToCheck = valuesToCheck;
        }

        public string ClaimType { get; }
        public string[] ValuesToCheck { get; }
    }
}
