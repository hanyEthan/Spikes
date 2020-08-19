using System;
using System.Collections.Generic;
using System.Text;

namespace XCore.Services.Config.API.Models
{
    public class ConfigSearchCriteriaDTO
    {
        
        public int? AppId { get; set; }
        public int? ModuleId { get; set; }
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool PagingEnabled { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public int? Order { get; set; }
        public int? OrderByDirection { get; set; }
        public int OrderByCultureMode { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; } = true;

    }
}
