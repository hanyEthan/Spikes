using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mcs.Invoicing.AdminPortal.Bff.Api
{
    public static class StartupHelpers
    {




        public static IServiceCollection HandleCors(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        //.AllowCredentials()
                        .WithExposedHeaders("Content-Disposition"));
            });
        }




        public static void HandleOcelot(this IServiceCollection services, IConfiguration configuration)
        {
            IdentityModelEventSource.ShowPII = true;

            var identityUrl = configuration.GetValue<string>("IdentityUrl");
            var authenticationProviderKey = "IdentityApiKey";

            //var configManager = new ConfigurationManager<OpenIdConnectConfiguration>($"{identityUrl}/.well-known/openid-configuration", new OpenIdConnectConfigurationRetriever());
            //var openidconfig = configManager.GetConfigurationAsync().Result;

            Action<IdentityServerAuthenticationOptions> options = opt =>
            {
                opt.JwtBackChannelHandler = new System.Net.Http.HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = System.Net.Http.HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                opt.Authority = identityUrl;
                opt.SupportedTokens = SupportedTokens.Both;
                
            };

            services.AddAuthentication()
                .AddIdentityServerAuthentication(authenticationProviderKey, options);

            services.AddOcelot(configuration);
        }
        public static void UseOcelotMiddleware(this IApplicationBuilder app)
        {

            app.UseOcelot()
               .Wait();
        }



    }
}
