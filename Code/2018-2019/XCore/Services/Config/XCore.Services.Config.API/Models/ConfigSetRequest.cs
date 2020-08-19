using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Config.Core.Models.Support;

namespace XCore.Services.Config.API.Models
{
   public class ConfigSetRequest
    {
        public ConfigKey key { get; set; }
        public string value { get; set; }
    }
}
