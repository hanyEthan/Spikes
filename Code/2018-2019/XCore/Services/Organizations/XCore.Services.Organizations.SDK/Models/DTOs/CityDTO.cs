using System.Collections.Generic;

namespace XCore.Services.Organizations.SDK.Models.DTOs
{ 
    public class CityDTO
    {
        public virtual string Code { get; set; }
        public int Id { get; set; }
        public virtual List<VenueDTO> VenuesDTO { get; set; }
        public virtual string Name { get; set; }
        public virtual string NameCultured { get; set; }
        public virtual bool? IsActive { get; set; }
        public virtual string CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string ModifiedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual string MetaData { get; set; }






    }
}
