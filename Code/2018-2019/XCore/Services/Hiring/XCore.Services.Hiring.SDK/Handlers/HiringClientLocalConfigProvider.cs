using System;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Services.Hiring.SDK.Models;

namespace XCore.Services.Hiring.SDK.Handlers
{
    public class HiringClientLocalConfigProvider : IConfigProvider<HiringClientConfig>
    {
        public bool Initialized => throw new NotImplementedException();

        public Task<HiringClientConfig> GetConfigAsync()
        {
            throw new NotImplementedException();
        }
    }
}
