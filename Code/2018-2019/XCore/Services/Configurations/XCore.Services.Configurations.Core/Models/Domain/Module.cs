using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Configurations.Core.Models.Domain
{
    public class Module : Entity<int>
    {
        #region props.

        public string Description { get; set; }
        public int AppId { get; set; }
        public virtual App App { get; set; }
        public IList<ConfigItem> Configurations { get; set; }

        #endregion
        #region cst.

        public Module()
        {
            this.Configurations = new List<ConfigItem>();
        }

        #endregion
    }
}
