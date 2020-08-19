using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Docs.Core.Models
{
    public class DocumentSearchCriteria : SearchCriteria
    {
        #region criteria.        

        public List<int> Id { get; set; }
        public List<string> UserIds { get; set; }
        public List<string> UserNames { get; set; }
        public List<string> Apps { get; set; }
        public List<string> Modules { get; set; }
        public List<string> Actions { get; set; }
        public List<string> Entities { get; set; }
        public List<string> AttachId { get; set; }
        public List<string> Category { get; set; }
        
        #endregion

    }
}
