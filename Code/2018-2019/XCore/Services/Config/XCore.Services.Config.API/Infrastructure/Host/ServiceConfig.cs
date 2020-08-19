using XCore.Framework.Utilities;
using XCore.Services.Config.API.Infrastructure.Models.Config;
using XCore.Services.Config.API.Infrastructure.Models.Misc;

namespace XCore.Services.Config.API.Infrastructure.Host
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
        public static JWTSettings ServiceSecuritySettings
        {
            get
            {
                return _XConfig.GetSection<JWTSettings>(Constants.Config.ServiceSecuritySettings);
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
