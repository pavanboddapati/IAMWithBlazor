using System;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorSvrAppWithClaims.Data
{
    public static class ApplicationServiceExtensions
    {
        public static void AddRegisteredHttpClient<TClient, TImplementation>(this IServiceCollection services, Uri apiBaseUrl)
                where TClient : class where TImplementation : class, TClient
        {
            services.AddHttpClient<TClient, TImplementation>(client =>
            {
                client.BaseAddress = apiBaseUrl;
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }
    }
}
