using System.Collections.Generic;
using XCore.Services.Personnel.Models.DTO.Base;

namespace XCore.Services.Personnel.Models.DTO.Organizations
{
    public class OrganizationSearchCriteriaDTO : SearchCriteriaDTO
    {
        public List<string> OrganizationReferenceId { get; set; }
    }
}
