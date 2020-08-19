using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.API.Models.ContactInfo;
using XCore.Services.Organizations.API.Models.ContactPersonal;
using XCore.Services.Organizations.API.Models.Department;
using XCore.Services.Organizations.API.Models.OrganizationDelegation;
using XCore.Services.Organizations.API.Models.Settings;

namespace XCore.Services.Organizations.API.Models.Organization
{
    public class OrganizationDTO
    {
        public virtual int Id { get; set; }
        public virtual string Description { get; set; }
        public virtual string Code { get; set; } =  Guid.NewGuid().ToString();
        public virtual bool IsActive { get; set; }
        public virtual string MetaData { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public virtual string Name { get; set; }
        public virtual string NameCultured { get; set; }

        public virtual List<DepartmentDTO> Departments { get; set; }
        public virtual List<SettingsDTO> Settings { get; set; }
        public virtual List<ContactInfoDTO> ContactInfo { get; set; }
        public virtual List<ContactPersonalDTO> ContactPersonnel { get; set; }
        public virtual List<OrganizationDTO> SubOrganizations { get; set; }

        public virtual int? ParentOrganizationId { get; set; }
    }
}
