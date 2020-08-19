using Mcs.Invoicing.Core.Framework.Persistence.Models;

namespace Mcs.Invoicing.Services.Config.Domain.Entities
{
    public class ConfigItem : IdentitifiedEntity<int>
    {
        #region props.

        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        public int ModuleId { get; set; }
        public Module Module { get; set; }

        #endregion
    }
}
