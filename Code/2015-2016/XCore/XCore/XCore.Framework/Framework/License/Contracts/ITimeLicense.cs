using System;

namespace XCore.Framework.Framework.License.Contracts
{
    public interface ITimeLicense : ILicenseValidator
    {
        DateTime StartDate { get; set; }
        TimeSpan Period { get; set; }
    }
}
