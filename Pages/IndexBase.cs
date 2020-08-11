using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorSvrAppWithClaims.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorSvrAppWithClaims.Pages
{
    public class IndexBase : ComponentBase
    {

        [Inject]
        public AuthorizationContext CurrentUserContext { get; set; }

        public List<Claim> UserClaims { get; set; }
        public string UnAuthorizedMessage { get; set; }
        public bool IsPartOfGroup1Role { get { return CurrentUserContext.HasGroup1Role(); } }

        protected async override Task OnInitializedAsync()
        {
            if (CurrentUserContext.CurrentUser.Identity.IsAuthenticated)
            {
                UserClaims = CurrentUserContext.CurrentUser.Claims?.ToList();
            }
            else
            {
                UnAuthorizedMessage = "Sorry, you are not authorized to access the resource";
            }
        }
    }
}
