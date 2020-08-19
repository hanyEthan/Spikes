using System;
using System.Configuration;

using ADS.Common.Contracts.Cache;

namespace ADS.Common.Handlers.Cache
{
    public class CacheHandler : ICacheHandler
    {
        #region props ...

        private readonly CacheClient _client;
        private bool Enabled { get; set; }

        #endregion
        #region cst ...

        public CacheHandler()
        {
            Enabled = GetStatusConfig();
            if ( Enabled ) _client = new CacheClient();
        }
        public CacheHandler( string cacheName ) : this()
        {
        }

        #endregion
        #region publics ...

        public bool Add<T>( Guid clusterId , string key , T value )
        {
            if ( !Enabled ) return false;
            return _client.Add<T>( clusterId , key , value );
        }
        public bool Remove<T>( Guid clusterId , string key )
        {
            if ( !Enabled ) return false;
            return _client.Remove<T>( clusterId , key );
        }
        public T Get<T>( Guid clusterId , string key )
        {
            if ( !Enabled ) return default( T );
            return _client.Get<T>( clusterId , key );
        }
        public bool Contains( Guid clusterId , string key )
        {
            if ( !Enabled ) return false;
            return _client.Contains( clusterId , key );
        }
        public bool Invalidate( Guid clusterId , bool invalidateGraph = true )
        {
            if ( !Enabled ) return false;
            return _client.Invalidate( clusterId , invalidateGraph );
        }

        #endregion
        #region Helpers

        private bool GetStatusConfig()
        {
            //string enabled = ConfigurationManager.AppSettings["XCache.Enabled"];

            string enabled = Broker.ConfigurationHandler.GetValue( Constants.CacheSection , Constants.CacheEnabled );
            return string.IsNullOrEmpty( enabled ) ? false : Convert.ToBoolean( enabled );
        }

        #endregion
    }
}
