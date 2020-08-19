using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Config.Core.Models.Domain
{
    public class Module : Entity<int>
    {
        #region props.

        public string Description { get; set; }
        public int AppId { get; set; }
        public App App { get; set; }
        public List<ConfigItem> configlist { get; set; }

        #endregion
    }
}
