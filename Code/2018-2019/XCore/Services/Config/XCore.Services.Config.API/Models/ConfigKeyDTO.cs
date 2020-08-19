using System;
using System.Collections.Generic;
using System.Text;

namespace XCore.Services.Config.API.Models
{
   public class ConfigKeyDTO
    {
        public virtual int App { get; set; }
        public virtual int Module { get; set; }
        public virtual string Key { get; set; }
    }
}
