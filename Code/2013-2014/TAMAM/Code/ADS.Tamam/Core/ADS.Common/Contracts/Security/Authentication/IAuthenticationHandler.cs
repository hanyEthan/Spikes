using System.Collections.Generic;
using ADS.Common.Models.Domain;

namespace ADS.Common.Contracts.Security.Authentication
{
    public interface IAuthenticationProvider : IBaseHandler
    {
        string Mode { get; }
        bool Authenticate( IIdentity identity );
    }
}
