using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Mcs.Invoicing.Core.Framework.Infrastructure.Security.Tokens.JWT;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mcs.Invoicing.Core.Framework.Infrastructure.Security.Authentication.Token
{
    public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthenticationOptions>
    {
        #region ...

        public IServiceProvider ServiceProvider { get; set; }

        public TokenAuthenticationHandler(IOptionsMonitor<TokenAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IServiceProvider serviceProvider) : base(options, logger, encoder, clock)
        {
            ServiceProvider = serviceProvider;
        }

        #endregion
        #region AuthenticationHandler

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var token = JWTUtilities.ExtractJwtToken(base.Request);
            if (token == null) return Task.FromResult(AuthenticateResult.Fail("Token is null"));

            var identity = new ClaimsIdentity(token.Claims, "AuthenticationTypes.Federation", "name", "role");
            var user = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(user, this.Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        #endregion
    }
}
