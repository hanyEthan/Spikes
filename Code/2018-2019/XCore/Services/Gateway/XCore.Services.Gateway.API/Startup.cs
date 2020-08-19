using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;

namespace XCore.Services.Gateway.API
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
            services.HandleConfigurations(this.Configuration);
            services.HandleXCore(this.Configuration);
            services.AddControllers();
            services.HandleVersioning(this.Configuration);
            //services.HandleHealth(this.Configuration);
            services.HandleSwaggerGen(this.Configuration);
            services.HandleCors(this.Configuration);
            services.HandleOcelot(Configuration);
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
            app.UseSwaggerUI();
            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                //endpoints.HandleHealthEndpoints();
                endpoints.MapControllers();
            });
            app.UseOcelotMiddleware();
        }
    }
}
