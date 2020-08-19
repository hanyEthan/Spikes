using System;
using ADS.Tamam.Modules.Integration.Helpers;

namespace ADS.Tamam.Modules.Integration.Models
{
    public interface IDetailCodeSimilar : ILoggable
    {
        string Code { get; set; }
        string Name { get; set; }
        string NameVariant { get; set; }
        bool Activated { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
    }
}