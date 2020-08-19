using System;
using System.Linq;
using System.Collections.Generic;

using ADS.Common.Contracts;
using ADS.Common.Models.Domain;
using ADS.Common.Handlers.Data.ORM;
using ADS.Common.Utilities;

namespace ADS.Common.Handlers.Data
{
    public class ConfigurationDataHandler : IConfigurationDataHandler
    {
        #region Properties

        public bool Initialized { get; private set; }
        public string Name { get { return "ConfigurationDataHandler"; } }

        protected DomainDataContext DataContext;

        #endregion

        #region cst.

        public ConfigurationDataHandler()
        {
            try
            {
                using ( DataContext = new DomainDataContext() )
                {
                    var conn = DataContext.Connection;
                    // nothing to do ...
                }

                Initialized = true;
            }
            catch(Exception x)
            {
                XLogger.Error( "ConfigurationDataHandler.ConfigurationDataHandler ... Exception : " + x );
                Initialized = false;
            }
        }
        
        #endregion

        public string GetValue( string section , string key )
        {
            try
            {
                using ( DataContext = new DomainDataContext() )
                {
                    var item = DataContext.ConfigurationItems.Where(x => x.ApplicationId == section && x.Key == key && x.Active == true).ToList();
                    if (item != null && item.Count == 1)
                    {
                        return item[0].Value;
                    }
                    else if (item != null && item.Count > 1)
                    {
                        throw new Exception(string.Format("more than one Configuration Entry with the key {0} have been detected.", key));
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch(Exception x)
            {
                XLogger.Error( "ConfigurationDataHandler.GetValue ... Exception : " + x );
                return null;
            }
        }
        public List<string> GetValues( string section , string partialKey )
        {
            try
            {
                using ( DataContext = new DomainDataContext() )
                {
                    var items = DataContext.ConfigurationItems.Where( x => x.ApplicationId == section && x.Key.Contains( partialKey ) && x.Active == true ).Select( e => e.Value ).ToList();
                    return items;
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "ConfigurationDataHandler.GetValue ... Exception : " + x );
                return null;
            }
        }
        public bool SetValue( string section , string key , string value )
        {
            try
            {
                using ( DataContext = new DomainDataContext() )
                {
                    // should check first if it exists , if yes, update, else create

                    var item = new ConfigurationItem( Guid.NewGuid() , section , key , value , true );

                    DataContext.Add( item );
                    DataContext.SaveChanges();

                    return true;
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "ConfigurationDataHandler.GetValue ... Exception : " + x );
                return false;
            }
        }

        public List<ConfigurationItem> GetAll()
        {
            try
            {
                using ( DataContext = new DomainDataContext() )
                {
                    var items = DataContext.ConfigurationItems.ToList();
                    items = DataContext.CreateDetachedCopy( items );
                    
                    return items;
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
    }
}
