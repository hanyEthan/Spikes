using System.Collections.Generic;
using XCore.Services.Personnel.Models.DTO.Base;

namespace XCore.Services.Personnel.Models.DTO.Settings
{
    public class SettingSearchCriteriaDTO : SearchCriteriaDTO
    {
        public List<int> SettingIds { get; set; }
        public List<int> AccountIds { get; set; }
   
    }
}
