using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Config.Core.Models.Domain
{
    public class ConfigItem : Entity<int>
    {
        #region props.

        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool ReadOnly { get; set; }
        public string Version { get; set; }

        public int ModuleId { get; set; }
        public Module Module { get; set; }

        public int AppId { get; set; }
        public App App { get; set; }
        
        #endregion
    }
}

