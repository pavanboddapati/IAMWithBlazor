using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorSvrAppWithClaims.Data
{
    public interface IApplicationAPIProxy
    {
        Task<IEnumerable<string>> GetApplications();
        Task<string> GetApplication();
        Task<string> SaveApplication(int appId);
    }

    public class ApplicationAPIProxy : IApplicationAPIProxy
    {
        private readonly HttpClient appApiClient;
        public ApplicationAPIProxy(HttpClient httpClient)
        {
            appApiClient = httpClient;
        }

        public async Task<IEnumerable<string>> GetApplications()
        {
            return await appApiClient.GetJsonAsync<string[]>("/api/Application");
        }

        public async Task<string> GetApplication()
        {
            try
            {
                 return await appApiClient.GetJsonAsync<string>("/api/Application/AppName");
            }
            catch (UnauthorizedAccessException authEx)
            {
                return "you are unauthorized to access the api";
            }
            catch (Exception ex)
            {
                return "Not able to get data";
            }
        }

        public async Task<string> SaveApplication(int appId)
        {
            try
            {
                await appApiClient.GetAsync($"/api/Application/SaveAppName/{appId}");
                return "Success";
            }
            catch (UnauthorizedAccessException authEx)
            {
                return "you are unauthorized to access the api";
            }
            catch (Exception ex)
            {
                return "Not able to save application";
            }
        }
    }
}
