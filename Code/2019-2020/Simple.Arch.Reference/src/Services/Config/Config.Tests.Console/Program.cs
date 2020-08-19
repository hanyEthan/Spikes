using System;
using System.Threading.Tasks;
using Mcs.Invoicing.Core.Framework.Infrastructure.Security.Tokens.JWT;
using Mcs.Invoicing.Services.Config.Api.gRPC.Protos;
using Mcs.Invoicing.Services.Config.Client.Sdk.Clients.Async.Models;
using Mcs.Invoicing.Services.Config.Client.Sdk.Contracts;
using Mcs.Invoicing.Services.Config.Client.Sdk.IOC;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Config.Tests.Console
{
    class Program
    {
        #region ...

        private static IServiceProvider _serviceProvider { get; set; }

        private static void Startup(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            _serviceProvider = host.Services;
        }
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddConfigSDKInfrastructure(hostContext.Configuration);
               });
        }
        private static T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        #endregion

        private static IConfigServiceClient _configClient { get; set; }

        static void Main(string[] args)
        {
            Init();
            Proto_Create();
            //Rest_Healthy();
        }

        #region Tests

        private static void Proto_Create()
        {
            var jwt = GetJwtToken();
            var instance = GetConfigMock();

            var response = Send(instance, jwt).GetAwaiter().GetResult();
        }
        private static void Rest_Healthy()
        {
            var jwt = GetJwtToken();
            var response =  _configClient.IsHealthy(jwt).GetAwaiter().GetResult();
        }

        #endregion
        #region helpers.

        private static void Init()
        {
            Startup(null);
            _configClient = GetService<IConfigServiceClient>();
        }
        private static string GetJwtToken()
        {
            // note : for SDK consumers, it's the responsibility for each component to get the correct jwt token.
            //        it could be extracted from http headers (for components that are being called through rest),
            //        or extracted from async messages (from the message header, in case of async messaging endpoints)
            //        or generated through the demonstrated utility, to generate either a vanilla basic token, or even 
            //        have the ability to create custom claims inside of that token to inject something like taxpayer id or similar custom data.

            return JWTUtilities.GenerateJwtEncodedToken(/* have the custom claims in here if any */);
        }
        private static async Task<CreateConfigItemResponseProto> Send(ConfigCreateCommandMessage instance, string jwtToken = null)
        {
            return await _configClient.CreateProto(instance, jwtToken);
        }
        private static ConfigCreateCommandMessage GetConfigMock()
        {
            return new ConfigCreateCommandMessage()
            {
                Key = "sample.key",
                Value = "sample.value",
                ModuleId = 1,
                Description = "sample.desc",
           };
        }

        #endregion
    }
}
