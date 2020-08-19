using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Services.Configurations.SDK.Models.Support;

namespace XCore.Services.Organizations.API.Constants
{
    public static class ConfigConstants
    {
        public readonly static ConfigKeyDTO BusConfig = new ConfigKeyDTO()
        {
            App = 1,
            Module = 7,
            Key = "XCore.Services.Organization.Config.Async",
        };
    }
}
