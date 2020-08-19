using System.Collections.Generic;
using ADS.Common.Models.Domain;

namespace ADS.Common.Contracts.Security.Authentication
{
    public interface IAuthenticationService : IBaseHandler
    {
        IIdentityProvider IdentityProvider { get; set; }
        Dictionary<string , IAuthenticationProvider> AuthenticationProviders { get; set; }

        bool Authenticate( IIdentity identity );
        IIdentity GetIdentity( string username );
        bool ChangePassword( IIdentity identity , string newPassword );
    }
}
