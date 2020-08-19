using System.Collections.Generic;
using ADS.Common.Models.Domain;

namespace ADS.Common.Contracts
{
    public interface IConfigurationDataHandler : IDataAccessHandler
    {
        string GetValue( string section , string key );
        List<string> GetValues( string section , string partialKey );
        List<ConfigurationItem> GetAll();
        bool SetValue( string section , string key , string value );
    }
}
