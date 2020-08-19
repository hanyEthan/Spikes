using System;
using System.Collections.Generic;

using ADS.Common.Models.Domain;

namespace ADS.Common.Contracts.AuditTrail
{
    public interface IAuditTrailHandler : IBaseHandler
    {
        Guid? Log( AuditTrailLog item );
        List<AuditTrailLog> GetLogs( AuditTrailLogCriteria criteria , out int totalCount );
        bool DeleteLog( Guid id );

        List<AuditTrailAction> GetActions();
        List<AuditTrailAction> GetActions( AuditTrailActionCriteria criteria );
        AuditTrailAction GetAction( Guid id );
        AuditTrailAction AddAction( AuditTrailAction item );
        bool DeleteAction( Guid id );

        AuditTrailModule GetModule( Guid id );
        List<AuditTrailModule> GetModules();
        List<AuditTrailModule> GetModules( AuditTrailModuleCriteria criteria );
        AuditTrailModule AddModule( AuditTrailModule item );
        bool DeleteModule( Guid id );
    }
}
