using System;
using System.Collections.Generic;
using ADS.Common.Context;
using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;

namespace ADS.Tamam.Common.Data.Contracts
{
    public interface ISystemLeavesHandler : IBaseHandler
    {
        ExecutionResponse<Leave> GetLeave( Guid personId , DateTime date );
        ExecutionResponse<bool> CreateLeaves(List<Leave> leaves, bool systemLevelAction, RequestContext requestContext);
        ExecutionResponse<bool> EditLeave(Leave leave, bool systemLevelAction, RequestContext requestContext);
        ExecutionResponse<bool> UpdateLeaveCredit( Guid personId , Leave leave , double daysAmount , RequestContext requestContext );

        ExecutionResponse<bool> CreateExcuses(List<Excuse> excuses, bool systemLevelAction, RequestContext requestContext);
        ExecutionResponse<bool> EditExcuse(Excuse excuse, bool systemLevelAction, RequestContext requestContext);

        //ExecutionResponse<LeavePreCredit> GetPersonLeavePreCredit( Guid personId );
    }
}
