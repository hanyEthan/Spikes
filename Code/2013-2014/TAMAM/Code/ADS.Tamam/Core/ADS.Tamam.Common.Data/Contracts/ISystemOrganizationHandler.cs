using System;
using System.Collections.Generic;
using ADS.Common.Context;
using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Model.Domain.Policy;

namespace ADS.Tamam.Common.Data.Contracts
{
    public interface ISystemOrganizationHandler : IBaseHandler
    {
        ExecutionResponse<List<Person>> GetSupervisors( Guid id , RequestContext requestContext );
        ExecutionResponse<List<Guid>> GetDepartmentsPeople( List<Guid> departments , RequestContext requestContext );

        ExecutionResponse<bool> HandlePolicyGroupEvents( List<Guid> departments , List<Guid> personnel );
        
        // check if the system policy group has a leave policy with the provided leave type..
        ExecutionResponse<bool> IsSystemPolicyGroupHasPolicyForLeaveType( int leaveType );
        ExecutionResponse<PolicyGroup> GetSystemPolicyGroup();
    }
}