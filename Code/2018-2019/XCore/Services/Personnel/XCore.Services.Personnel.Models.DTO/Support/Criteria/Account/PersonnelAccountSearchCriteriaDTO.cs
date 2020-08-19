using System.Collections.Generic;
using XCore.Services.Personnel.Models.DTO.Base;

namespace XCore.Services.Personnel.Models.DTO.Accounts
{
    public class PersonnelAccountSearchCriteriaDTO : SearchCriteriaDTO
    {
        public List<int> AccountIds { get; set; }
        public List<int> PersonIds { get; set; }
     
    }
}
