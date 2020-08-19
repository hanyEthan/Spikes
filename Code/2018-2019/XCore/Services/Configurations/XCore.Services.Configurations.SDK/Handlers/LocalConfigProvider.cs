using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Services.Configurations.SDK.Models;

namespace XCore.Services.Configurations.SDK.Handlers
{
    public class LocalConfigProvider : IConfigProvider<ConfigClientData>
    {
        #region props.

        public virtual bool Initialized { get; set; }

        protected virtual IConfiguration Configuration { get; set; }
        public const string ConfigName = "XCore:Configuration.Service:Endpoint";

        #endregion
        #region cst.

        public LocalConfigProvider(IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.Initialized = Initialize();
        }

        #endregion
        #region IConfigProvider

        public async Task<ConfigClientData> GetConfigAsync()
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
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this.Configuration != null;
            isValid = isValid && !string.IsNullOrWhiteSpace(this.Configuration.GetValue<string>(ConfigName));

            return isValid;
        }

        #endregion
    }
}
