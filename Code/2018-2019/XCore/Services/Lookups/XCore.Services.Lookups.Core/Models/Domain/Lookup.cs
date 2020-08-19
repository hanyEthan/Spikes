using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Lookups.Core.Models.Domain
{
    public class Lookup : Entity<int>
    {
        #region props.

        public int CategoryId { get; set; }
        public LookupCategory Category { get; set; }

        #endregion
    }
}
