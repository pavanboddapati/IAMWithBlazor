using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorSvrAppWithClaims.Data;
using BlazorSvrAppWithClaims.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace BlazorSvrAppWithClaims.Pages
{
    public class FetchDataBase : ComponentBase
    {

        private int appId = 12;

        [Inject] public IApplicationAPIProxy ApplicationProxy { get; set; }
        [Inject] IAuthorizationService AuthService { get; set; }
        [Inject] AuthorizationContext CurrentUserContext { get; set; }

        protected IEnumerable<string> Applications { get; set; }
        protected string AppNameSpecific { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Applications = await ApplicationProxy.GetApplications();
            AppNameSpecific = await ApplicationProxy.GetApplication();
        }

        protected async Task SaveApplication()
        {
            var result = await ApplicationProxy.SaveApplication(appId);
        }

        protected async Task<bool> IsSaveAllowed()
        {
            var authResult = await AuthService.AuthorizeAsync(CurrentUserContext.CurrentUser, new int[] { appId }, "NormalUserPolicy");
            return authResult.Succeeded;
        }
    }
}
