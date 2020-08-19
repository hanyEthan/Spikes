using System;
using System.Collections.Generic;
using ADS.Common.Context;
using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.AttendanceStatistics;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Tamam.Common.Data.Model.Domain.Reports;

namespace ADS.Tamam.Common.Data.Contracts
{
    public interface IReportingHandler : IBaseHandler
    {
        ExecutionResponse<List<AttendanceDivisionSummary>> GetAttendanceDivisionSummary( Guid departmentId , DateTime startDate , DateTime endDate , int sickLeaveId , bool includeRootDepartmentSubordinates , RequestContext requestContext );
        ExecutionResponse<List<AttendanceDivisionLostWorkingHoursStatistics>> GetAttendanceDivisionLostWorkingHoursStatistics( Guid departmentId , DateTime startDate , DateTime endDate , int sickLeaveId , bool includeRootDepartmentSubordinates , RequestContext requestContext );
        ExecutionResponse<List<PendingNotificationsManager>> GetAttendanceManagerPendingNotifications(List<Guid> managerIds, DateTime startDate, DateTime endDate, RequestContext requestContext);
        ExecutionResponse<List<PendingNotificationsPerson>> GetAttendancePersonPendingNotifications(List<Guid> personnelIds, DateTime startDate, DateTime endDate, RequestContext requestContext);
        ExecutionResponse<List<ScheduleEvent>> GetScheduleEvents( ScheduleEventSearchCriteria criteria , RequestContext requestContext );

        #region ScheduledReportEvent

        ExecutionResponse<ScheduledReportEvent> GetScheduledReportEvent(Guid id, RequestContext requestContext);
        ExecutionResponse<List<ScheduledReportEvent>> GetScheduledReportEvents(ScheduledReportEventsSearchCriteria criteria, RequestContext requestContext);
        ExecutionResponse<bool> EditScheduledReport(ScheduledReportEvent scheduledReport, RequestContext requestContext);
        ExecutionResponse<bool> CreateScheduledReport(ScheduledReportEvent scheduledReport, RequestContext requestContext);
        ExecutionResponse<bool> DeleteScheduledReport(Guid id, RequestContext requestContext);

        #endregion
    }
}