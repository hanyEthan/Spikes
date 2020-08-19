using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using core.Infrastructure;
using core.Models.Constants;
using core.Models.Domain;
using core.Models.Support;
using core.Utilities;

namespace core.Application
{
    public class AppHandler
    {
        #region props.

        private DataLayer _dataLayer { get; set; }
        private MemoryCacheManager _memoryCache { get; set; }

        #endregion
        #region cst.

        public AppHandler(DataLayer dataLayer,
                          MemoryCacheManager memoryCache)
        {
            this._dataLayer = dataLayer;
            this._memoryCache = memoryCache;
        }

        #endregion
        #region application.

        public async Task Create(MyModel instance)
        {
            await this._dataLayer.Create(instance);
            await this._memoryCache.Clear();
        }
        public async Task Update(MyModel instance)
        {
            await this._dataLayer.Update(instance);
            await this._memoryCache.Clear();
        }
        public async Task Delete(MyModel instance)
        {
            await this._dataLayer.Delete(instance);
            await this._memoryCache.Clear();
        }
        public async Task<MyModel> Get(int id)
        {
            var key = CachingConstants.Key_item + id;
            Func<Task<MyModel>> source = async () => await this._dataLayer.Get(id);

            return await this._memoryCache.GetOrSet(key, source);
        }
        public async Task<List<MyModel>> List()
        {
            var key = CachingConstants.Key_list;
            Func<Task<List<MyModel>>> source = async () => await this._dataLayer.List();

            return await this._memoryCache.GetOrSet(key, source);
        }

        #endregion
    }
}
