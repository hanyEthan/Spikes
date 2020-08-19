using System.Collections.Generic;

namespace XCore.Services.Hiring.SDK.Models.Search
{
    public class SearchCriteriaDTO<T>
    {
        public List<string> Apps { get; set; }
        public List<string> Modules { get; set; }
        public string Name { get; set; }        
        public List<string> Codes { get; set; }
        public List<T> Ids { get; set; }        
        public bool? IsActive { get; set; }
        public bool PagingEnabled { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public int? Order { get; set; }
        public int? OrderByDirection { get; set; }
        public int OrderByCultureMode { get; set; }
    }
}
