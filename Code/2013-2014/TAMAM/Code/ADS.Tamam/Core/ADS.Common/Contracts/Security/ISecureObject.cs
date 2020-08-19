using System;

namespace ADS.Common.Contracts.Security
{
    public interface ISecureObject
    {
        Guid Id { get; set; }
        string Code { get; set; }
    }
}
