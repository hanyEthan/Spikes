using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace XCore.Services.Audit.Test.Integration
{
    public static class AuditClient
    {
        #region props.

        public static readonly HttpClient HttpClient;

        #endregion
        #region cst.

        static AuditClient()
        {
            var server = new TestServer(new WebHostBuilder()
                        .UseEnvironment("Development")
                        .UseStartup<XCore.Services.Audit.API.Startup>());

            HttpClient = server.CreateClient();
        }

        #endregion
    }
}
