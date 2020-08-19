using System;

namespace ADS.Common.Models.Domain
{
    public interface IIdentity
    {
        Guid Id { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string ProviderName { get; set; }
    }
}
