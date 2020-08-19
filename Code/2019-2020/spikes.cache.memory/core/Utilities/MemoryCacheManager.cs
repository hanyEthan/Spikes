using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace core.Utilities
{
    public class MemoryCacheManager
    {
        #region props.

        private static TimeSpan _absoluteExpiration = new TimeSpan(0, 1, 0);
        private static CancellationTokenSource _resetCacheToken = new CancellationTokenSource();
        private readonly Dictionary<string, Dictionary<string, string>> _clusterKeyMap = new Dictionary<string, Dictionary<string, string>>();

        private IMemoryCache _memoryCache { get; set; }

        #endregion
        #region cst.

        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            this._memoryCache = memoryCache;
        }

        #endregion
        #region publics.

        public async Task<TResult> GetOrSet<TResult>(object key, Func<Task<TResult>> dataSource)
        {
            if (_memoryCache.TryGetValue(key, out TResult cached))
            {
                return cached;
            }
            else
            {
                var data = await dataSource();
                if (data != null) await Insert(key, data);

                return data;
            }
        }

        public async Task Clear()
        {
            lock (_resetCacheToken)
            {
                if (_resetCacheToken != null && !_resetCacheToken.IsCancellationRequested && _resetCacheToken.Token.CanBeCanceled)
                {
                    _resetCacheToken.Cancel();
                    _resetCacheToken.Dispose();
                }

                _resetCacheToken = new CancellationTokenSource();
            }
        }
        public async Task Insert<TCached>(object key, TCached data)
        {
            if (data == null) throw new Exception("cached item cannot be null.");

            var options = new MemoryCacheEntryOptions()
                             .SetPriority(CacheItemPriority.Normal)
                             .SetAbsoluteExpiration(_absoluteExpiration);
            options.AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token));

            _memoryCache.Set(key, data, options);
        }

        #endregion
        #region helpers.

        private bool SetKey(string cluster, string key)
        {
            throw new NotImplementedException();

            //if (string.IsNullOrWhiteSpace(cluster)) throw new ArgumentNullException("cluster");
            //if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key");

            //Dictionary<string, string> clusterMap;

            //if (this._clusterKeyMap.TryGetValue(cluster, out Dictionary<string, string> existingClusterMap))
            //{
            //    clusterMap = existingClusterMap;
            //}
            //else
            //{
            //    clusterMap = new Dictionary<string, string>();
            //    this._clusterKeyMap.Add(cluster, clusterMap);
            //}

            //if (!clusterMap.TryGetValue(key, out string existingKey))
            //{
            //    clusterMap.Add(key, key);
            //}

            //return true;
        }

        private void ClearKey(string key)
        {
            throw new NotImplementedException();
        }

        private void ClearKeyCluster(string key)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
