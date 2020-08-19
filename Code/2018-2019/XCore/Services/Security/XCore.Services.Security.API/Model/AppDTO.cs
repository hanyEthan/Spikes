using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XCore.Services.Security.API.Model
{
    public class AppDTO
    {
        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string NameCultured { get; set; }
        public virtual string Description { get; set; }
        public virtual bool? IsActive { get; set; }
        public virtual string CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string ModifiedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual string MetaData { get; set; }
        //public List<PrivilegeDTO> Privileges { get; set; }
        //public List<RoleDTO> Roles { get; set; }
        //public List<ActorDTO> Actors { get; set; }
        //public List<TargetDTO> Targets { get; set; }
    }
}
