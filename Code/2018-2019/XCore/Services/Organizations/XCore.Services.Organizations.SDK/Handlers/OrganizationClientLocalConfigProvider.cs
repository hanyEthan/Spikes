using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Services.Organizations.SDK.Models.Support;

namespace XCore.Services.Organizations.SDK.Handlers
{
   public class OrganizationClientLocalConfigProvider : IConfigProvider<OrganizationClientConfig>
    {
        public bool Initialized => throw new NotImplementedException();

        public async Task<OrganizationClientConfig> GetConfigAsync()
        {
            throw new NotImplementedException();
        }
    }
}
