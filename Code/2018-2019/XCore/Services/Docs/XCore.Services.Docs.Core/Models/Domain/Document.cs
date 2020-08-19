using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Docs.Core.Models
{
    public class Document : Entity<int>
    {
        public string AttachId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string App { get; set; }
        public string Module { get; set; }
        public string Action { get; set; }
        public string Entity { get; set; }
        public string Category { get; set; }

        
    }
}
