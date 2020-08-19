using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Audit.Models.Contracts;
using XCore.Services.Audit.Models.Enums;

namespace XCore.Services.Audit.Core.Models
{
    public class AuditTrail : Entity<int>, IAuditMessage
    {
        #region IAuditMessage

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string App { get; set; }
        public string Module { get; set; }
        public string Action { get; set; }
        public string Entity { get; set; }
        public string Text { get; set; }
        public string SourceIP { get; set; }
        public string SourcePort { get; set; }
        public string SourceOS { get; set; }
        public string SourceClient { get; set; }
        public string DestinationIP { get; set; }
        public string DestinationPort { get; set; }
        public string DestinationAddress { get; set; }
        public string ConnectionMethod { get; set; }
        public AuditTrailLevel? Level { get; set; }
        public AuditTrailSyncStatus? SyncStatus { get; set; }

        #endregion
    }
}
