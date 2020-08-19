using System;
using System.Collections.Generic;
using XCore.Services.Configurations.Core.Models.Domain;

namespace XCore.Services.Configurations.Models
{
    public class ConfigDTO
    {
        public virtual int Id { get; set; }
        public virtual string Key { get; set; }
        public virtual string Value { get; set; }
        public virtual string Description { get; set; }
        public virtual string Type { get; set; }
        public virtual bool ReadOnly { get; set; }
        public virtual string Version { get; set; }

        public virtual int ModuleId { get; set; }
        public virtual ModuleDTO Module { get; set; }

        public virtual int AppId { get; set; }
        public AppDTO App { get; set; }

        public virtual string Code { get; set; } = Guid.NewGuid().ToString();

        public virtual string Name { get; set; }
        public virtual string NameCultured { get; set; }

        public virtual bool? IsActive { get; set; } = true;


        public virtual string CreatedDate { get; set; }
        public virtual string ModifiedDate { get; set; }

        public virtual string CreatedBy { get; set; }
        public virtual string ModifiedBy { get; set; }

        public virtual string MetaData { get; set; }

       

    }
}
