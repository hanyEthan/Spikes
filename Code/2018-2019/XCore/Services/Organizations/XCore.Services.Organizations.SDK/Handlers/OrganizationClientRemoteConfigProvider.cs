using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Services.Configurations.SDK.Contracts;
using XCore.Services.Organizations.SDK.Models.Support;

namespace XCore.Services.Organizations.SDK.Handlers
{
    public class OrganizationClientRemoteConfigProvider : RemoteConfigProvider<OrganizationClientConfig>
    {
        #region cst.

        public OrganizationClientRemoteConfigProvider(IConfigClient configClient) : base(configClient, Constants.OrganizationConfigKey)
        {
        }

        #endregion
    }
}
