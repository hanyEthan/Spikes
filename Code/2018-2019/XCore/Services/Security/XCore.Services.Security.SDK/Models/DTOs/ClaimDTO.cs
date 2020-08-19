using System;
using System.Collections.Generic;
using System.Text;

namespace XCore.Services.Security.SDK.Models.DTOs
{
   public class ClaimDTO
    {
        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Key { get; set; }
        public virtual string Value { get; set; }
        public virtual string Map { get; set; }
        public virtual string Description { get; set; }
        public virtual bool? IsActive { get; set; }
        public virtual string CreatedDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual string ModifiedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual string MetaData { get; set; }
        public virtual int AppId { get; set; }
        public virtual List<RoleDTO> Roles { get; set; }
        public virtual List<ActorDTO> Actors { get; set; }
    }
}
