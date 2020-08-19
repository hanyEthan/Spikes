using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.SDK.Models.DTOs
{
   public class OrganizationDTO :Entity<int>
    {
        public virtual string Description { get; set; }
        public virtual List<DepartmentDTO> Departments { get; set; }
        public virtual List<SettingsDTO> Settings { get; set; }
        public virtual List<ContactInfoDTO> ContactInfo { get; set; }
        public virtual List<ContactPersonalDTO> ContactPersonnel { get; set; }
        public virtual List<OrganizationDTO> SubOrganizations { get; set; }
        public virtual List<OrganizationDelegationDTO> OrganizationDelegates { get; set; }
        public virtual List<OrganizationDelegationDTO> OrganizationDelegators { get; set; }
        public virtual OrganizationDTO ParentOrganization { get; set; }
        public virtual int? ParentOrganizationId { get; set; }
    }
}
