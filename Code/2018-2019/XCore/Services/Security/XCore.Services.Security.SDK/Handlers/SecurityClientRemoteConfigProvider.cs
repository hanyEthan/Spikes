using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Services.Configurations.SDK.Contracts;
using XCore.Services.Security.SDK.Contracts;
using XCore.Services.Security.SDK.Models.DTOs;
using XCore.Services.Security.SDK.Models.Support;

namespace XCore.Services.Security.SDK.Handlers
{
    public class SecurityClientRemoteConfigProvider : RemoteConfigProvider<SecurityClientConfig>
    {
        #region cst.

        public SecurityClientRemoteConfigProvider(IConfigClient configClient) : base(configClient,Constants.SecurityConfigKey)
        {

        }

        #endregion
    }
}
