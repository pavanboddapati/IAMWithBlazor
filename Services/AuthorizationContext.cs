using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorSvrAppWithClaims.Services
{
    public class AuthorizationContext
    {
        private readonly ClaimsPrincipal currentUser;

        public AuthorizationContext(AuthenticationStateProvider authenticationStateProvider)
        {
            var authState = authenticationStateProvider.GetAuthenticationStateAsync().Result;
            this.currentUser = authState.User;
        }
        public ClaimsPrincipal CurrentUser
        {
            get
            {
                return this.currentUser;
            }
        }
        public IList<Claim> UserClaims => currentUser.Claims.ToList();
        public bool HasGroup1Role() => currentUser.Claims.Any(clm => clm.Type == ClaimTypes.Role && clm.Value.Contains("Group1"));
        public bool HasGroup2Role() => currentUser.Claims.Any(clm => clm.Type == ClaimTypes.Role && clm.Value.Contains("Group2"));
        public bool HasGroup3Role() => currentUser.Claims.Any(clm => clm.Type == ClaimTypes.Role && clm.Value.Contains("Group3"));
        public bool HasGroup4Role() => currentUser.Claims.Any(clm => clm.Type == ClaimTypes.Role && clm.Value.Contains("Group4"));
        public bool HasGroup5Role() => currentUser.Claims.Any(clm => clm.Type == ClaimTypes.Role && clm.Value.Contains("Group5"));
    }
}
