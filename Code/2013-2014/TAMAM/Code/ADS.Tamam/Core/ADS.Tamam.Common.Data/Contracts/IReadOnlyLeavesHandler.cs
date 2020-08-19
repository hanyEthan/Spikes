using System.Collections.Generic;
using ADS.Common.Context;
using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Tamam.Common.Data.Model.DTO.Composite;

namespace ADS.Tamam.Common.Data.Contracts
{
    public interface IReadOnlyLeavesHandler : IBaseHandler
    {
        ExecutionResponse<List<LeaveDTO>> GetCompositeLeave( LeaveSearchCriteria criteria , bool activePersonsOnly , RequestContext requestContext );
    }
}
