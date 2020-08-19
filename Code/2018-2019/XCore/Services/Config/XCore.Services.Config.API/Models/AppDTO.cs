﻿using System.Collections.Generic;

namespace XCore.Services.Config.API.Models
{
    public class AppDTO
    {
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
        public List<ModuleDTO> modulelist { get; set; }

    }
}
