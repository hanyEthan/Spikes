using System;
using System.Linq;
using System.Configuration;
using System.Runtime.Caching;
using System.Collections.Generic;

using ADS.Common.Contracts.Cache;
using ADS.Common.Handlers.Cache;
using ADS.Common.Utilities;
using System.Threading.Tasks;

namespace ADS.Common.Handlers.Cache
{
    public class CacheEngine
    {
        #region Models

        public class Cluster
        {
            #region props ...

            public Guid Id { get; private set; }
            public TimeSpan? AbsoluteExpiration { get; private set; }
            public List<Cluster> Parents { get; set; }

            private readonly ObjectCache _cache;

            #endregion
            #region cst.

            public Cluster( Guid id )
            {
                this.Id = id;
                _cache = new MemoryCache( this.Id.ToString() );
                
                this.Parents = new List<Cluster>();
            }
            public Cluster( Guid id, TimeSpan absoluteExpiration ) : this( id )
            {
                this.AbsoluteExpiration = absoluteExpiration;
            }
            public Cluster( Guid id , List<Cluster> parents ) : this( id )
            {
                this.Parents = parents;
            }

            #endregion
            #region publics ...

            internal bool Add( string key , object value )
            {
                try
                {
                    var absoluteExpiration = AbsoluteExpiration.HasValue ? DateTime.UtcNow.Add( AbsoluteExpiration.Value ) : DateTime.UtcNow.AddDays( 30 );
                    _cache.Set( key, value, absoluteExpiration );
                    return true;
                }
                catch ( Exception x )
                {
                    //XLogger.Error( "Exception : " + x );
                    return false;
                }
            }
            internal bool Remove( string key )
            {
                try
                {
                    if ( this.Contains( key ) ) _cache.Remove( key );
                    return true;
                }
                catch ( Exception x )
                {
                    //XLogger.Error( "Exception : " + x );
                    return false;
                }
            }
            internal object Get( string key )
            {
                try
                {
                    return this.Contains( key ) ? _cache.Get( key ) : null;
                }
                catch ( Exception x )
                {
                    //XLogger.Error( "Exception : " + x );
                    return false;
                }
            }
            internal bool Contains( string key )
            {
                try
                {
                    return _cache.Contains( key );
                }
                catch ( Exception x )
                {
                    //XLogger.Error( "Exception : " + x );
                    return false;
                }
            }

            internal bool Invalidate( bool invalidateGraph )
            {
                return Invalidate( invalidateGraph , new List<Guid>() );
            }
            internal bool Invalidate( bool invalidateGraph , List<Guid> clustersHandled )
            {
                try
                {
                    clustersHandled = clustersHandled ?? new List<Guid>();
                    if (clustersHandled.Contains(Id))
                    {                      
                        return true;            // validate : cycle or repeated case ?
                    }
                    clustersHandled.Add( Id );                                    // flag cluster as cleaned ...

                    //if ( _cache.Any() )
                    {
                        ClearAllItems();
                        XLogger.Info(string.Format("Cache Cluster {0}: Cleared", this.Id));
                    }

                    if (invalidateGraph && Parents != null)
                    {                        
                        Parents.ToList().ForEach(x => x.Invalidate(true, clustersHandled));
                    }

                    return true;
                }
                catch ( Exception x )
                {
                    //XLogger.Error( "Exception : " + x );
                    return false;
                }
            }

            internal bool InvalidateAsync( bool invalidateGraph )
            {
                InvalidateAsync( invalidateGraph , new List<Guid>() );
                return true;
            }
            internal async void InvalidateAsync( bool invalidateGraph , List<Guid> clustersHandled )
            {
                await Task.Run( () => { Invalidate( invalidateGraph , clustersHandled ); } );
            }

            internal int KeysCount()
            {
                if ( _cache == null ) return 0;
                return _cache.Count();
            }

            #endregion
            #region helpers

            private void ClearAllItems()
            {
                _cache.ToList().ForEach( x => _cache.Remove( x.Key ) );
            }

            #endregion
        }

        #endregion

        #region props ...

        private readonly Dictionary<Guid , Cluster> _clusters;

        private static object syncRoot = new Object();
        private static volatile CacheEngine instance;
        public static CacheEngine Instance
        {
            get
            {
                if ( instance == null )
                {
                    lock ( syncRoot )
                    {
                        if ( instance == null ) instance = new CacheEngine();
                    }
                }

                return instance;
            }
        }

        #endregion
        #region cst.

        private CacheEngine()
        {
            try
            {
                _clusters = GetClusterConfigurations();
            }
            catch ( Exception x )
            {
                XLogger.Error( x.ToString() );
            }
        }

        #endregion
        #region publics

        public void AddCluster( Guid id )
        {
            try
            {
                if ( _clusters.ContainsKey( id ) ) return;

                var cluster = new Cluster( id , null );
                lock ( _clusters )
                {
                    _clusters.Add( id , cluster );
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
            }
        }
        public void AddCluster( Guid id , List<Guid> parents )
        {
            try
            {
                if ( _clusters.ContainsKey( id ) ) return;

                // ...
                var clusterParents = new List<Cluster>();
                foreach ( var cid in parents )
                {
                    clusterParents.Add( _clusters[cid] );
                }

                // ...
                var cluster = new Cluster( id , clusterParents );
                lock ( _clusters )
                {
                    _clusters.Add( id , cluster );
                }
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
            }
        }

        public bool Add( Guid clusterId , string key , object value )
        {
            try
            {
                if ( !_clusters.ContainsKey( clusterId ) ) return false;

                var cluster = _clusters[clusterId];
                return cluster.Add( key , value );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public bool Remove( Guid clusterId , string key )
        {
            try
            {
                if ( !_clusters.ContainsKey( clusterId ) ) return false;

                var cluster = _clusters[clusterId];
                return cluster.Remove( key );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public object Get( Guid clusterId , string key )
        {
            try
            {
                if ( !_clusters.ContainsKey( clusterId ) ) return null;

                var cluster = _clusters[clusterId];
                return cluster.Get( key );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
        public bool Contains( Guid clusterId , string key )
        {
            try
            {
                if ( !_clusters.ContainsKey( clusterId ) ) return false;

                var cluster = _clusters[clusterId];
                return cluster.Contains( key );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public bool Invalidate( Guid clusterId , bool invalidateGraph )
        {
            try
            {
                if ( !_clusters.ContainsKey( clusterId ) ) return false;
                var cluster = _clusters[clusterId];
                
                return cluster.InvalidateAsync( invalidateGraph );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public int ClusterCount()
        {
            try
            {
                return _clusters != null ? _clusters.Count : 0;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return 0;
            }
        }
        public int KeysCount()
        {
            try
            {
                if ( _clusters == null ) return 0;

                var count = 0;
                foreach ( var item in _clusters )
                {
                    count += item.Value.KeysCount();
                }

                return count;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return 0;
            }
        }

        #endregion
        #region Helpers

        private Dictionary<Guid , Cluster> GetClusterConfigurations()
        {
            var obj = ConfigurationManager.GetSection( "ClusterInfo/Clusters" );
            return ( obj != null && obj is Dictionary<Guid , Cluster> ) ? ( Dictionary<Guid , Cluster> ) obj : new Dictionary<Guid , Cluster>();
        }

        #endregion
    }
}
