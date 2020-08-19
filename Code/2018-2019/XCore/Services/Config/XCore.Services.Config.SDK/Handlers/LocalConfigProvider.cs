using System;
using Microsoft.Extensions.Configuration;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Services.Config.SDK.Models;

namespace XCore.Services.Config.SDK.Handlers
{
    public class LocalConfigProvider : IConfigProvider<ConfigClientData>
    {
        #region props.

        public bool Initialized { get; set; } = true;

        protected virtual IConfiguration Configuration { get; set; }
        public const string ConfigName = "ConfigClientData:XCore.Services.Config.Endpoint";

        #endregion
        #region cst.

        public LocalConfigProvider(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        #endregion
        #region IConfigProvider

        public ConfigClientData GetConfig()
        {
            try
            {
                var endpoint = this.Configuration.GetValue<string>(ConfigName);
                return string.IsNullOrWhiteSpace(endpoint)
                     ? null
                     : new ConfigClientData()
                     {
                         Endpoint = endpoint,
                     };

            }
            catch (Exception)
            {
                // todo : log
                throw;
            }
        }

        #endregion
    }
}
