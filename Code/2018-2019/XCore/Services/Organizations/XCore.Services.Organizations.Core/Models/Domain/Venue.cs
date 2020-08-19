using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.Core.Models.Domain
{
    public class Venue : Entity<int>
    {
        #region props.

        public string Description { get; set; }

        public int? EventId { get; set; }
        public Event Event { get; set; }

        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public int? ParentVenueId { get; set; }
        public virtual Venue ParentVenue { get; set; }
        public virtual IList<Venue> SubVenues{ get; set; }

        public virtual IList<VenueCity> Cities { get; set; }
        public virtual IList<VenueDepartment> Departments { get; set; }

        #endregion
        #region cst.

        public Venue()
        {
            Cities = new List<VenueCity>();
            Departments = new List<VenueDepartment>();
            SubVenues = new List<Venue>();
        }

        #endregion
    }
}
