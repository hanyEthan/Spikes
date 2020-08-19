using System.Collections.Generic;

namespace XCore.Services.Docs.SDK.Models
{
    public class DocumentSearchCriteriaDTO
    {
        public List<int> Id { get; set; }
        public List<string> AttachId { get; set; }
        public List<string> UserIds { get; set; }
        public List<string> UserNames { get; set; }
        public List<string> Apps { get; set; }
        public List<string> Modules { get; set; }
        public List<string> Actions { get; set; }
        public List<string> Entities { get; set; }
     
    }
}
