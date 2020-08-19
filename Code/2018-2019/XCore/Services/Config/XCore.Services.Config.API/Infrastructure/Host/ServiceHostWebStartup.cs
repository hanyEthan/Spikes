using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Nancy.Owin;

namespace XCore.Services.Config.API.Infrastructure.Host
{
    public class ServiceHostWebStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            #region compression.

            //services.AddResponseCompression(options =>
            //{
            //    options.Providers.Add<GzipCompressionProvider>();
            //    options.MimeTypes = Microsoft.AspNetCore.ResponseCompression.ResponseCompressionDefaults.MimeTypes.
            //    Concat(new string[] { "application/json" });
            //});

            //services.Configure<GzipCompressionProviderOptions>(options =>
            //{
            //    options.Level = System.IO.Compression.CompressionLevel.Fastest;
            //});

            #endregion
            #region authentication.

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        string Jwt_SigningSecret = ServiceConfig.ServiceSecuritySettings.SigningSecret;
                        var signingKey = Convert.FromBase64String(Jwt_SigningSecret);
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(signingKey)
                        };
                    });

            #endregion
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment hosting, ILoggerFactory loggerFactory)
        {
            app.UseOwin(pipeline => pipeline.UseNancy(options =>
            {
                options.Bootstrapper = new ServiceHostWebBootstrapper(app.ApplicationServices);
            }));
        }
    }
}
