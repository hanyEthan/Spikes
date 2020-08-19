using System;
using XCore.Caching.Custom.Contracts;

namespace XCore.Caching.Custom.Handlers
{
    public class CacheHandler : ICacheHandler
    {
        #region props ...

        private readonly CacheClient _client;
        private bool _enabled { get; set; }

        #endregion
        #region cst ...

        public CacheHandler(bool isEnabled = true)
        {
            _enabled = isEnabled;
            if (_enabled) _client = new CacheClient();
        }
        public CacheHandler(string cacheName, bool isEnabled = true) : this(isEnabled)
        {
        }

        #endregion
        #region publics ...

        public bool Add<T>(Guid clusterId, string key, T value)
        {
            if (!_enabled) return false;
            return _client.Add<T>(clusterId, key, value);
        }
        public bool Remove<T>(Guid clusterId, string key)
        {
            if (!_enabled) return false;
            return _client.Remove<T>(clusterId, key);
        }
        public T Get<T>(Guid clusterId, string key)
        {
            if (!_enabled) return default(T);
            return _client.Get<T>(clusterId, key);
        }
        public bool Contains(Guid clusterId, string key)
        {
            if (!_enabled) return false;
            return _client.Contains(clusterId, key);
        }
        public bool Invalidate(Guid clusterId, bool invalidateGraph = true)
        {
            if (!_enabled) return false;
            return _client.Invalidate(clusterId, invalidateGraph);
        }

        #endregion
    }
}
