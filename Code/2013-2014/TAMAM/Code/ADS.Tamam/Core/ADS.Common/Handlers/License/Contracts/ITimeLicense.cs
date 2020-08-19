using System;

namespace ADS.Common.Handlers.License.Contracts
{
    public interface ITimeLicense : ILicenseValidator
    {
        DateTime StartDate { get; set; }
        TimeSpan Period { get; set; }
    }
}
