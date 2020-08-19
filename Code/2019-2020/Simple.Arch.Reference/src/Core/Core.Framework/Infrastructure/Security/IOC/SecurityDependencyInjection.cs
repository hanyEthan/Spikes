using Mcs.Invoicing.Core.Framework.Infrastructure.Security.Authentication.Token;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Mcs.Invoicing.Core.Framework.Infrastructure.Security.IOC
{
    public static class SecurityDependencyInjection
    {
        public static AuthenticationBuilder HandleCustomAuthentication(this IServiceCollection services)
        {
            return services.AddAuthentication(SecurityCustomConstants.AuthenticationScheme)
                           .AddScheme<TokenAuthenticationOptions, TokenAuthenticationHandler>(SecurityCustomConstants.AuthenticationScheme, options => { });
        }
    }
}
