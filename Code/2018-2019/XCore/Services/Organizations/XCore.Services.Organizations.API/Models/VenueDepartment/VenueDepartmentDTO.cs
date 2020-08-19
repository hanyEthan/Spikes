using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.API.Models.Department;
using XCore.Services.Organizations.API.Models.DepartmentRole;
using XCore.Services.Organizations.API.Models.Venue;

namespace XCore.Services.Organizations.API.Models.VenueDepartmentDTO
{
    public class VenueDepartmentDTO
    {
        public virtual int Id { get; set; }
        public int VenueId { get; set; }
        public int DepartmentId { get; set; }
        public virtual string Description { get; set; }
        public virtual string Code { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string MetaData { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public virtual string Name { get; set; }
        public virtual string NameCultured { get; set; }
        public VenueDTO VenueDTO { get; set; }
        public DepartmentDTO DepartmentDTO { get; set; }




    }
}
