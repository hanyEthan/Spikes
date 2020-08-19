using System;

namespace XCore.Framework.Framework.License.Contracts
{
    public interface ILicenseHandler
    {
        bool Initialized { get; }

        bool Validate(Guid featureId);
        bool Validate(Guid featureId, int count);
        bool IsValid { get; }

        event EventHandler<EventArgs> LicenseExpired;
    }
}
