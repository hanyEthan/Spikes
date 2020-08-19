using Mcs.Invoicing.Core.Framework.Infrastructure.Context.APIs.Middleware;
using Mcs.Invoicing.Services.Config.Api.gRPC.Services;
using Mcs.Invoicing.Services.Config.Api.IOC.Cors;
using Mcs.Invoicing.Services.Config.Api.IOC.Logging;
using Mcs.Invoicing.Services.Config.Api.IOC.Mapping;
using Mcs.Invoicing.Services.Config.Api.IOC.Security;
using Mcs.Invoicing.Services.Config.Api.IOC.Swagger;
using Mcs.Invoicing.Services.Config.Api.IOC.Validation;
using Mcs.Invoicing.Services.Config.Api.IOC.Versioning;
using Mcs.Invoicing.Services.Config.Application;
using Mcs.Invoicing.Services.Config.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Config.Api
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
            services.AddApplication(this.Configuration);
            services.AddInfrastructure(this.Configuration);
            services.AddMapping(this.Configuration);
            services.AddAuthenticationSupport(this.Configuration);
            services.AddAuthorizationSupport(this.Configuration);

            services.AddVersioning(this.Configuration);
            services.AddSwaggerGen(this.Configuration);
            services.AddCors(this.Configuration);

            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddControllersWithViews()
                    .AddValidation()
                    .AddNewtonsoftJson();
            services.AddGrpc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogMiddlewareSupport();
            app.UseCustomExceptionHandler();
            app.UseCorsMiddleware();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwaggerUI();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<ConfigItemsProtoService>();
            });
        }
    }
}
