using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Caching.Custom.Bases.Services.Proxies;
using XCore.Caching.Custom.Contracts;
using XCore.Caching.Custom.Utilities;

namespace XCore.Caching.Custom.Handlers
{
    public class CacheClient : WCFServiceProxy<ICacheService>
    {
        #region cst

        public CacheClient() : base()
        {
            try
            {
                var stats = base.Initialized ? client.GetStatistics() : null;
            }
            catch
            {
                Initialized = false;
            }
        }

        #endregion
        #region Publics

        public void AddCluster(Guid id)
        {
            try
            {
                if (!CheckStatus()) return;
                client.AddCluster(id);
            }
            catch (Exception x)
            {
                //XLogger.Error( x.ToString() );
                XLogger.Error("Cache Service : couldn't reach the caching service (connection to service is down, please check the network), skipping it ...");
            }
        }
        public void AddClusterWithParents(Guid id, List<Guid> parents)
        {
            try
            {
                if (!CheckStatus()) return;
                client.AddClusterWithParents(id, parents);
            }
            catch (Exception x)
            {
                //XLogger.Error( x.ToString() );
                XLogger.Error("Cache Service : couldn't reach the caching service (connection to service is down, please check the network), skipping it ...");
            }
        }
        public bool Add<T>(Guid clusterId, string key, T value)
        {
            try
            {
                if (!CheckStatus()) return false;

                if (value == null)
                {
                    return false;
                }
                byte[] data;
                XSerialize.Serialize(XSerialize.Mode.BinaryProtobuf, value, out data);
                return client.Add(clusterId, key, data);
            }
            catch (Exception x)
            {
                //XLogger.Error( x.ToString() );
                XLogger.Error("Cache Service : couldn't reach the caching service (connection to service is down, please check the network), skipping it ...");
                return false;
            }
        }
        public T Get<T>(Guid clusterId, string key)
        {
            try
            {
                if (!CheckStatus()) return default(T);

                var result = client.Get(clusterId, key);
                return result != null ? XSerialize.Deserialize<T>(XSerialize.Mode.BinaryProtobuf, (byte[])result) : default(T);
            }
            catch (Exception x)
            {
                //XLogger.Error( x.ToString() );
                XLogger.Error("Cache Service : couldn't reach the caching service (connection to service is down, please check the network), skipping it ...");
                return default(T);
            }
        }
        public bool Remove<T>(Guid clusterId, string key)
        {
            try
            {
                if (!CheckStatus()) return false;
                return client.Remove(clusterId, key);
            }
            catch (Exception x)
            {
                //XLogger.Error( x.ToString() );
                XLogger.Error("Cache Service : couldn't reach the caching service (connection to service is down, please check the network), skipping it ...");
                return false;
            }
        }
        public bool Contains(Guid clusterId, string key)
        {
            try
            {
                if (!CheckStatus()) return false;
                return client.Contains(clusterId, key);
            }
            catch (Exception x)
            {
                //XLogger.Error( x.ToString() );
                XLogger.Error("Cache Service : couldn't reach the caching service (connection to service is down, please check the network), skipping it ...");
                return true;
            }
        }
        public bool Invalidate(Guid clusterId, bool invalidateGraph)
        {
            try
            {
                if (!CheckStatus()) return false;
                return client.Invalidate(clusterId, invalidateGraph);
            }
            catch (Exception x)
            {
                //XLogger.Error( x.ToString() );
                XLogger.Error("Cache Service : couldn't reach the caching service (connection to service is down, please check the network), skipping it ...");
                return false;
            }
        }
        public CacheServiceStats GetStatistics()
        {
            try
            {
                if (!CheckStatus()) return new CacheServiceStats { Health = CacheServiceStats.ServiceHealth.Unkown };
                return client.GetStatistics();
            }
            catch (Exception x)
            {
                //XLogger.Error( x.ToString() );
                XLogger.Error("Cache Service : couldn't reach the caching service (connection to service is down, please check the network), skipping it ...");
                return new CacheServiceStats { Health = CacheServiceStats.ServiceHealth.Unkown };
            }
        }

        #endregion
    }
}
