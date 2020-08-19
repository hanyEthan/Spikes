using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.Configuration;
using Nancy.TinyIoc;

namespace XCore.Services.Config.API.Infrastructure.Host
{
    public class ServiceHostWebBootstrapper : DefaultNancyBootstrapper
    {
        #region props.

        private readonly IServiceProvider _serviceProvider;

        #endregion
        #region cst.

        public ServiceHostWebBootstrapper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region DefaultNancyBootstrapper

        public override void Configure(INancyEnvironment environment)
        {
            environment.Tracing(true, true);
        }
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            container.Register(_serviceProvider.GetService<ILoggerFactory>());
        }
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            var configuration = new StatelessAuthenticationConfiguration(context =>
            {
                if (context.Request.Path == "/login")
                    return null;

                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var jwt = context.Request.Headers.Authorization;
                if (string.IsNullOrWhiteSpace(jwt))
                {
                    return null;
                }

                jwt = jwt.Substring("Bearer ".Length);
                JwtSecurityToken jwtToken = jwtTokenHandler.ReadJwtToken(jwt);

                if (jwtToken == null) return null;

                byte[] signingKey = Encoding.UTF8.GetBytes(ServiceConfig.ServiceSecuritySettings.SigningSecret);
                int expiryDuration = ServiceConfig.ServiceSecuritySettings.ExpiryDuration;

                var parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(signingKey)
                };

                /*
                  if (authToken.ExpirationDateTime < DateTime.UtcNow)
                    return null;
                 */
                try
                {
                    SecurityToken securityToken;
                    ClaimsPrincipal principal = jwtTokenHandler.ValidateToken(jwt, parameters, out securityToken);

                    int userid;
                    if (int.TryParse(principal?.FindFirst("userid")?.Value, out userid))
                    {
                        ///later...

                        //User appUser = UsersStore.Find(userid);
                        //CustomClaimsPrincipal appUserPrinciple = new CustomClaimsPrincipal(appUser);

                        //return appUserPrinciple;
                    }
                    return null;
                }
                catch
                {
                    return null;
                }
            });
            StatelessAuthentication.Enable(pipelines, configuration);

            base.ApplicationStartup(container, pipelines);
        }
        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            //CORS Enable
            pipelines.AfterRequest.AddItemToEndOfPipeline((ctx) =>
            {
                ctx.Response.WithHeader("Access-Control-Allow-Origin", "*")
                                .WithHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS")
                                .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type, Authorization, Cache-Control");
            });
        }

        #endregion
    }
}
