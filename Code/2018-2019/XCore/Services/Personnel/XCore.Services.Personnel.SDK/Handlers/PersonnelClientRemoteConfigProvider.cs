using XCore.Services.Personnel.SDK.Models;
using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Services.Configurations.SDK.Contracts;

namespace XCore.Services.Personnel.SDK.Handlers
{
    public class PersonnelClientRemoteConfigProvider : RemoteConfigProvider<PersonnelClientConfig>
    {
        #region cst.

        public PersonnelClientRemoteConfigProvider(IConfigClient configClient) : base(configClient, Constants.PersonnelConfigKey)
        {
        }

        #endregion
    }
}
