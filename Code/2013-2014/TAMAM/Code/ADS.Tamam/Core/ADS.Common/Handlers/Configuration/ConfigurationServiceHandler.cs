using System;
using System.Collections.Generic;

using ADS.Common.Contracts;
using ADS.Common.Models.Domain;
using ADS.Common.Utilities;
using ADS.Common.ServiceReferences;

namespace ADS.Common.Handlers.Configuration
{
    /// <summary>
    /// configuration handler, exposing the default configuration service
    /// </summary>
    public class ConfigurationServiceHandler : IConfigurationHandler
    {
        #region Properties

        public bool Initialized { get; private set; }
        private ConfigurationServiceClient _Service;
        public IConfigurationDataHandler DataHandler { set { } }
        public string Name { get { return "ConfigurationServiceHandler"; } }

        #endregion

        #region cst.

        public ConfigurationServiceHandler()
        {
            XLogger.Info( "ConfigurationServiceHandler ..." );

            try
            {
                Initialized = InitializeService();
            }
            catch ( Exception x )
            {
                XLogger.Error( "ConfigurationServiceHandler ... Exception: " + x );
                //ExceptionHandler.Handle( x );
            }
        }

        #endregion
        #region publics

        public string GetValue( string section , string key )
        {
            return _Service.GetValue( section , key );
        }
        public List<string> GetValues( string section , string partialKey )
        {
            return new List<string>( _Service.GetValues( section , partialKey ) );
        }
        public List<ConfigurationItem> GetConfigurationItems(string section, string partialKey)
        {
            throw new NotSupportedException("GetConfigurationItems is not supported !");
        }

        public bool SetValue( string section , string key , string value )
        {
            return _Service.SetValue( section , key , value );
        }

        #endregion
        #region Helpers

        private bool InitializeService()
        {
            try
            {
                _Service = new ConfigurationServiceClient();
                return _Service.IsInitialized();
            }
            catch ( Exception x )
            {
                XLogger.Error( "ConfigurationServiceHandler.InitializeService ... Exception: " + x );
                return false;
            }
        }
        
        #endregion


        
    }
}
