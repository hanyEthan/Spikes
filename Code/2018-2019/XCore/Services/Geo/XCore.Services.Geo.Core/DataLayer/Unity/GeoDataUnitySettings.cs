using XCore.Services.Geo.Core.DataLayer.Contracts;

namespace XCore.Framework.Infrastructure.Messaging.Queues.Repositories.Unity
{
    public class GeoDataUnitySettings : IGeoDataUnitySettings
    {
        #region IGeoDataUnitySettings

        public virtual string DBConnectionName { get; set; } = "XCore.Geo";
        //public virtual string QueueTableName { get; set; } = "MQMessage";

        #endregion
    }
}
