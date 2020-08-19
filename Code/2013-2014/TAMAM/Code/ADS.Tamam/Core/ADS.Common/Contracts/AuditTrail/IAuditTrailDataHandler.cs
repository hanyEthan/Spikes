using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADS.Common.Models.Domain;
using ADS.Common.Models.DTO;

namespace ADS.Common.Contracts.AuditTrail
{
    public interface IAuditTrailDataHandler : IBaseHandler
    {
        List<AuditTrailAction> GetAuditTrailActions();
        List<AuditTrailAction> GetAuditTrailActions(AuditTrailActionCriteria criteria);
        AuditTrailAction GetAuditTrailAction(Guid id);
        AuditTrailAction AddAuditTrailAction(AuditTrailAction item);
        bool DeleteAuditTrailAction(Guid id);


        AuditTrailLog Log(AuditTrailLog item);
        List<AuditTrailLog> GetLogs(AuditTrailLogCriteria criteria, out int totalCount);
        bool DeleteLog(Guid id);

        List<AuditTrailModule> GetAuditTrailModules(AuditTrailModuleCriteria criteria);
        List<AuditTrailModule> GetAuditTrailModules();
        AuditTrailModule GetAuditTrailModule(Guid id);
        AuditTrailModule AddAuditTrailModule(AuditTrailModule item);
        bool DeleteAuditTrailModule(Guid id);

        
    }
}
