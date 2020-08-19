using System;
using System.Collections.Generic;

using ADS.Common.Context;
using ADS.Common.Contracts;
using ADS.Common.Workflow.Models;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Attendance;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Tamam.Common.Data.Model.Domain.AttendanceStatistics;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Terminal;
using ADS.Tamam.Common.Data.Model.DTO.Composite;
using ADS.Tamam.Common.Data.Model.Domain.PersonnelPrivileges;

namespace ADS.Tamam.Common.Data.Contracts
{
    public interface IAttendanceHandler : IBaseHandler
    {
        ExecutionResponse<Guid> CreateAttendanceRaw( AttendanceRawData data , RequestContext requestContext );
        ExecutionResponse<bool> EditAttendanceRaw( AttendanceRawData data , RequestContext requestContext );
        ExecutionResponse<List<AttendanceRawData>> GetUnProcessedAttendanceRaw ();
        ExecutionResponse<bool> MarkRawDataAsProcessed ( Guid rawDataId );
        ExecutionResponse<bool> HandleAttendanceEvent ( AttendanceRawData rawData , RequestContext requestContext );
        ExecutionResponse<List<AttendanceRawData>> GetAttendanceRaw(AttendanceRawDataFilters filters, RequestContext requestContext);
        ExecutionResponse<List<AttendanceRawData>> GetAttendanceRaw(List<Guid> ScheduleEventIDs, bool? considerAsAttendance, RequestContext requestContext);
        ExecutionResponse<AttendanceRawData> GetAttendanceRaw( Guid id , RequestContext requestContext );
        ExecutionResponse<AttendanceRawDataHistoryItem> GetAttendanceRawDataHistoryItem( Guid id , RequestContext requestContext );
        
        ExecutionResponse<ScheduleEvent> GetScheduleEvent( Guid personId , Guid shiftId , DateTime date );
        ExecutionResponse<List<ScheduleEvent>> GetScheduleEvent( Guid personId , DateTime date );
        ExecutionResponse<Guid> CreateScheduleEvent( ScheduleEvent scheduleEvent );
        ExecutionResponse<DateTime> GetLastScheduleEventDate();
        ExecutionResponse<bool> DeleteScheduleEvent( ScheduleEvent scheduleEvent );
        ExecutionResponse<Guid> UpdateScheduleEvent( ScheduleEvent scheduleEvent );

        ExecutionResponse<Guid> UpdateAttendanceRawData( AttendanceRawData rawData );
        ExecutionResponse<bool> UpdateAttendanceRawData(List<AttendanceRawData> rawData);

        ExecutionResponse<Guid> UpdateAttendanceRawDataHistoryItem( AttendanceRawDataHistoryItem rawDataHistoryItem );

        ExecutionResponse<bool> HandleLeave( Leave leave , RequestContext requestContext );
        ExecutionResponse<bool> HandleAttendanceDuration(DateTime startDate, DateTime endDate, Guid PersonId, RequestContext requestContext);
        ExecutionResponse<bool> HandleExcuse( Excuse excuse , RequestContext requestContext );

        ExecutionResponse<AttendanceStats> GetDepartmentAttendanceStats( Guid departmentId, DateTime date, bool showLeftEarlyAfterShiftEnd, bool showWorkedLessAfterShiftEnd, RequestContext requestContext );
        ExecutionResponse<List<TerminalStatsComposite>> GetDepartmentAttendanceStatsByTerminalIdsComposite( DateTime date, RequestContext requestContext );
        ExecutionResponse<bool> GetDepartmentAttendanceStatsByTerminalIds(RequestContext requestContext);

        ExecutionResponse<ScheduleEventSearchResult> GetAttendances( AttendanceFilters filters , RequestContext requestContext );
        ExecutionResponse<bool> CreateManualAttendance( AttendanceEventMetadata attendanceData , RequestContext requestContext );
        ExecutionResponse<bool> CreateManualAttendance(List<AttendanceEventMetadata> attendanceDataList, RequestContext requestContext);
        ExecutionResponse<List<PersonnelPrivilege>> GetPersonnelPrivileges(AttendanceFilters filters, RequestContext requestContext);
        ExecutionResponse<bool> EditAttendance( AttendanceEventMetadata attendanceData , RequestContext requestContext );
        ExecutionResponse<bool> RestoreAttendanceOriginalValue( AttendanceEventMetadata attendanceData , RequestContext requestContext );
        ExecutionResponse<bool> HandleHolidayPolicy( Guid personId , Policy policy , RequestContext requestContext );
        ExecutionResponse<bool> HandleNativeHolidays(Guid personId, DateTime from, DateTime to, RequestContext requestContext);
        ExecutionResponse<bool> MarkScheduleEventAsDirty(string personnelIds, DateTime startDate, DateTime endDate, RequestContext requestContext);
        ExecutionResponse<List<WorkflowCheckPoint>> ApprovalStepsGet( Guid targetId , RequestContext requestContext );

        ExecutionResponse<List<WorkflowCheckPoint>> AttendanceManualEditApprovalStepsGet( Guid targetId , RequestContext requestContext );

        ExecutionResponse<bool> ReviewAttendanceViolation( AttendanceViolationReview review , RequestContext requestContext );
        ExecutionResponse<bool> ReviewAttendanceManualEdit( AttendanceManualEditReview review , RequestContext requestContext );
    }
}
