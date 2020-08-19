using System;
using System.Collections.Generic;
using XCore.Services.Organizations.API.Models.Organization;
using XCore.Services.Organizations.API.Models.Role;
using XCore.Services.Organizations.API.Models.Venue;

namespace XCore.Services.Organizations.API.Models.Department
{
    public class DepartmentDTO
    {
        public virtual int Id { get; set; }
        public virtual string Description { get; set; }
        public virtual string Code { get; set; }=Guid.NewGuid().ToString();
        public virtual bool IsActive { get; set; }
        public virtual string MetaData { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public virtual string Name { get; set; }
        public virtual List<RoleDTO> RolesDTO { get; set; }
        public virtual List<VenueDTO> VenuesDTO { get; set; }
        public virtual string NameCultured { get; set; }
        public virtual int OrganizationId { get; set; } = 1;
        public virtual OrganizationDTO Organization { get; set; }
        public virtual List<DepartmentDTO> SubDepartments { get; set; }
        public virtual DepartmentDTO ParentDepartment { get; set; }
        public int? ParentDepartmentId { get; set; }
    }
}
