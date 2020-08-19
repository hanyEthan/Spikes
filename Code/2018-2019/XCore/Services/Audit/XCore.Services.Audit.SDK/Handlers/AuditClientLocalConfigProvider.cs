using System;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Services.Audit.SDK.Models;

namespace XCore.Services.Audit.SDK.Handlers
{
    public class AuditClientLocalConfigProvider : IConfigProvider<AuditClientConfig>
    {
        public bool Initialized => throw new NotImplementedException();

        public async Task<AuditClientConfig> GetConfigAsync()
        {
            throw new NotImplementedException();
        }
    }
}
