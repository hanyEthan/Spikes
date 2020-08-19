using System;
using System.Linq;
using System.Threading.Tasks;
using core.Application;
using core.Infrastructure;
using core.Models.Domain;
using core.Models.Support;
using core.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace spike._01
{
    static class Program
    {
        #region ...

        private static IServiceProvider _serviceProvider { get; set; }

        private async static Task Init()
        {
            var host = CreateHostBuilder(null).Build();
            _serviceProvider = host.Services;
        }
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
               .ConfigureServices((hostContext, services) =>
               {
                   services.Startup();
               });
        }

        private static T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }
        public static IServiceCollection Startup(this IServiceCollection services)
        {
            return services.AddSingleton<DataLayer>()
                           .AddSingleton<AppHandler>()
                           .AddSingleton<MemoryCacheManager>()
                           .AddSingleton(new MyModelCacheToken())

                           .AddMemoryCache();
        }

        #endregion

        async static Task Main()
        {
            #region prep.

            await Init();
            await Seed();

            var handler = GetService<AppHandler>();

            #endregion

            var items = await handler.List();
            items = await handler.List();
            await handler.Delete(items.FirstOrDefault());
            items = await handler.List();
        }

        #region ...

        private static async Task Seed()
        {
            var handler = GetService<AppHandler>();

            for (int i = 1; i <= 5; i++)
            {
                var instance = new MyModel() { Id = i, Name = "M." + i.ToString() };
                await handler.Create(instance);
            }
        }

        #endregion
    }
}
