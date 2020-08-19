using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ADS.Common.Contracts;
using ADS.Common.Handlers.Data;
using ADS.Common.Models.Domain;
using ADS.Common.Utilities;

namespace ADS.Common.Handlers.Configuration
{
    /// <summary>
    /// default configuration handler, exposing the configuration data in specific data store ...
    /// </summary>
    public class ConfigurationHandler : IConfigurationHandler
    {
        #region Properties

        public bool Initialized { get; private set; }

        private IConfigurationDataHandler _DataHandler;
        public IConfigurationDataHandler DataHandler
        {
            set
            {
                _DataHandler = value;
                if ( UpdateState() ) CacheInitialize();
            }
        }
        public string Name { get { return "ConfigurationHandler"; } }

        private Dictionary<string , string> ConfigurationCache;

        #endregion

        #region cst.

        public ConfigurationHandler()
        {
            XLogger.Info( "ConfigurationHandler ..." );

            try
            {
                Initialized = false; // waiting for the datastore to be set ...
                ConfigurationCache = new Dictionary<string , string>();
            }
            catch ( Exception x )
            {
                XLogger.Error( "ConfigurationHandler ... Exception: " + x );
                //ExceptionHandler.Handle( x );
            }
        }

        #endregion

        #region publics

        public string GetValue( string section , string key )
        {
            // validate input ...
            // connect to data store ...
            // handle result, and return ...

            var v = CacheGetValue( section , key );
            if ( !string.IsNullOrEmpty( v ) ) return v;

            return _DataHandler.GetValue( section , key );
        }
        public List<string> GetValues( string section , string partialKey )
        {
            var values = CacheGetValues( section , partialKey );
            if ( values != null && values.Count > 0 ) return values;

            return _DataHandler.GetValues( section , partialKey );
        }
        public bool SetValue( string section , string key , string value )
        {
            return _DataHandler.SetValue( section , key , value )
                && CacheSetValue( section , key , value );
        }

        #endregion
        #region Helpers

        private bool UpdateState()
        {
            Initialized = _DataHandler == null ? false : _DataHandler.Initialized;
            return Initialized;
        }
        private bool CacheInitialize()
        {
            try
            {
                var configs = _DataHandler.GetAll();
                foreach ( var config in configs )
                {
                    var key = config.ApplicationId + "." + config.Key;

                    if ( config.Active && !ConfigurationCache.Keys.Contains( key ) )
                    {
                        ConfigurationCache.Add( key , config.Value );
                    }
                }

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Excepion : " + x );
                return false;
            }
        }
        private string CacheGetValue( string section , string key )
        {
            try
            {
                var cachekey = section + "." + key;
                return ConfigurationCache[cachekey];
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
        private List<string> CacheGetValues( string section , string partialKey )
        {
            try
            {
                var result = new List<string>();
                var cachePartialKey = section + "." + partialKey;

                foreach ( var key in ConfigurationCache.Keys )
                {
                    if ( key.StartsWith( cachePartialKey ) ) result.Add( ConfigurationCache[key] );
                }

                return result;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Excepion : " + x );
                return null;
            }
        }
        private bool CacheSetValue( string section , string key , string value )
        {
            try
            {
                var cachekey = section + "." + key;
                ConfigurationCache[cachekey] = value;
                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Excepion : " + x );
                return false;
            }
        }
        
        #endregion
    }
}
