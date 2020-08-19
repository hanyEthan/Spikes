using System;
using XCore.Services.Audit.Models.Enums;

namespace XCore.Services.Audit.Models.Contracts
{
    public interface IAuditMessage
    {
        string UserId { get; set; }
        string UserName { get; set; }
        string App { get; set; }
        string Module { get; set; }
        string Action { get; set; }
        string Entity { get; set; }
        string Text { get; set; }
        string SourceIP { get; set; }
        string SourcePort { get; set; }
        string SourceOS { get; set; }
        string SourceClient { get; set; }
        string DestinationIP { get; set; }
        string DestinationPort { get; set; }
        string DestinationAddress { get; set; }
        string ConnectionMethod { get; set; }
        AuditTrailLevel? Level { get; set; }
        AuditTrailSyncStatus? SyncStatus { get; set; }

        string CreatedBy { get; set; }
        string ModifiedBy { get; set; }
        string MetaData { get; set; }
    }
}
