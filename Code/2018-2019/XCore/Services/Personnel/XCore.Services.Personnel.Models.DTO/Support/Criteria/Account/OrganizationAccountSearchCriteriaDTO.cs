using System.Collections.Generic;
using XCore.Services.Personnel.Models.DTO.Base;

namespace XCore.Services.Personnel.Models.DTO.Accounts
{
    public class OrganizationAccountSearchCriteriaDTO : SearchCriteriaDTO
    {
        public List<int> AccountIds { get; set; }
        public List<int> OrganizationIds { get; set; }
    }
}
