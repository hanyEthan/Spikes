using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Configurations.SDK.Models.Support;
using XCore.Services.Organizations.SDK.Models.DTOs;

namespace XCore.Services.Organizations.SDK.Models.Support
{
    public static class Constants
    {
        public const string OrganizationAsyncEndppoint = "XCore.Services.Organization";

        public readonly static ConfigKeyDTO OrganizationConfigKey = new ConfigKeyDTO()
        {
            App = 1,
            Module = 7,
            Key = "XCore.SDK.Organization.Config",
        };
    }
}
