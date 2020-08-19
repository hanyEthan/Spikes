using System.Collections.Generic;
using Mcs.Invoicing.Core.Framework.Persistence.Models;

namespace Mcs.Invoicing.Services.Config.Domain.Entities
{
    public class Module : IdentitifiedEntity<int>
    {
        #region props.

        public string Name { get; set; }
        public string NameCultured { get; set; }
        public string Description { get; set; }
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
