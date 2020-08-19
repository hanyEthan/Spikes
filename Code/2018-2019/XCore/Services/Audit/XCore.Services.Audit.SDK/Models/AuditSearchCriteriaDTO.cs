using System.Collections.Generic;

namespace XCore.Services.Audit.SDK.Models
{
    public class AuditSearchCriteriaDTO
    {
        public List<string> UserIds { get; set; }
        public List<string> UserNames { get; set; }
        public List<string> Apps { get; set; }
        public List<string> Modules { get; set; }
        public List<string> Actions { get; set; }
        public List<string> Entities { get; set; }
        public string Text { get; set; }

        public bool PagingEnabled { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }

        public int? Order { get; set; }
        public int? OrderByDirection { get; set; }
        public int? OrderByCultureMode { get; set; }
    }
}
