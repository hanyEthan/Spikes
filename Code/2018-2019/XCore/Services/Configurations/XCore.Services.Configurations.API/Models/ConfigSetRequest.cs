using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Configurations.Core.Models.Support;

namespace XCore.Services.Configurations.Models
{
   public class ConfigSetRequest
    {
        public ConfigKey key { get; set; }
        public string value { get; set; }
    }
}
