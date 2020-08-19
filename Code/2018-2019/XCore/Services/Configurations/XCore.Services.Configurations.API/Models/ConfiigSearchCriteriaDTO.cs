using System;
using System.Collections.Generic;
using System.Text;

namespace XCore.Services.Configurations.Models
{
    public class ConfigSearchCriteriaDTO
    {
        
        public List<int> AppIds { get; set; }
        public List<int> ModuleIds { get; set; }
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool PagingEnabled { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public int? Order { get; set; }
        public List<string> Keys { get; set; }

        public int? OrderByDirection { get; set; }
        public int OrderByCultureMode { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; }

    }
}
