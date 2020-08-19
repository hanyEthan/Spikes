using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Services.Personnel.Models.Enums;

namespace XCore.Services.Personnel.Models.DTO.Base
{
    public class SearchCriteriaDTO
    {
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
        public SearchIncludesEnum SearchIncludes { get; set; }
    }
}
