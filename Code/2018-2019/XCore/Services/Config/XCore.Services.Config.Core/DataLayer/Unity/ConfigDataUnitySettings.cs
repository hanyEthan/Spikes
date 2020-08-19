using XCore.Services.Config.Core.DataLayer.Contracts;

namespace XCore.Services.Config.Core.DataLayer.Unity
{
    public class ConfigDataUnitySettings : IConfigDataUnitySettings
    {
        #region IConfigDataUnitySettings

        public virtual string DBConnectionName { get; set; } = "XCore.Configuration";

        #endregion
    }
}
