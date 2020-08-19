using System.Collections.Generic;

using XCore.Services.Organizations.SDK.Models.DTOs;

namespace XCore.Services.Organizations.SDK.Models.DTOs
{

    public class VenueDTO
    {
        public int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string NameCultured { get; set; }
        public virtual bool? IsActive { get; set; }
        public virtual string CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string ModifiedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual string MetaData { get; set; }
        public virtual List<CityDTO> CitiesDTO { get; set; } 
        public virtual List<DepartmentDTO> DepartmentsDTO { get; set; }
        public virtual List<VenueDTO> SubVenuesDTO { get; set; }



    }
}
