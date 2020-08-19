using System;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

using ADS.Common.Contracts.Cache;
using ADS.Common.Handlers.Cache;
using ADS.Common.Utilities;

namespace ADS.Common.Services.Cache.WCF
{
    public class CacheService : ICacheService
    {
        public void AddCluster( Guid id )
        {
            try
            {
                CacheEngine.Instance.AddCluster( id );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
            }
        }
        public void AddClusterWithParents( Guid id , List<Guid> parents )
        {
            try
            {
                CacheEngine.Instance.AddCluster( id , parents );
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
                return CacheEngine.Instance.Add( clusterId , key , value );
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
                return CacheEngine.Instance.Get( clusterId , key );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
        public bool Remove( Guid clusterId , string key )
        {
            try
            {
                return CacheEngine.Instance.Remove( clusterId , key );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public bool Contains( Guid clusterId , string key )
        {
            try
            {
                return CacheEngine.Instance.Contains( clusterId , key );
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
                return CacheEngine.Instance.Invalidate( clusterId , invalidateGraph );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public CacheServiceStats GetStatistics()
        {
            try
            {
                return new CacheServiceStats
                {
                    Health = CacheServiceStats.ServiceHealth.Healthy ,
                    TotalClusters = CacheEngine.Instance.ClusterCount() ,
                    TotalKeys = CacheEngine.Instance.KeysCount()
                };
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return null;
            }
        }
    }
}
