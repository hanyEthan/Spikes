using System.Collections.Generic;
using ADS.Common.Models.Domain;

namespace ADS.Common.Contracts
{
    public interface IConfigurationHandler : IBaseHandler
    {
        IConfigurationDataHandler DataHandler { set; }
        
        string GetValue( string section , string key );
        List<string> GetValues( string section , string partialKey );
        bool SetValue( string section , string key , string value );
    }
}
