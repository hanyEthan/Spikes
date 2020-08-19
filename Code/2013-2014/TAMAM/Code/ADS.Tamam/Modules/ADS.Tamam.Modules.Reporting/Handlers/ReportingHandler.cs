using System;
using System.Collections.Generic;
using ADS.Common.Context;
using ADS.Common.Handlers;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Contracts;
using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.AttendanceStatistics;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Reports;
using ADS.Tamam.Common.Data.Validation;

namespace ADS.Tamam.Modules.Reporting.Handlers
{
    public class ReportingHandler : IReportingHandler , ISystemReportingHandler , IReadOnlyReportingHandler
    {
        #region Properties

        public bool Initialized { get; private set; }
        public string Name { get { return "ReportingHandler"; } }

        private readonly ReportsDataHandler dataHandler;

        #endregion
        #region cst.

        public ReportingHandler()
        {
            XLogger.Info("Initializing ...");

            if (!TamamDataBroker.Initialized)
            {
                XLogger.Error("Initialization Failed, underlying handlers are not registered or initilaized successfully.");
                return;
            }

            dataHandler = TamamDataBroker.GetRegisteredDataLayer<ReportsDataHandler>( TamamConstants.ReportsDataHandlerName );
            if ( dataHandler != null && dataHandler.Initialized )
            {
                XLogger.Info( "Initialized" );
                Initialized = true;
            }
            else
            {
                XLogger.Error( "Initialization Failed, underlying handlers are not registered or initilaized successfully." );
                Initialized = false;
            }
        }

        #endregion

        #region Scheduled reports

        public ExecutionResponse<ScheduledReportEvent> GetScheduledReportEvent( Guid id, RequestContext requestContext )
        {
            var context = new ExecutionContext<ScheduledReportEvent>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "ReportingHandler_GetScheduledReportEvent" + id + requestContext;
                var cached = Broker.Cache.Get<ScheduledReportEvent>( TamamCacheClusters.ScheduledReports, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.EditScheduledReportEventsAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.ShowAdministration,
                    messageForDenied: TamamConstants.AuthorizationConstants.EditScheduledReportEventsAuditMessageFailure,
                    messageForFailure:
                        TamamConstants.AuthorizationConstants.EditScheduledReportEventsAuditMessageFailure,
                    messageForSuccess:
                        TamamConstants.AuthorizationConstants.EditScheduledReportEventsAuditMessageSuccessful
                    );

                var securityResponse = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( securityResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( securityResponse.Type, null );
                    return;
                }

                var securityContext = securityResponse.Result;

                #endregion
                #region DataLayer

                var dataHandlerResponse = dataHandler.GetScheduledReportEvent( id, securityContext );
                if ( dataHandlerResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext, context.ActionContext, context.ActionContext.MessageForFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                #endregion
                #region Cache

                Broker.Cache.Add<ScheduledReportEvent>( TamamCacheClusters.ScheduledReports, cacheKey, dataHandlerResponse.Result );

                #endregion

                #endregion

            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<List<ScheduledReportEvent>> GetScheduledReportEvents( ScheduledReportEventsSearchCriteria criteria, RequestContext requestContext )
        {
            var context = new ExecutionContext<List<ScheduledReportEvent>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "ReportingHandler_GetScheduledReportEvents" + criteria + requestContext;
                var cached = Broker.Cache.Get<List<ScheduledReportEvent>>( TamamCacheClusters.ScheduledReports, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                                        (
                                        moduleId: TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                                        actionId: TamamConstants.AuthorizationConstants.GetScheduledReportEventsAuditActionId,
                                        actionKey: TamamConstants.AuthorizationConstants.ShowAdministration,
                                        messageForDenied: TamamConstants.AuthorizationConstants.GetScheduledReportEventsAuditMessageFailure,
                                        messageForFailure: TamamConstants.AuthorizationConstants.GetScheduledReportEventsAuditMessageFailure,
                                        messageForSuccess: TamamConstants.AuthorizationConstants.GetScheduledReportEventsAuditMessageSuccessful
                                        );

                var securityResponse = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( securityResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( securityResponse.Type, null );
                    return;
                }

                var securityContext = securityResponse.Result;

                #endregion
                #region DataLayer

                var dataHandlerResponse = dataHandler.GetScheduledReportEvents( criteria, securityContext );
                if ( dataHandlerResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext, context.ActionContext, context.ActionContext.MessageForFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                #endregion
                #region Cache

                Broker.Cache.Add<List<ScheduledReportEvent>>( TamamCacheClusters.ScheduledReports, cacheKey, dataHandlerResponse.Result );

                #endregion

                #endregion

            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> EditScheduledReport( ScheduledReportEvent scheduledReport, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                context.ActionContext = new ActionContext
                                       (
                                        moduleId: TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                                        actionId: TamamConstants.AuthorizationConstants.EditScheduledReportEventsAuditActionId,
                                        actionKey: TamamConstants.AuthorizationConstants.ShowAdministration,
                                        messageForDenied: TamamConstants.AuthorizationConstants.EditScheduledReportEventsAuditMessageFailure,
                                        messageForFailure: TamamConstants.AuthorizationConstants.EditScheduledReportEventsAuditMessageFailure,
                                        messageForSuccess: TamamConstants.AuthorizationConstants.EditScheduledReportEventsAuditMessageSuccessful
                                        );

                var securityResponse = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( securityResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( securityResponse.Type, false );
                    return;
                }

                var securityContext = securityResponse.Result;

                #endregion
                #region Validation

                var validator = new ScheduledReportEventValidator( scheduledReport );

                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext, context.ActionContext, context.ActionContext.MessageForFailure, scheduledReport.Id.ToString() );
                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    return;
                }

                #endregion
                #region Model ...

                scheduledReport.ReportDefinition = null;

                #endregion
                #region DataLayer

                var dataHandlerResponse = dataHandler.EditScheduledReportEvents( scheduledReport );
                if ( dataHandlerResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext, context.ActionContext, context.ActionContext.MessageForFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse.Type, false );
                    return;
                }

                context.Response.Set( ResponseState.Success, dataHandlerResponse.Result );

                #endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.ScheduledReports );

                #endregion

                #endregion

            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> DeleteScheduledReport( Guid id, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                context.ActionContext = new ActionContext
                                        (
                                        moduleId: TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                                        actionId: TamamConstants.AuthorizationConstants.DeleteScheduledReportEventsAuditActionId,
                                        actionKey: TamamConstants.AuthorizationConstants.ShowAdministration,
                                        messageForDenied: TamamConstants.AuthorizationConstants.DeleteScheduledReportEventsAuditMessageFailure,
                                        messageForFailure: TamamConstants.AuthorizationConstants.DeleteScheduledReportEventsAuditMessageFailure,
                                        messageForSuccess: TamamConstants.AuthorizationConstants.DeleteScheduledReportEventsAuditMessageSuccessful
                                        );

                var securityResponse = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( securityResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( securityResponse.Type, false );
                    return;
                }

                var securityContext = securityResponse.Result;

                #endregion
                #region DataLayer

                var dataHandlerResponse = dataHandler.DeleteScheduledReportEvents( id );
                if ( dataHandlerResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext, context.ActionContext, context.ActionContext.MessageForFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse.Type, false );
                    return;
                }

                context.Response.Set( ResponseState.Success, dataHandlerResponse.Result );

                #endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.ScheduledReports );

                #endregion

                #endregion

            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> CreateScheduledReport( ScheduledReportEvent scheduledReport, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                context.ActionContext = new ActionContext
                                        (
                                        moduleId: TamamConstants.AuthorizationConstants.OrganizationAuditModuleId,
                                        actionId: TamamConstants.AuthorizationConstants.CreateScheduledReportEventsAuditActionId,
                                        actionKey: TamamConstants.AuthorizationConstants.ShowAdministration,
                                        messageForDenied: TamamConstants.AuthorizationConstants.CreateScheduledReportEventsAuditMessageFailure,
                                        messageForFailure: TamamConstants.AuthorizationConstants.CreateScheduledReportEventsAuditMessageFailure,
                                        messageForSuccess: TamamConstants.AuthorizationConstants.CreateScheduledReportEventsAuditMessageSuccessful
                                        );

                var securityResponse = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( securityResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( securityResponse.Type, false );
                    return;
                }

                //? ????????????????
                var securityContext = securityResponse.Result;

                #endregion
                #region Validation

                var validator = new ScheduledReportEventValidator( scheduledReport );

                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext, context.ActionContext, context.ActionContext.MessageForFailure, scheduledReport.Id.ToString() );
                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    return;
                }

                #endregion
                #region DataLayer

                var dataHandlerResponse = dataHandler.CreateScheduledReportEvents( scheduledReport );
                if ( dataHandlerResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext, context.ActionContext, context.ActionContext.MessageForFailure, string.Empty );
                    context.Response.Set( dataHandlerResponse.Type, false );
                    return;
                }

                context.Response.Set( ResponseState.Success, dataHandlerResponse.Result );

                #endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.ScheduledReports );

                #endregion

                #endregion

            }, requestContext );

            return context.Response;
        }

        #endregion

        #region IReportingHandler

        public ExecutionResponse<List<AttendanceDivisionSummary>> GetAttendanceDivisionSummary( Guid departmentId , DateTime startDate , DateTime endDate , int sickLeaveId , bool includeRootDepartmentSubordinates , RequestContext requestContext )
        {
            var context = new ExecutionContext<List<AttendanceDivisionSummary>>();
            context.Execute(() =>
            {
                #region Cache

                var cacheKey = "ReportingHandler_GetAttendanceDivisionSummary" + departmentId + startDate.ToShortDateString() + endDate.ToShortDateString() + sickLeaveId + requestContext;
                var cached = Broker.Cache.Get<List<AttendanceDivisionSummary>>(TamamCacheClusters.Reports, cacheKey);
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.AttendanceAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.AttendanceStatsGetActionKey,
                    messageForDenied: TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageFailure,
                    messageForFailure: TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageFailure,
                    messageForSuccess: TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageSuccessful
                    );

                var response = TamamServiceBroker.Secure(context.ActionContext, requestContext);
                if (response.Type != ResponseState.Success)
                {
                    context.Response.Set(response.Type, null);
                    return;
                }

                var securityContext = response.Result;

                #endregion
                #region data layer ...

                // data layer ...
                var dataHandlerResponse = dataHandler.GetAttendanceDivisionSummary( departmentId , startDate , endDate , sickLeaveId , includeRootDepartmentSubordinates , securityContext );
                if (dataHandlerResponse.Result == null)
                {
                    context.Response.Set(dataHandlerResponse); TamamServiceBroker.Audit(requestContext, TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditActionId, TamamConstants.AuthorizationConstants.AttendanceAuditModuleId, TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageFailure, string.Empty);
                    return;
                }

                #endregion

                context.Response.Set(dataHandlerResponse);

                #region Cache

                Broker.Cache.Add<List<AttendanceDivisionSummary>>( TamamCacheClusters.Reports , cacheKey , dataHandlerResponse.Result );

                #endregion

            }, requestContext);

            return context.Response;
        }
        public ExecutionResponse<List<AttendanceDivisionLostWorkingHoursStatistics>> GetAttendanceDivisionLostWorkingHoursStatistics( Guid departmentId , DateTime startDate , DateTime endDate , int sickLeaveId , bool includeRootDepartmentSubordinates , RequestContext requestContext )
        {
            var context = new ExecutionContext<List<AttendanceDivisionLostWorkingHoursStatistics>>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "ReportingHandler_AttendanceDivisionLostWorkingHoursStatistics" + departmentId + startDate.ToShortDateString() + endDate.ToShortDateString() + sickLeaveId + requestContext;
                var cached = Broker.Cache.Get<List<AttendanceDivisionLostWorkingHoursStatistics>>(TamamCacheClusters.Reports, cacheKey);
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.AttendanceAuditModuleId ,
                    actionId: TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditActionId ,
                    actionKey: TamamConstants.AuthorizationConstants.AttendanceStatsGetActionKey ,
                    messageForDenied: TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageFailure ,
                    messageForFailure: TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageFailure ,
                    messageForSuccess: TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageSuccessful
                    );

                var response = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type , null );
                    return;
                }

                var securityContext = response.Result;


                #endregion
                #region data layer ...

                // data layer ...
                var dataHandlerResponse = dataHandler.GetAttendanceDivisionLostWorkingHoursStatistics( departmentId , startDate , endDate , sickLeaveId , includeRootDepartmentSubordinates , securityContext );
                if (dataHandlerResponse.Result == null)
                {
                    context.Response.Set(dataHandlerResponse);
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageFailure , string.Empty );
                    return;
                }

                #endregion

                context.Response.Set(dataHandlerResponse);

                #region Cache

                Broker.Cache.Add<List<AttendanceDivisionLostWorkingHoursStatistics>>( TamamCacheClusters.Reports , cacheKey , dataHandlerResponse.Result );

                #endregion

            } , requestContext );

            return context.Response;
        }        
        public ExecutionResponse<List<PendingNotificationsManager>> GetAttendanceManagerPendingNotifications(List<Guid> managerIds, DateTime startDate, DateTime endDate, RequestContext requestContext)
        {
            var context = new ExecutionContext<List<PendingNotificationsManager>>();
            context.Execute(() =>
            {
                #region Cache

                var cacheKey = "ReportingHandler_GetAttendanceManagerPendingNotifications" + XModel.ListToString(managerIds) + startDate.ToShortDateString() + endDate.ToShortDateString() + requestContext;
                var cached = Broker.Cache.Get<List<PendingNotificationsManager>>(TamamCacheClusters.Reports, cacheKey);
                if (cached != null)
                {
                    context.Response.Set(ResponseState.Success, cached);
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.AttendanceAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.AttendanceStatsGetActionKey,
                    messageForDenied: TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageFailure,
                    messageForFailure: TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageFailure,
                    messageForSuccess: TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageSuccessful
                    );

                var response = TamamServiceBroker.Secure(context.ActionContext, requestContext);
                if (response.Type != ResponseState.Success)
                {
                    context.Response.Set(response.Type, null);
                    return;
                }

                var securityContext = response.Result;


                #endregion
                #region data layer ...

                // data layer ...
                var dataHandlerResponse = dataHandler.GetAttendanceManagerPendingNotifications(managerIds, startDate, endDate, securityContext);
                if (dataHandlerResponse.Result == null)
                {
                    context.Response.Set(dataHandlerResponse);
                    TamamServiceBroker.Audit(requestContext, TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditActionId, TamamConstants.AuthorizationConstants.AttendanceAuditModuleId, TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageFailure, string.Empty);
                    return;
                }

                #endregion

                context.Response.Set(dataHandlerResponse);

                #region Cache

                Broker.Cache.Add<List<PendingNotificationsManager>>(TamamCacheClusters.Reports, cacheKey, dataHandlerResponse.Result);

                #endregion

            }, requestContext);

            return context.Response;
        }        
        public ExecutionResponse<List<PendingNotificationsPerson>> GetAttendancePersonPendingNotifications(List<Guid> personnelIds, DateTime startDate, DateTime endDate, RequestContext requestContext)
        {
            var context = new ExecutionContext<List<PendingNotificationsPerson>>();
            context.Execute(() =>
            {
                #region Cache

                var cacheKey = "ReportingHandler_GetAttendancePersonPendingNotifications" + XModel.ListToString(personnelIds) + startDate.ToShortDateString() + endDate.ToShortDateString() + requestContext;
                var cached = Broker.Cache.Get<List<PendingNotificationsPerson>>(TamamCacheClusters.Reports, cacheKey);
                if (cached != null)
                {
                    context.Response.Set(ResponseState.Success, cached);
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.AttendanceAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.AttendanceStatsGetActionKey,
                    messageForDenied: TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageFailure,
                    messageForFailure: TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageFailure,
                    messageForSuccess: TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageSuccessful
                    );

                var response = TamamServiceBroker.Secure(context.ActionContext, requestContext);
                if (response.Type != ResponseState.Success)
                {
                    context.Response.Set(response.Type, null);
                    return;
                }

                var securityContext = response.Result;


                #endregion
                #region data layer ...

                // data layer ...
                var dataHandlerResponse = dataHandler.GetAttendancePersonPendingNotifications(personnelIds, startDate, endDate, securityContext);
                if (dataHandlerResponse.Result == null)
                {
                    context.Response.Set(dataHandlerResponse);
                    TamamServiceBroker.Audit(requestContext, TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditActionId, TamamConstants.AuthorizationConstants.AttendanceAuditModuleId, TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageFailure, string.Empty);
                    return;
                }

                #endregion

                context.Response.Set(dataHandlerResponse);

                #region Cache

                Broker.Cache.Add<List<PendingNotificationsPerson>>(TamamCacheClusters.Reports, cacheKey, dataHandlerResponse.Result);

                #endregion

            }, requestContext);

            return context.Response;
        }

        public ExecutionResponse<List<ScheduleEvent>> GetScheduleEvents(ScheduleEventSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<List<ScheduleEvent>>();
            context.Execute(() =>
            {
                #region Cache

                var cacheKey = "ReportingHandler_GetScheduleEvents" + criteria + requestContext;
                var cached = Broker.Cache.Get<List<ScheduleEvent>>(TamamCacheClusters.Attendance, cacheKey);
                if (cached != null)
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.AttendanceAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.AttendanceStatsGetActionKey,
                    messageForDenied: TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageFailure,
                    messageForFailure: TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageFailure,
                    messageForSuccess: TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageSuccessful
                    );

                var response = TamamServiceBroker.Secure(context.ActionContext, requestContext);
                if (response.Type != ResponseState.Success)
                {
                    context.Response.Set(response.Type, null);
                    return;
                }

                var securityContext = response.Result;


                #endregion
                #region data layer ...

                var dataHandlerResponse = dataHandler.GetScheduleEvents(criteria, securityContext);
                if (dataHandlerResponse.Result == null)
                {
                    context.Response.Set(dataHandlerResponse);
                    TamamServiceBroker.Audit(requestContext, TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditActionId, TamamConstants.AuthorizationConstants.AttendanceAuditModuleId, TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageFailure, string.Empty);
                    return;
                }

                #endregion

                context.Response.Set(dataHandlerResponse);

                #region Cache

                Broker.Cache.Add<List<ScheduleEvent>>( TamamCacheClusters.Attendance , cacheKey , dataHandlerResponse.Result );

                #endregion
            }, requestContext);

            return context.Response;
        }

        
        
        #endregion
    }
}
