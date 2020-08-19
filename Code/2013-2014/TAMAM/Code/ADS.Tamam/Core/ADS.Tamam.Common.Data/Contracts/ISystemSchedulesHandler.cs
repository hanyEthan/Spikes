using System;
using System.Collections.Generic;

using ADS.Common.Context;
using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;

namespace ADS.Tamam.Common.Data.Contracts
{
    public interface ISystemSchedulesHandler : IBaseHandler
    {
        ExecutionResponse<bool> IsValidWorkingHours( Guid personId , DateTime date , TimeSpan from , TimeSpan to );
        ExecutionResponse<double> GetScheduledHoursCount( Guid personId , DateTime date , TimeSpan from , TimeSpan to );
        ExecutionResponse<double> GetExcuseEffectiveTime(Excuse excuse, Shift shift);
        ExecutionResponse<List<ScheduleTemplateDayShifts>> GetScheduledShifts( Guid personId , DateTime date );
        ExecutionResponse<List<DateTime>> GetScheduledDays( Guid personId , DateTime from , DateTime to , bool includeOffDays , bool includeLeaves );
        ExecutionResponse<int> GetScheduledDaysCount( Guid personId , DateTime from , DateTime to , bool includeOffDays , bool includeLeaves );

        // Policies
        ExecutionResponse<Policy> GetShiftPolicy( List<Policy> policies , Shift shift );
        ExecutionResponse<Policy> GetShiftPolicy( Guid personId , Shift shift );
    }
}
