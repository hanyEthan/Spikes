using System;
using System.Collections.Generic;
using System.Text;

namespace XCore.Services.Organizations.SDK.Models.Support
{
   public class DepartmentSearchCriteriaDTO
    {
        public List<int> Ids { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; } = true;
        public int? OrganizationId { get; set; }
        public int? ParentDepartmentId { get; set; }
        public bool IncludeRecursive { get; set; } = false;
        public bool PagingEnabled { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public int? Order { get; set; }
        public int? OrderByDirection { get; set; }
        public int? OrderByCultureMode { get; set; }

    }
}
