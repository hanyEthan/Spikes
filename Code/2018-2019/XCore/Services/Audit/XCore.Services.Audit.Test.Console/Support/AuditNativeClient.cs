using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Utilities;
using XCore.Services.Audit.Core.Contracts;
using XCore.Services.Audit.SDK.Models;

namespace XCore.Services.Audit.Test.Console.Support
{
    public static class AuditNativeClient
    {
        #region props.

        public static readonly HttpClient HttpClient;
        private static IServiceProvider Services { get; set; }

        public static IAuditHandler AuditHandler { get { return GetInstance<IAuditHandler>(); } }

        #endregion
        #region cst.

        static AuditNativeClient()
        {
            var server = new TestServer(new WebHostBuilder()
                        .UseEnvironment("Development")
                        .UseStartup<XCore.Services.Audit.API.Startup>());

            HttpClient = server.CreateClient();
            Services = server.Services;
        }

        #endregion
        #region publics

        public static T GetInstance<T>()
        {
            return (T) Services.GetService(typeof(T));
        }
        public static async Task<TRres> RestCallAsync<TRreq, TRres>(string method, string url, TRreq request)
        {
            // ...
            var requestMessage = new HttpRequestMessage(new HttpMethod(method), url);
            requestMessage.Content = new StringContent(
                                         content: XSerialize.JSON.Serialize(request),
                                         encoding: Encoding.UTF8,
                                         mediaType: "application/json");

            // ...
            var response = await AuditNativeClient.HttpClient.SendAsync(requestMessage);

            // ...
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            return XSerialize.JSON.Deserialize<TRres>(responseContent);
        }

        public static async Task<ServiceExecutionResponseDTO<SearchResultsDTO<AuditTrailDTO>>> AuditGet(ServiceExecutionRequestDTO<AuditSearchCriteriaDTO> request)
        {
            return await AuditNativeClient.RestCallAsync<ServiceExecutionRequestDTO<AuditSearchCriteriaDTO>, ServiceExecutionResponseDTO<SearchResultsDTO<AuditTrailDTO>>>("POST", "/api/0.1/Audit/", request);
        }

        #endregion
    }
}
