using XCore.Services.Personnel.Core.Constants;
using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Services.Configurations.SDK.Contracts;

namespace XCore.Services.Personnel.Core.Support.Config
{
    public class PersonnelServiceRemoteConfigProvider : RemoteConfigProvider<PersonnelServiceConfig>
    {
        #region cst.

        public PersonnelServiceRemoteConfigProvider(IConfigClient configClient) : base(configClient, ConfigConstants.PersonnelConfigKey)
        {
        }

        #endregion
    }
}
