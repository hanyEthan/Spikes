using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XCore.Services.Security.API.Model
{
    public class TargetDTO
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
        public virtual int PrivilegeId { get; set; }
        public virtual int AppId { get; set; }

    }
}
