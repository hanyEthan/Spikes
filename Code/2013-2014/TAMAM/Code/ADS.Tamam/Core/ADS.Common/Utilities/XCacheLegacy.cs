using System;
using System.Collections.Generic;
using System.Configuration;

using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace ADS.Common.Utilities
{
    /// <summary>
    /// in-process cache provider.
    /// </summary>
    public class XCacheLegacy
    {
        #region Internals

        public static class Settings
        {
            public static bool Enabled = true;
        }
        private static class Configuration
        {
            public static ICacheManager GetCacheManager( string cacheName )
            {
                try
                {
                    return CacheFactory.GetCacheManager( cacheName );  // created cache from config ...
                }
                catch ( Exception x )
                {
                    return CreateCacheManager( cacheName , 120 );  // created cache programmatically ...
                    //if ( _CacheManager == null ) XLogger.Error( "XCache ... Exception: " + x );
                }
            }

            private static ICacheManager CreateCacheManager( string cacheName , int expirationPollFrequencyInSeconds )
            {
                //XLogger.Info( "XCache ... trying to load configs programmatically ..." );

                try
                {
                    var settings = new CacheManagerSettings();

                    var config = new DictionaryConfigurationSource();
                    config.Add( CacheManagerSettings.SectionName , settings );

                    var storage = new CacheStorageData( cacheName + "_Storage" , typeof( NullBackingStore ) );
                    settings.BackingStores.Add( storage );

                    var cacheManagerData = new CacheManagerData( cacheName , expirationPollFrequencyInSeconds , 1000 , 5 , storage.Name );
                    settings.CacheManagers.Add( cacheManagerData );
                    settings.DefaultCacheManager = cacheManagerData.Name;
                    var factory = new CacheManagerFactory( config );

                    var cacheManager = factory.CreateDefault();
                    //if ( cacheManager != null ) XLogger.Info( "XCache ... cache creation is successful ..." );

                    return cacheManager;
                }
                catch ( Exception x )
                {
                    //XLogger.Error( "XCache.CreateCacheManager ... Exception: " + x );
                    return null;
                }
            }
            public static bool GetStatus()
            {
                try
                {
                    string enabled = ConfigurationManager.AppSettings["XCache.Enabled"];
                    if ( !string.IsNullOrEmpty( enabled ) ) bool.TryParse( enabled , out Settings.Enabled );

                    return true;
                }
                catch { return false; }
            }
        }
        private class Keys
        {
            public Keys( ICacheManager cacheManager )
            {
                _CacheManager = cacheManager;
            }

            private ICacheManager _CacheManager;

            private const string _CacheKeysKey = "<< CacheKeys >>";
            private List<string> _Keys
            {
                get
                {
                    if ( !Settings.Enabled ) return null;

                    var keys = _CacheManager.GetData( _CacheKeysKey ) as List<string>;
                    if ( keys == null )
                    {
                        keys = new List<string>();
                        _CacheManager.Add( _CacheKeysKey , keys , CacheItemPriority.Normal , null , new NeverExpired() );
                    }

                    return keys;
                }
                set { if ( Settings.Enabled ) _CacheManager.Add( _CacheKeysKey , value ); }
            }

            public void Add( string key )
            {
                if ( !_Keys.Contains( key ) ) _Keys.Add( key );
            }
            public void Remove( string key )
            {
                if ( _Keys.Contains( key ) ) _Keys.Remove( key );
            }
            public void Clear()
            {
                _Keys.Clear();
            }
        }

        #endregion
        #region Properties

        private ICacheManager _CacheManager;
        private Keys _CacheKeys;

        #endregion

        public XCacheLegacy( string cacheName )
        {
            Configuration.GetStatus();
            _CacheManager = Configuration.GetCacheManager( cacheName );
            _CacheKeys = new Keys( _CacheManager );

            Settings.Enabled = Settings.Enabled && ( _CacheManager != null );
        }

        #region publics

        public object Get( string key )
        {
            return Settings.Enabled ? _CacheManager.GetData( key ) : null;
        }
        public void Add( string key , object value )
        {
            if ( !Settings.Enabled ) return;

            _CacheManager.Add( key , value );
            _CacheKeys.Add( key );
            //XLogger.Info( "Number of Cache Entries:" + _CacheManager.Count.ToString() );
        }
        public void Remove( string key )
        {
            if ( !Settings.Enabled ) return;

            _CacheManager.Remove( key );
            _CacheKeys.Remove( key );
        }
        public void Flush()
        {
            if ( !Settings.Enabled ) return;

            _CacheManager.Flush();
            _CacheKeys.Clear();
        }

        #endregion
    }
}
