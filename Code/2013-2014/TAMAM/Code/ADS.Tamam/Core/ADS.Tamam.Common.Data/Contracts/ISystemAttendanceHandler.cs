using System;
using System.Collections.Generic;

using ADS.Common.Context;
using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;

namespace ADS.Tamam.Common.Data.Contracts
{
    public interface ISystemAttendanceHandler : IBaseHandler
    {
        ExecutionResponse<List<ScheduleEvent>> GetScheduleEventsDirty();
        ExecutionResponse<List<ScheduleEvent>> GetScheduleEventsMissedOut();
        ExecutionResponse<ScheduleEvent> GetScheduleEvent( Guid id );
        ExecutionResponse<AttendanceRawData> GetAttendanceRawData( Guid id );
        ExecutionResponse<bool> DeleteAttendanceRawData( AttendanceRawData rawData );
        ExecutionResponse<bool> DeleteScheduleEvents( Guid personId , DateTime date );
        ExecutionResponse<bool> DeleteScheduleEvent(ScheduleEvent scheduleEvent);
        ExecutionResponse<List<AttendanceRawData>> GetAttendanceRaw(Guid personId, DateTime date, bool considerAsAttendance);
        ExecutionResponse<bool> ApprovalIntegrityMaintainByOwner( Guid ownerId );
        ExecutionResponse<bool> CanPersonReviewManualEdit( Guid TargetId , Guid ReviewerId );
        ExecutionResponse<List<ScheduleEvent>> GetScheduleEvent(Guid personId, DateTime date);
    }
}
