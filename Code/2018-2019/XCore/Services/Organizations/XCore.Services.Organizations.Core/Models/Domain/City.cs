using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.Core.Models.Domain
{
    public  class City : Entity<int>
    {
        #region props.

        public virtual IList<VenueCity> Venues { get; set; }

        #endregion
        #region cst.

        public City()
        {
            Venues = new List<VenueCity>();
        }
        
        #endregion
    }
}
