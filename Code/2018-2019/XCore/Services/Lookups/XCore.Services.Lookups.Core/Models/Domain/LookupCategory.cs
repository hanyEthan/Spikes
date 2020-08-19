using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Lookups.Core.Models.Domain
{
    public class LookupCategory : Entity<int>
    {
        #region props.

        public IList<Lookup> Lookups { get; set; }

        #endregion
        #region cst.

        public LookupCategory()
        {
            this.Lookups = new List<Lookup>();
        }

        #endregion
    }
}
