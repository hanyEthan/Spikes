using System;
using ADS.Common.Contracts;

namespace ADS.Common.Handlers.License.Contracts
{
    public interface ILicenseHandler : IBaseHandler
    {
        bool Validate(Guid featureId);
        bool Validate(Guid featureId, int count);
        bool IsValid { get; }

        event EventHandler<EventArgs> LicenseExpired;
    }
}
