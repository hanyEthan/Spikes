using System;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Mcs.Invoicing.PublicPortals.Bff.Api
{
    public static class StartupHelpers
    {
        public static IServiceCollection HandleCors(this IServiceCollection services)
        {
            return services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("Content-Disposition"));
            });
        }

        public static void HandleOcelot(this IServiceCollection services, IConfiguration configuration)
        {
            var identityUrl = configuration.GetValue<string>("IdentityUrl");

            var authenticationProviderKey = "IdentityApiKey";
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
            app.UseOcelot().Wait();
        }
    }
}
