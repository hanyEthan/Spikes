using System.Collections.Generic;

namespace XCore.Services.Configurations.Models
{
    public class ModuleDTO
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
        public virtual int AppId { get; set; }
        
        public virtual AppDTO App { get; set; }
        public virtual IList<ConfigDTO> Configurations { get; set; }
    }
}
