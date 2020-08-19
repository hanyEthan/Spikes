using System;
using XCore.Framework.Framework.Services.Rest;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Clients.Common.Framework.Configurations;
using XCore.Services.Configurations.SDK.Contracts;
using XCore.Services.Configurations.SDK.Models.DTOs;
using XCore.Services.Configurations.SDK.Models.Support;
using XCore.Services.Docs.SDK.Models;

namespace XCore.Services.Docs.SDK.Handlers
{
    public class DocumentClientRemoteConfigProvider : RemoteConfigProvider<DocumentClientConfig>
    {
        #region cst.

        public DocumentClientRemoteConfigProvider(IConfigClient configClient) : base(configClient, Constants.DocumentsConfigKey)
        {
        }

        #endregion
    }
}
