using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using XCore.Framework.Infrastructure.Config.Contracts;

namespace XCore.Framework.Framework.ServiceBus.MST.Support
{
    public class ServiceBusLocalConfigProvider : IConfigProvider<ServiceBusConfiguration>
    {
        #region props.

        public bool Initialized { get; protected set; }

        protected virtual IConfiguration Configuration { get; set; }
        public const string ConfigName = "ServiceBus";

        #endregion
        #region cst.

        public ServiceBusLocalConfigProvider(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
            this.Initialized = Initialize();
        }

        #endregion

        #region IConfigProvider

        public async Task<ServiceBusConfiguration> GetConfigAsync()
        {
            return this.Configuration.GetSection(ConfigName).Get<ServiceBusConfiguration>();
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this.Configuration != null;

            return isValid;
        }

        #endregion
    }
}
