using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorSvrAppWithClaims.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using BlazorSvrAppWithClaims.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using BlazorSvrAppWithClaims.Services;

namespace BlazorSvrAppWithClaims
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            var apiUrl = new Uri("https://localhost:44379");
            services.AddRegisteredHttpClient<IApplicationAPIProxy, ApplicationAPIProxy>(apiUrl);

            services.AddScoped<AppAuthorizationMiddleware>();
            services.AddScoped<AuthorizationContext>();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = "AppCookie";
                options.LoginPath = "/Authorization/Login";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                options.AccessDeniedPath = "/Unauthorized";
            });
            services.AddScoped<IAuthorizationHandler, AdminUserRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, AppUserRequirementHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("FullAdminPolicy", policyBuilder =>
                {
                    policyBuilder.AddRequirements(new AdminUserRequirement(ClaimTypes.Role, "Group1" ));
                });
                options.AddPolicy("ReadOnlyAdminPolicy", policyBuilder =>
                {
                    policyBuilder.AddRequirements(new AdminUserRequirement(ClaimTypes.Role, "Group3", "Group5" ));
                });
                options.AddPolicy("DOBRequiredPolicy", policyBuilder =>
                {
                    policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
                });

                options.AddPolicy("NormalUserPolicy", policyBuilder =>
                {
                    policyBuilder.AddRequirements(new AppUserRequirement("AppClaims.AppId"));
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAppAuthorization();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}"
                );
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");

            });
        }
    }
}
