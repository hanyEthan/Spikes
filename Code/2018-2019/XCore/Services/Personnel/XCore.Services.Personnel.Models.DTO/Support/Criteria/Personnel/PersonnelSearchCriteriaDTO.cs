using System.Collections.Generic;
using XCore.Services.Personnel.Models.DTO.Base;

namespace XCore.Services.Personnel.Models.DTO.Personnels
{
    public class PersonnelSearchCriteriaDTO : SearchCriteriaDTO
    {
        public List<int> PersonnelIds { get; set; }
        public List<int?> ManagerIds { get; set; }
        public List<int?> DepartmentIds { get; set; }
    }
}
