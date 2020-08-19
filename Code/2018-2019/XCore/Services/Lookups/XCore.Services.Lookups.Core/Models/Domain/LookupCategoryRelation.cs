using System;
using System.Collections.Generic;
using System.Text;

namespace XCore.Services.Lookups.Core.Models.Domain
{
    public class LookupCategoryRelation
    {
        #region props.

        public int ParentId { get; set; }
        public LookupCategory Parent { get; set; }

        public int ChildId { get; set; }
        public LookupCategory Child { get; set; }

        #endregion
    }
}
