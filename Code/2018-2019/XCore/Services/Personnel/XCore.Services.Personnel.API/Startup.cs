using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace XCore.Services.Personnel.API
{

    public class Startup
    {
        #region ...
        private IConfiguration Configuration { get; }
        private ILoggerFactory LoggerFactory { get; }
        private IWebHostEnvironment HostingEnvironment { get; }

        public Startup(IWebHostEnvironment env)
        {
            this.Configuration = InitializeConfig(env);
            //this.LoggerFactory = loggerFactory;
            this.HostingEnvironment = env;
        }
        private IConfiguration InitializeConfig(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                         .SetBasePath(env.ContentRootPath)
                         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                         .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                         .AddEnvironmentVariables();

            //if (env.IsDevelopment())
            //{
            //    builder.AddUserSecrets<Startup>();
            //}

            return builder.Build();
        }

        #endregion

        public void ConfigureServices(IServiceCollection services)
        {
            services.HandleConfigurations(this.Configuration);
            services.HandleXCore(this.Configuration);
            services.AddControllers();
            services.HandleVersioning(this.Configuration);
            //services.HandleHealth(this.Configuration);
            services.HandleSwaggerGen(this.Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSwaggerUI();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.HandleHealthEndpoints();
                endpoints.MapControllers();
            });
        }
    }
}
