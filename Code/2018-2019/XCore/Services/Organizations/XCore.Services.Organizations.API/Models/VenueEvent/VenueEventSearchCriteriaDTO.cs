using System.Collections.Generic;

namespace XCore.Services.Organizations.API.Models.VenueEvent
{
    public class VenueEventSearchCriteriaDTO
    {
        #region criteria.        

        public List<int> Ids { get; set; }
        public int VenueId { get; set; }
        public int? EventId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; } = true;
        public bool IncludeRecursive { get; set; } = false;
        public bool PagingEnabled { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public int? Order { get; set; }
        public int? OrderByDirection { get; set; }
        public int? OrderByCultureMode { get; set; }
        #endregion
    }
}
