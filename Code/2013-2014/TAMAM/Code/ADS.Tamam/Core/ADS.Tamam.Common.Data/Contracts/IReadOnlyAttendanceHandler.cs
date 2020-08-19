using System.Collections.Generic;
using ADS.Common.Context;
using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Attendance;
using ADS.Tamam.Common.Data.Model.DTO.Composite;

namespace ADS.Tamam.Common.Data.Contracts
{
    public interface IReadOnlyAttendanceHandler : IBaseHandler
    {
        ExecutionResponse<List<ScheduleEventCurrentlyInDTO>> GetScheduleEventCurrentlyIn( AttendanceFilters filters , RequestContext requestContext );
        ExecutionResponse<List<ScheduleEventHighLightDTO>> GetScheduleEventsHighLight( AttendanceFilters filters , RequestContext requestContext );
    }
}
