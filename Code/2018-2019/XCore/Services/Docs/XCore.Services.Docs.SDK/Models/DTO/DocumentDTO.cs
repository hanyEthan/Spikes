using System;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Docs.SDK.Models
{
    public class DocumentDTO  
    {
        public int Id { get; set; }
        public string Code { get; set; } = Guid.NewGuid().ToString();
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string MetaData { get; set; }
        public string AttachId { get; set; }
        public string Category { get; set; }


        public string UserId { get; set; }
        public string UserName { get; set; }
        public string App { get; set; }
        public string Module { get; set; }
        public string Action { get; set; }
        public string Entity { get; set; }

    }
}
