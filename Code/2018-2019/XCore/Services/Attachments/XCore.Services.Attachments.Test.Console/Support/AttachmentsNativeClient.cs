using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Utilities;
using XCore.Services.Attachments.Core.Contracts;
using XCore.Services.Attachments.SDK.Contracts;

namespace XCore.Services.Attachments.Test.Console.Support
{
    public static class AttachmentsNativeClient
    {
        #region props.

        public static readonly HttpClient HttpClient;
        private static IServiceProvider Services { get; set; }

        public static IAttachmentsHandler BL { get { return GetInstance<IAttachmentsHandler>(); } }

       // public static IAttachmentClient Client { get { return GetInstance<IAttachmentClient>(); } }

        
        #endregion
        #region cst.

        static AttachmentsNativeClient()
        {
            var server = new TestServer(new WebHostBuilder()
                        .UseEnvironment("Development")
                        .UseStartup<XCore.Services.Attachments.API.Startup>());

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
            var response = await AttachmentsNativeClient.HttpClient.SendAsync(requestMessage);

            // ...
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            return XSerialize.JSON.Deserialize<TRres>(responseContent);
        }

        #endregion
    }
}
