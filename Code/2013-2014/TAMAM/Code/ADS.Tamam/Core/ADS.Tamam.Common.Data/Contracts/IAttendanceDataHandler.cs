using System;
using System.Collections.Generic;

using ADS.Common.Context;
using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Attendance;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Tamam.Common.Data.Model.Domain.AttendanceStatistics;
using ADS.Tamam.Common.Data.Model.Domain.Terminal;
using ADS.Tamam.Common.Data.Model.DTO.Composite;
using ADS.Tamam.Common.Data.Model.Domain.PersonnelPrivileges;

namespace ADS.Tamam.Common.Data.Contracts
{
    public interface IAttendanceDataHandler : IDataAccessHandler
    {
        ExecutionResponse<Guid> CreateAttendanceRaw ( AttendanceRawData data );
        ExecutionResponse<bool> EditAttendanceRaw( AttendanceRawData data );
        ExecutionResponse<bool> DeleteAttendanceRaw( AttendanceRawData data );
        ExecutionResponse<List<AttendanceRawData>> GetUnProcessedAttendanceRaw ();
        ExecutionResponse<bool> MarkRawDataAsProcessed ( Guid rawDataId );
        ExecutionResponse<ScheduleEvent> GetScheduleEvent ( Guid personId , Guid shiftId , DateTime date );
        ExecutionResponse<List<ScheduleEvent>> GetScheduleEvent( Guid personId , DateTime date );
        ExecutionResponse<Guid> CreateScheduleEvent ( ScheduleEvent scheduleEvent );
        ExecutionResponse<Guid> UpdateScheduleEvent ( ScheduleEvent scheduleEvent );

        ExecutionResponse<Guid> UpdateAttendanceRawData( AttendanceRawData rawData );
        ExecutionResponse<bool> UpdateAttendanceRawData(List<AttendanceRawData> rawData);

        ExecutionResponse<Guid> UpdateAttendanceRawDataHistoryItem( AttendanceRawDataHistoryItem rawDataHistoryItem );

        ExecutionResponse<bool> DeleteScheduleEvent ( ScheduleEvent scheduleEvent );
        ExecutionResponse<bool> DeleteAttendanceEvent ( AttendanceEvent attendanceEvent );
        ExecutionResponse<bool> AddAttendanceEvent( AttendanceEvent attendanceEvent );
        ExecutionResponse<DateTime> GetLastScheduleEventDate();
        ExecutionResponse<List<ScheduleEvent>> GetDirtyScheduleEvents();
        ExecutionResponse<List<ScheduleEvent>> GetScheduleEventsMissedOut();
        ExecutionResponse<List<AttendanceRawData>> GetAttendanceRaw(Guid personId, DateTime date, bool considerAsAttendance);
        ExecutionResponse<List<AttendanceRawData>> GetAttendanceRaw(AttendanceRawDataFilters filters, SecurityContext securityContext);
        ExecutionResponse<List<AttendanceRawData>> GetAttendanceRaw(List<Guid> ScheduleEventIDs, bool? considerAsAttendance, SecurityContext securityContext);
        ExecutionResponse<bool> DeletePersonScheduleEvents( Guid personId , DateTime date );
        ExecutionResponse<AttendanceStats> GetDepartmentAttendanceStats( Guid departmentId, DateTime date, bool showLeftEarlyAfterShiftEnd, bool showWorkedLessAfterShiftEnd, SecurityContext securityContext );
        ExecutionResponse<List<TerminalStatsComposite>> GetDepartmentAttendanceStatsByTerminalIdsComposite( DateTime date, SecurityContext securityContext );
        ExecutionResponse<bool> GetDepartmentAttendanceStatsByTerminalIds(DateTime date, bool showLeftEarlyAfterShiftEnd, bool showWorkedLessAfterShiftEnd, SecurityContext securityContext);
        //ExecutionResponse<AttendanceDivisionSummary> GetAttendanceDivisionSummary( Guid departmentId , DateTime startDate , DateTime endDate , int sickLeaveId , SecurityContext securityContext );

        ExecutionResponse<ScheduleEventSearchResult> GetScheduleEvents( AttendanceFilters filters , SecurityContext securityContext );
        ExecutionResponse<List<ScheduleEventCurrentlyInDTO>> GetScheduleEventsCurrentlyIn(AttendanceFilters filters, SecurityContext securityContext);
        ExecutionResponse<List<ScheduleEventHighLightDTO>> GetScheduleEventsHighLight(AttendanceFilters filters, SecurityContext securityContext);

        ExecutionResponse<ScheduleEvent> GetScheduleEvent( Guid id );

        ExecutionResponse<ScheduleEvent> GetScheduleEventDetailed( Guid id );
        ExecutionResponse<AttendanceRawData> GetAttendanceRaw( Guid id );
        ExecutionResponse<bool> CheckScheduleEvent( Guid id );

        ExecutionResponse<bool> MarkScheduleEventAsDirty(string personnelIds, DateTime startDate, DateTime endDate);

        ExecutionResponse<AttendanceRawDataHistoryItem> GetRawDataHistoryItem( Guid historyId );
        ExecutionResponse<bool> EditRawDataHistoryItem( AttendanceRawDataHistoryItem historyItem );

        ExecutionResponse<AttendanceEvent> GetAttendanceEvent( Guid AttendanceEventId );
        ExecutionResponse<bool> DeleteAttendanceRawDataHistoryItems( List<AttendanceRawDataHistoryItem> HistoryItems );
        ExecutionResponse<List<PersonnelPrivilege>> GetPersonnelPrivileges(AttendanceFilters filters, SecurityContext securityContext);
    }
}
