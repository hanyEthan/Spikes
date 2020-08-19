using System;
using System.Collections.Generic;

namespace ADS.Common.Handlers.License.Contracts
{
    public interface ILicenseValidator
    {
        Guid Id { get; set; }
        List<ILicenseValidator> LicenseValidators { get; set; }

        bool IsValid();
        bool IsValid(int count);
    }
}
