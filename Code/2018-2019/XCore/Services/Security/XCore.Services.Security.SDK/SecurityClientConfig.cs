using System;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Services.Security.SDK.Contracts;

namespace XCore.Services.Security.SDK
{
    public class SecurityClientConfig : IConfigData
    {
        public string Endpoint { get; set; }
    }
}
