using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.SDK.Models.DTOs
{
    public class DepartmentDTO : Entity<int>
    {
        public virtual string Description { get; set; }
        public virtual int? OrganizationId { get; set; }
        public virtual OrganizationDTO Organization { get; set; }
        public virtual List<DepartmentDTO> SubDepartments { get; set; }
        public virtual DepartmentDTO ParentDepartment { get; set; }
        public int? ParentDepartmentId { get; set; }

    }

}