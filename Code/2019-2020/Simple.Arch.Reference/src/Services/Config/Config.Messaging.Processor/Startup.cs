using Config.Messaging.Processor.IOC.AutoMapper;
using Config.Messaging.Processor.IOC.Infrastructure;
using Config.Messaging.Processor.IOC.ServiceBus;
using Config.Messaging.Processor.IOC.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Config.Messaging.Processor
{
    public class Startup
    {
        #region ...

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        #endregion

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAutoMapperSupport(this.Configuration);
            services.AddAuditInfrastructure(this.Configuration);
            services.AddAsyncCommunication(this.Configuration);

            services.AddControllersWithViews().AddValidation();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
