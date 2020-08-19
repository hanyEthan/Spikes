using ADS.Common.Models.Domain;

namespace ADS.Common.Contracts.Security.Authentication
{
    public interface IIdentityProvider : IBaseHandler
    {
        IIdentity GetIdentity( string username );
        bool ChangePassword( IIdentity identity , string newPassword );
    }
}
