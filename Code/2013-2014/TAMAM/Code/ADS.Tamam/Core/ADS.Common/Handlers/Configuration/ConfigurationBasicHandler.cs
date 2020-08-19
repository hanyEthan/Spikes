using System;
using System.Collections.Generic;
using ADS.Common.Contracts;
using ADS.Common.Models.Domain;
using ADS.Common.Utilities;

namespace ADS.Common.Handlers.Configuration
{
    /// <summary>
    /// basic configuration handler, exposing the local configuration data in app.config
    /// </summary>
    public class ConfigurationBasicHandler : IConfigurationHandler
    {
        #region Properties

        public bool Initialized { get; private set; }

        private IConfigurationDataHandler _DataHandler;
        public IConfigurationDataHandler DataHandler
        {
            set
            {
                _DataHandler = value;
            }
        }
        public string Name { get { return "ConfigurationBasicHandler"; } }

        #endregion

        #region cst.

        public ConfigurationBasicHandler()
        {
        }

        #endregion

        #region publics

        public string GetValue( string section , string key )
        {
            return XConfig.GetValue( key );
        }
        public List<string> GetValues( string section , string partialKey )
        {
            throw new NotSupportedException( "ConfigurationBasicHandler is a wrapper for app.cong / web.config, thus, no support for this functionality." );
        }
        public List<ConfigurationItem> GetConfigurationItems(string section, string partialKey)
        {
            throw new NotSupportedException("ConfigurationBasicHandler is a wrapper for app.cong / web.config, thus, no support for this functionality.");
        }
        public bool SetValue( string section , string key , string value )
        {
            throw new NotSupportedException( "ConfigurationBasicHandler is a wrapper for app.cong / web.config, thus, no support for this functionality." );
        }

        #endregion


        
    }
}
