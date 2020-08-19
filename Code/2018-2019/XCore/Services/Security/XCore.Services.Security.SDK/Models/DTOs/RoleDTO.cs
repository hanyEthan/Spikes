using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Services.Security.SDK.Contracts;

namespace XCore.Services.Security.SDK.Models.DTOs
{
    public class RoleDTO
    {
        public virtual int Id { get; set; }
        public virtual string Code { get; set; } = Guid.NewGuid().ToString();
        public virtual string Name { get; set; }
        public virtual string NameCultured { get; set; }
        public virtual string Description { get; set; }
        public virtual bool? IsActive { get; set; } = true;
        public virtual string CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string ModifiedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual string MetaData { get; set; }
        public virtual int AppId { get; set; } = 1011;
        public virtual List<PrivilegeDTO> Privileges { get; set; }
        //public virtual List<ActorDTO> Actors { get; set; }
    }
}
