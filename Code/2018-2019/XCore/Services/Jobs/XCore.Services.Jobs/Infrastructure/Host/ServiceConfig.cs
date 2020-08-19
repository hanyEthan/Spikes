using XCore.Framework.Utilities;
using XCore.Services.Jobs.Infrastructure.Models.Config;
using XCore.Services.Jobs.Infrastructure.Models.Misc;

namespace XCore.Services.Jobs.Infrastructure.Host
{
    public static class ServiceConfig
    {
        #region props.

        private static XConfig _XConfig { get; set; }

        public static ConfigSettings ServiceSettings
        {
            get
            {
                return _XConfig.GetSection<ConfigSettings>(Constants.Config.ServiceSettings);
            }
        }

        #endregion
        #region cst.

        static ServiceConfig()
        {
            _XConfig = new XConfig();
        }

        #endregion
    }
}
