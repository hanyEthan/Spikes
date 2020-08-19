using XCore.Services.Config.Core.Contracts;
using XCore.Services.Config.Core.Handlers;

namespace XCore.Services.Config.Core.Unity
{
    public static class ConfigUnity
    {
        #region props.

        public static IConfigHandlerBase Configs { get; set; }

        #endregion
        #region cst.

        static ConfigUnity()
        {
            Initialize();
        }

        private static void Initialize()
        {
            Configs = new ConfigHandler();
        }

        #endregion
    }
}
