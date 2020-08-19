using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.Core.Models.Domain
{
    public class VenueCity
    {
        #region props.

        public int VenueId { get; set; }
        public Venue Venue { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }
        
        #endregion
    }
}
