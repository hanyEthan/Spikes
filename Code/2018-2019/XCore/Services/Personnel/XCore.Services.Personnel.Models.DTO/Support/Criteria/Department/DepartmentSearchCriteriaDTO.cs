using System.Collections.Generic;
using XCore.Services.Personnel.Models.DTO.Base;

namespace XCore.Services.Personnel.Models.DTO.Departments
{
    public class DepartmentSearchCriteriaDTO : SearchCriteriaDTO
    {
        public List<string> DepartmentReferenceId { get; set; }
        public List<int?> HeadDepartmentId { get; set; }
    }
}
