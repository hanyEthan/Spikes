using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Caching.Custom.Handlers;

namespace Cache.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            var cache = new CacheHandler();

            // ...
            var cacheKey_01 = "codes_01";
            var cached_01 = cache.Get<List<string>>(CacheClusters.Common, cacheKey_01);

            // ...
            var codes = new List<string>() { "code.01", "code.02" };
            cache.Add(CacheClusters.Common, cacheKey_01, codes);
            var cached_02 = cache.Get<List<string>>(CacheClusters.Common, cacheKey_01);

            // ...
            var cacheKey_02 = "personnel_01";
            var personnel = new List<string>() { "person.01", "person.02" };
            cache.Add(Cache.Tester.CacheClusters.Person, cacheKey_02, personnel);
            var cached_03 = cache.Get<List<string>>(CacheClusters.Person, cacheKey_02);

            // ...
            cache.Invalidate(Cache.Tester.CacheClusters.Common); // to invalidate common cluster, and all dependent clusters, like personnel cluster.

            // ...
            var cached_04 = cache.Get<List<string>>(CacheClusters.Common, cacheKey_01);
            var cached_05 = cache.Get<List<string>>(CacheClusters.Person, cacheKey_02);
        }
    }
}
