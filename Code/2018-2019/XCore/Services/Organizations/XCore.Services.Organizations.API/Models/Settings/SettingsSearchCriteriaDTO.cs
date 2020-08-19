using System.Collections.Generic;

namespace XCore.Services.Organizations.API.Models.Settings
{
    public class SettingsSearchCriteriaDTO
    {
      
        public List<int> Ids { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; } = true;
        public int? OrgainzationId { get; set; }
        public bool PagingEnabled { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }

        public int? Order { get; set; }
        public int? OrderByDirection { get; set; }
        public int? OrderByCultureMode { get; set; }

    }
}
