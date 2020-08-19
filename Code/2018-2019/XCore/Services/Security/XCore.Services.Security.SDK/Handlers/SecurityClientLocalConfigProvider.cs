using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Config.Contracts;

namespace XCore.Services.Security.SDK.Handlers
{
    public class SecurityClientLocalConfigProvider : IConfigProvider<SecurityClientConfig>
    {
        public bool Initialized => throw new NotImplementedException();

        public Task<SecurityClientConfig> GetConfigAsync()
        {
            throw new NotImplementedException();
        }
    }
}
