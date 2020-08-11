using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazorSvrAppWithClaims.Controllers
{
    public class AuthorizationController : Controller
    {
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (HttpContext.User.HasClaim(clm => clm.Type == ClaimTypes.Name))
                return Ok();
            else
                return Unauthorized();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            //return new SignOutResult(new[]
            //{
            //    CookieAuthenticationDefaults.AuthenticationScheme
            //});

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}
