using Mcs.Invoicing.Core.Framework.Infrastructure.Security.IOC;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mcs.Invoicing.Services.Config.Api.IOC.Security
{
    public static class SecurityStartupHelpers
    {
        public static AuthenticationBuilder AddAuthenticationSupport(this IServiceCollection services, IConfiguration configuration)
        {
            return services.HandleCustomAuthentication();
        }
        public static IServiceCollection AddAuthorizationSupport(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Internal", policy => policy.RequireRole("system"));

                //options.AddPolicy("Role.00", policy => policy.RequireRole("Role.00", "system"));
                //options.AddPolicy("Claim.00", policy => policy.RequireClaim("client_Claim_01", "Claim_01_Value"));
            });

            return services;
        }
    }
}
