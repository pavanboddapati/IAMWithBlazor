using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazorSvrAppWithClaims.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ApplicationController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public ApplicationController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        public IActionResult Get()
        {
            IEnumerable<string> appNames = new string[] { "App1", "App2", "App3"};
            return Ok(appNames);
        }

        [Route("AppName")]
        [Authorize(Policy = "ReadOnlyAdminPolicy")]
        public IActionResult GetApp()
        {
            var appName = "App1";
            return Ok(appName);
        }

        [Route("SaveAppName/{appId}")]
        public async Task<IActionResult> SaveApp(int appId)
        {
            var isSaveAllowed = await authorizationService.AuthorizeAsync(User, new int[] { appId }, "NormalUserPolicy");

            if (isSaveAllowed.Succeeded)
            {
                //Save Changes to DB
                return Ok();
            }
            else
                return Unauthorized();

        }
    }
}
