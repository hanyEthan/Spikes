using System;
using System.Collections.Generic;
using System.Text;
using XCore.Services.Organizations.API.Models.City;
using XCore.Services.Organizations.API.Models.Venue;

namespace XCore.Services.Organizations.API.Models.VenueCity
{
   public class VenueCityDTO
    {
        public virtual string Code { get; set; }
        public int Id { get; set; }
        public int VenueId { get; set; }
        public int CityId { get; set; }
        public virtual string Name { get; set; }
        public virtual string NameCultured { get; set; }
        public virtual bool? IsActive { get; set; }
        public virtual string CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string ModifiedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual string MetaData { get; set; }
        public VenueDTO VenueDTO { get; set; }
        public CityDTO CityDTO { get; set; }




    }
}
