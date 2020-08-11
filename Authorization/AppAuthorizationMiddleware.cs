using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;

namespace BlazorSvrAppWithClaims.Authorization
{
    public sealed class AppAuthorizationMiddleware : IMiddleware, IDisposable
    {
        public HttpContext CurrentContext { get; set; }
        public AppAuthorizationMiddleware()
        {
        }

        public void Dispose()
        {
            //CurrentContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            CurrentContext = context;
            var applicationUserIdentity = GetApplicationUserIdentity();
            var userPrincipal = new ClaimsPrincipal(new[] { applicationUserIdentity });
            await CurrentContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);
            await next(CurrentContext);
        }

        private ClaimsIdentity GetApplicationUserIdentity()
        {
            var applicationClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "ApplicationUser1"),
                new Claim(ClaimTypes.AuthenticationMethod, "Intranet"),
                new Claim(ClaimTypes.Country, "USA"),
                new Claim(ClaimTypes.Email, "applicationuser@app.com"),
                new Claim(ClaimTypes.GivenName, "App User"),
                new Claim(ClaimTypes.NameIdentifier, "AppUser1"),
                new Claim(ClaimTypes.Role, "Group1"),
                new Claim(ClaimTypes.Role, "Group2"),
                //new Claim(ClaimTypes.Role, "Group3"),
                new Claim(ClaimTypes.Role, "Group4"),
                new Claim(ClaimTypes.Role, "Group5"),
                new Claim(ClaimTypes.Surname, "App"),
                new Claim("AppClaims.AppId", "1"),
                new Claim("AppClaims.AppId", "12"),
                new Claim("AppClaims.AppId", "14"),
                new Claim("AppClaims.AppId", "21"),
                new Claim("AppClaims.AppId", "32")

            };
            var applicationUserIdentity = new ClaimsIdentity(applicationClaims, "AppUserIdentity");
            return applicationUserIdentity;
        }
    }

    public static class AppAuthorizationMiddlewareExtension
    {
        public static IApplicationBuilder UseAppAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AppAuthorizationMiddleware>();
        }
    }
}
