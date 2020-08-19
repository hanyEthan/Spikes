using System;
using System.Linq;
using System.Collections.Generic;
using ADS.Common.Context;
using ADS.Common.Handlers;
using ADS.Common.Utilities;
using ADS.Common.Validation;
using ADS.Common.Workflow.Models;
using ADS.Tamam.Common.Data;
using ADS.Tamam.Common.Data.Model.Domain.Attendance;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Contracts;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Tamam.Common.Data.Model.Domain.AttendanceStatistics;
using ADS.Tamam.Common.Data.Validation;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.ManualEdit.Data;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.ManualEdit.Definitions;
using ADS.Tamam.Resources.Culture;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.Violations.Definitions;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.Violations.Data;
using ADS.Common.Workflow.Enums;
using ADS.Tamam.Common.Data.Model.DTO.Composite;
using System.Diagnostics;
using ADS.Common;
using ADS.Tamam.Common.Data.Model.Domain.Terminal;
using ADS.Tamam.Common.Data.Model.Domain.PersonnelPrivileges;

namespace ADS.Tamam.Modules.Attendance.Handlers
{
    public partial class AttendanceHandler : IAttendanceHandler , ISystemAttendanceHandler , IReadOnlyAttendanceHandler
    {
        #region Properties

        public bool Initialized { get; private set; }
        public string Name { get { return "AttendanceHandler"; } }

        private readonly IAttendanceDataHandler dataHandler;
        private readonly AttendanceManualEditApprovalWorkflowDefinition manualEditWorkflowDefinition = new AttendanceManualEditApprovalWorkflowDefinition();

        #endregion
        #region cst.

        public AttendanceHandler()
        {
            XLogger.Info( "AttendanceHandler ... Initializing ..." );

            if ( !TamamDataBroker.Initialized )
            {
                XLogger.Error( "AttendanceHandler ... Initialization Failed, underlying handlers are not registered or initilaized successfully." );
                return;
            }

            dataHandler = TamamDataBroker.GetRegisteredDataLayer<IAttendanceDataHandler>( TamamConstants.AttendanceDataHandlerName );
            if ( dataHandler != null && dataHandler.Initialized )
            {
                XLogger.Info( "AttendanceHandler ... Initialized" );
                Initialized = true;
            }
            else
            {
                XLogger.Error( "AttendanceHandler ... Initialization Failed, underlying handlers are not registered or initilaized successfully." );
                Initialized = false;
            }
        }

        #endregion

        #region IAttendanceHandler

        #region RAW

        public ExecutionResponse<Guid> CreateAttendanceRaw( AttendanceRawData data , RequestContext requestContext )
        {
            var context = new ExecutionContext<Guid>();
            context.Execute( () =>
            {
                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError , Guid.Empty , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext , TamamConstants.AuthorizationConstants.AttendancePunchActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendancePunchAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendancePunchAuditMessageFailure , "" );
                    context.Response.Set( ResponseState.AccessDenied , Guid.Empty , new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region validation ...


                #endregion
                #region data layer ...

                // data layer ...
                var dataHandlerResponse = dataHandler.CreateAttendanceRaw(data);

                //? This expression is always false
                //if ( dataHandlerResponse.Result == null )
                //{
                //    context.Response.Set( dataHandlerResponse );
                //    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendancePunchAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendancePunchAuditMessageFailure , "" );
                //    return;
                //}

                #endregion
                #region audit ...

               // audit ...
                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendancePunchAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendancePunchAuditMessageSuccessful , "" );

                #endregion

                context.Response.Set(dataHandlerResponse);

                #region Cache

                Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

                #endregion
            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> EditAttendanceRaw( AttendanceRawData data , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError , false , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext , TamamConstants.AuthorizationConstants.AttendancePunchActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendancePunchAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendancePunchAuditMessageFailure , "" );
                    context.Response.Set( ResponseState.AccessDenied , false , new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region validation ...

                #endregion
                #region data layer ...

                // data layer ...
                var response = dataHandler.EditAttendanceRaw( data );
                if ( !response.Result )
                {
                    context.Response.Set( response );
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendancePunchAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendancePunchAuditMessageFailure , "" );
                    return;
                }

                #endregion
                #region audit ...

                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendancePunchAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendancePunchAuditMessageSuccessful , "" );

                #endregion

                context.Response.Set( response );

                #region Cache

                Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

                #endregion

            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<List<AttendanceRawData>> GetUnProcessedAttendanceRaw()
        {
            var context = new ExecutionContext<List<AttendanceRawData>>();
            context.Execute(() =>
            {
                #region Cache

                //const string cacheKey = "AttendanceHandler_GetUnProcessedAttendanceRaw";
                //var cached = Broker.Cache.Get<List<AttendanceRawData>>(TamamCacheClusters.Attendance, cacheKey);
                //if ( cached != null )
                //{
                //    context.Response.Set( ResponseState.Success , cached );
                //    return;
                //}

                #endregion
                #region data layer ...

                var dataHandlerResponse = dataHandler.GetUnProcessedAttendanceRaw();
                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                //Broker.Cache.Add<List<AttendanceRawData>>( TamamCacheClusters.Attendance , cacheKey , dataHandlerResponse.Result );

                #endregion
            });

            return context.Response;
        }
        public ExecutionResponse<bool> MarkRawDataAsProcessed( Guid rawDataId )
        {
            var context = new ExecutionContext<bool>();
            context.Execute(() =>
            {
                #region data layer ...

                var dataHandlerResponse = dataHandler.MarkRawDataAsProcessed(rawDataId);
                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

                #endregion
            });

            return context.Response;
        }
        public ExecutionResponse<List<AttendanceRawData>> GetAttendanceRaw( AttendanceRawDataFilters filters , RequestContext requestContext )
        {
            var context = new ExecutionContext<List<AttendanceRawData>>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "AttendanceHandler_GetAttendanceRaws" + filters + requestContext;
                var cached = Broker.Cache.Get<List<AttendanceRawData>>(TamamCacheClusters.Attendance, cacheKey);
                if (cached != null)
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.AttendanceAuditModuleId ,
                    actionId: TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId ,
                    actionKey: TamamConstants.AuthorizationConstants.AttendanceGetActionKey ,
                    messageForDenied: TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure ,
                    messageForFailure: TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure ,
                    messageForSuccess: TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageSuccessful
                    );

                var response = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type , null );
                    return;
                }

                var securityContext = response.Result;

                #endregion
                #region validation ...

                if ( filters == null )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure , "" );
                    context.Response.Set( ResponseState.ValidationError , null , new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region data layer ...

                // data layer ...
                var dataHandlerResponse = dataHandler.GetAttendanceRaw(filters, securityContext);
                if ( dataHandlerResponse.Result == null )
                {
                    context.Response.Set( dataHandlerResponse );
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure , "" );
                    return;
                }

                #endregion
                #region audit ...

                // audit ...
                //TamamServiceBroker.Audit ( requestContext , TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageSuccessful , "" );

                #endregion

                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Add<List<AttendanceRawData>>( TamamCacheClusters.Attendance , cacheKey , dataHandlerResponse.Result );

                #endregion

            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<List<AttendanceRawData>> GetAttendanceRaw(List<Guid> ScheduleEventIDs, bool? considerAsAttendance, RequestContext requestContext)
        {
            var context = new ExecutionContext<List<AttendanceRawData>>();
            context.Execute(() =>
            {
                #region Cache

                var cacheKey = "AttendanceHandler_GetAttendanceRaws" + XModel.ListToString(ScheduleEventIDs) + considerAsAttendance.ToString()+ requestContext;
                var cached = Broker.Cache.Get<List<AttendanceRawData>>(TamamCacheClusters.Attendance, cacheKey);
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
                    actionId: TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.AttendanceGetActionKey,
                    messageForDenied: TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure,
                    messageForFailure: TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure,
                    messageForSuccess: TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageSuccessful
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
                var dataHandlerResponse = dataHandler.GetAttendanceRaw(ScheduleEventIDs,considerAsAttendance, securityContext);
                if (dataHandlerResponse.Result == null)
                {
                    context.Response.Set(dataHandlerResponse);
                    TamamServiceBroker.Audit(requestContext, TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId, TamamConstants.AuthorizationConstants.AttendanceAuditModuleId, TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure, "");
                    return;
                }

                #endregion
                #region audit ...

                // audit ...
                //TamamServiceBroker.Audit ( requestContext , TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageSuccessful , "" );

                #endregion

                context.Response.Set(dataHandlerResponse);

                #region Cache

                Broker.Cache.Add<List<AttendanceRawData>>(TamamCacheClusters.Attendance, cacheKey, dataHandlerResponse.Result);

                #endregion

            }, requestContext);

            return context.Response;
        }

        public ExecutionResponse<AttendanceRawData> GetAttendanceRaw( Guid id , RequestContext requestContext )
        {
            var context = new ExecutionContext<AttendanceRawData>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "AttendanceHandler_GetAttendanceRaw" + id + requestContext;
                var cached = Broker.Cache.Get<AttendanceRawData>(TamamCacheClusters.Attendance, cacheKey);
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError , null , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext , TamamConstants.AuthorizationConstants.AttendanceGetActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure , "" );
                    context.Response.Set( ResponseState.AccessDenied , null , new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region validation ...

                #endregion
                #region data handler ...

                var dataHandlerResponse = dataHandler.GetAttendanceRaw(id);
                if (dataHandlerResponse.Result == null)
                {
                    context.Response.Set(dataHandlerResponse);
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure , "" );
                    return;
                }

                #endregion
                #region audit ...

                // audit ...
                //TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageSuccessful , "" );

                #endregion

                context.Response.Set( ResponseState.Success , dataHandlerResponse.Result );

                #region Cache

                Broker.Cache.Add<AttendanceRawData>( TamamCacheClusters.Attendance , cacheKey , dataHandlerResponse.Result );

                #endregion

            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<AttendanceRawDataHistoryItem> GetAttendanceRawDataHistoryItem(Guid id, RequestContext requestContext)
        {
            var context = new ExecutionContext<AttendanceRawDataHistoryItem>();
            context.Execute(() =>
            {
                #region Cache

                var cacheKey = "AttendanceHandler_GetAttendanceRawDataHistoryItem" + id + requestContext;
                var cached = Broker.Cache.Get<AttendanceRawDataHistoryItem>( TamamCacheClusters.Attendance , cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Security

                //// authentication ...
                //if (!TamamServiceBroker.Authenticate(requestContext))
                //{
                //    context.Response.Set(ResponseState.AuthenticationError, null, new List<ModelMetaPair>());
                //    return;
                //}

                //// authorization ...
                //if (
                //    !TamamServiceBroker.Authorize(requestContext,
                //        TamamConstants.AuthorizationConstants.AttendanceGetActionKey))
                //{
                //    TamamServiceBroker.Audit(requestContext,
                //        TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId,
                //        TamamConstants.AuthorizationConstants.AttendanceAuditModuleId,
                //        TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure, "");
                //    context.Response.Set(ResponseState.AccessDenied, null, new List<ModelMetaPair>());
                //    return;
                //}

                #endregion
                #region data layer ...

                // data layer ...
                var dataHandlerResponse = dataHandler.GetRawDataHistoryItem(id);

                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                Broker.Cache.Add<AttendanceRawDataHistoryItem>( TamamCacheClusters.Attendance , cacheKey , dataHandlerResponse.Result );

                #endregion

            }, requestContext);

            return context.Response;
        }
        public ExecutionResponse<Guid> UpdateAttendanceRawData( AttendanceRawData rawData )
        {
            var context = new ExecutionContext<Guid>();
            context.Execute(() =>
            {
                #region data layer ...

                var dataHandlerResponse = dataHandler.UpdateAttendanceRawData(rawData);
                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

                #endregion
            });

            return context.Response;
        }
        public ExecutionResponse<Guid> UpdateAttendanceRawDataHistoryItem( AttendanceRawDataHistoryItem rawDataHistoryItem)
        {
            var context = new ExecutionContext<Guid>();

            context.Execute(() =>
            {
                #region data layer ...

                var dataHandlerResponse = dataHandler.UpdateAttendanceRawDataHistoryItem(rawDataHistoryItem);
                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

                #endregion
            });
            return context.Response;
        }
        public ExecutionResponse<bool> UpdateAttendanceRawData(List<AttendanceRawData> rawData)
        {
            var context = new ExecutionContext<bool>();
            context.Execute(() =>
            {
                #region data layer ...

                var dataHandlerResponse = dataHandler.UpdateAttendanceRawData(rawData);
                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

                #endregion
            });

            return context.Response;
        }

        #endregion
        #region ScheduleEvent

        public ExecutionResponse<ScheduleEvent> GetScheduleEvent(Guid personId, Guid shiftId, DateTime date)
        {
            var context = new ExecutionContext<ScheduleEvent>();
            context.Execute(() =>
            {
                #region Cache

                var cacheKey = "AttendanceHandler_GetScheduleEvent" + personId + shiftId + date;
                var cached = Broker.Cache.Get<ScheduleEvent>(TamamCacheClusters.Attendance, cacheKey);
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Data layer ...

                var dataHandlerResponse = dataHandler.GetScheduleEvent(personId, shiftId, date);
                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                Broker.Cache.Add<ScheduleEvent>(TamamCacheClusters.Attendance, cacheKey, dataHandlerResponse.Result);

                #endregion
            });

            return context.Response;
        }
        public ExecutionResponse<List<ScheduleEvent>> GetScheduleEvent(Guid personId, DateTime date)
        {
            var context = new ExecutionContext<List<ScheduleEvent>>();
            context.Execute(() =>
            {
                #region Cache

                var cacheKey = "AttendanceHandler_GetScheduleEvent" + personId + date;
                var cached = Broker.Cache.Get<List<ScheduleEvent>>(TamamCacheClusters.Attendance, cacheKey);
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Data layer ...

                var dataHandlerResponse = dataHandler.GetScheduleEvent(personId, date);
                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                Broker.Cache.Add<List<ScheduleEvent>>( TamamCacheClusters.Attendance , cacheKey , dataHandlerResponse.Result );

                #endregion
            });

            return context.Response;
        }
        public ExecutionResponse<Guid> UpdateScheduleEvent(ScheduleEvent scheduleEvent)
        {
            var context = new ExecutionContext<Guid>();
            context.Execute(() =>
            {
                #region data layer ...

                var dataHandlerResponse = dataHandler.UpdateScheduleEvent(scheduleEvent);
                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

                #endregion
            });
            return context.Response;
        }
        public ExecutionResponse<Guid> CreateScheduleEvent(ScheduleEvent scheduleEvent)
        {
            var context = new ExecutionContext<Guid>();
            context.Execute(() =>
            {
                #region data layer ...

                var dataHandlerResponse = dataHandler.CreateScheduleEvent(scheduleEvent);
                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

                #endregion
            });
            return context.Response;
        }
        public ExecutionResponse<DateTime> GetLastScheduleEventDate()
        {
            //! Apply Caching in data handler
            var context = new ExecutionContext<DateTime>();
            context.Execute(() =>
            {
                #region Cache

                const string cacheKey = "AttendanceHandler_GetLastScheduleEventDate";
                var cached = Broker.Cache.Get<DateTime?>( TamamCacheClusters.Attendance , cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached.Value );
                    return;
                }

                #endregion
                #region Data layer ...

                var dataHandlerResponse = dataHandler.GetLastScheduleEventDate();
                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                Broker.Cache.Add<DateTime?>(TamamCacheClusters.Attendance, cacheKey, dataHandlerResponse.Result);

                #endregion
            });

            return context.Response;
        }
        public ExecutionResponse<bool> DeleteScheduleEvent(ScheduleEvent scheduleEvent)
        {
            var context = new ExecutionContext<bool>();
            context.Execute(() =>
            {
                #region data layer ...

                var dataHandlerResponse = dataHandler.DeleteScheduleEvent(scheduleEvent);

                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

                #endregion

            });
            return context.Response;
        }

        #endregion
        #region Leave & Excuse

        public ExecutionResponse<bool> HandleLeave(Leave leave, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Execute(() =>
            {
                List<DateTime> dates = Enumerable.Range( 0 , ( int ) ( ( leave.EndDate - leave.StartDate ).TotalDays ) + 1 ).Select( n => leave.StartDate.AddDays( n ) ).ToList();
                foreach (var date in dates)
                {
                    var scheduleEvents = GetScheduleEvent(leave.PersonId, date).Result;
                    if (scheduleEvents != null)
                    {
                        foreach (var SE in scheduleEvents)
                        {
                            SE.IsDirty = true;
                            UpdateScheduleEvent( SE );
                        }
                    }
                }
                context.Response.Set( ResponseState.Success , true );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Attendance );

                #endregion

            }, requestContext);

            return context.Response;
        }

        public ExecutionResponse<bool> HandleAttendanceDuration(DateTime startDate, DateTime endDate, Guid PersonId, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Execute(() =>
            {
                List<DateTime> dates = Enumerable.Range(0, (int)((endDate - startDate).TotalDays) + 1).Select(n => startDate.AddDays(n)).ToList();
                foreach (var date in dates)
                {
                    var scheduleEvents = GetScheduleEvent(PersonId, date).Result;
                    if (scheduleEvents != null)
                    {
                        foreach (var SE in scheduleEvents)
                        {
                            SE.IsDirty = true;
                            UpdateScheduleEvent(SE);
                        }
                    }
                }
                context.Response.Set(ResponseState.Success, true);

                #region Cache

                Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

                #endregion

            }, requestContext);

            return context.Response;
        }

        public ExecutionResponse<bool> HandleExcuse( Excuse excuse , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                var scheduleEvents = GetScheduleEvent( excuse.PersonId , excuse.ExcuseDate ).Result;
                if ( scheduleEvents != null )
                {
                    foreach ( var SE in scheduleEvents )
                    {
                        SE.IsDirty = true;
                        UpdateScheduleEvent( SE );
                    }
                }
                context.Response.Set( ResponseState.Success , true );
            } , requestContext );

            #region Cache

            Broker.Cache.Invalidate( TamamCacheClusters.Attendance );

            #endregion

            return context.Response;
        }

        #endregion
        #region Attendance Event

        public ExecutionResponse<AttendanceStats> GetDepartmentAttendanceStats( Guid departmentId, DateTime date, bool showLeftEarlyAfterShiftEnd, bool showWorkedLessAfterShiftEnd, RequestContext requestContext )
        {
            var context = new ExecutionContext<AttendanceStats>();
            context.Execute(() =>
            {
                #region Cache

                var cacheKey = "AttendanceHandler_GetDepartmentAttendanceStats" + departmentId + date + showLeftEarlyAfterShiftEnd + showWorkedLessAfterShiftEnd + requestContext;
                var cached = Broker.Cache.Get<AttendanceStats>(TamamCacheClusters.Attendance, cacheKey);
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
                #region Data layer ...

                // data layer ...
                var dataHandlerResponse = dataHandler.GetDepartmentAttendanceStats( departmentId, date, showLeftEarlyAfterShiftEnd, showWorkedLessAfterShiftEnd, securityContext );
                if (dataHandlerResponse.Result == null)
                {
                    context.Response.Set(dataHandlerResponse);
                    TamamServiceBroker.Audit(requestContext,
                        TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditActionId,
                        TamamConstants.AuthorizationConstants.AttendanceAuditModuleId,
                        TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageFailure, "");
                    return;
                }

                #endregion
                #region audit ...

                // audit ...
                //TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageSuccessful , "" );

                #endregion

                context.Response.Set(dataHandlerResponse);

                #region Cache

                Broker.Cache.Add<AttendanceStats>( TamamCacheClusters.Attendance , cacheKey , dataHandlerResponse.Result );

                #endregion

            }, requestContext);

            return context.Response;
        }
        public ExecutionResponse<List<TerminalStatsComposite>> GetDepartmentAttendanceStatsByTerminalIdsComposite( DateTime date, RequestContext requestContext )
        {
            var context = new ExecutionContext<List<TerminalStatsComposite>>();
            context.Execute(() =>
            {
                #region Cache

                var cacheKey = "AttendanceHandler_GetDepartmentAttendanceStatsByTerminalIdsComposite"+ date + requestContext;
                var cached = Broker.Cache.Get<List<TerminalStatsComposite>>(TamamCacheClusters.Attendance, cacheKey);
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
                #region Data layer ...

                var dataHandlerResponse = dataHandler.GetDepartmentAttendanceStatsByTerminalIdsComposite( date, securityContext );
                if (dataHandlerResponse.Result == null)
                {
                    context.Response.Set(dataHandlerResponse);
                    //TamamServiceBroker.Audit(requestContext,
                    //    TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditActionId,
                    //    TamamConstants.AuthorizationConstants.AttendanceAuditModuleId,
                    //    TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageFailure, "");
                    return;
                }

                #endregion
                #region audit ...

                // audit ...
                //TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageSuccessful , "" );

                #endregion

                context.Response.Set(dataHandlerResponse);

                #region Cache

                Broker.Cache.Add<List<TerminalStatsComposite>>( TamamCacheClusters.Attendance , cacheKey , dataHandlerResponse.Result );

                #endregion

            }, requestContext);

            return context.Response;
        }
        public ExecutionResponse<bool> GetDepartmentAttendanceStatsByTerminalIds(RequestContext requestContext)
        {
            bool showLeftEarlyAfterShiftEnd, showWorkedLessAfterShiftEnd;
            var context = new ExecutionContext<bool>();
            context.Execute(() =>
            {
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
                    context.Response.Set(response.Type, false);
                    return;
                }

                var securityContext = response.Result;

                #endregion
                #region Data layer ...

                if (!bool.TryParse(Broker.ConfigurationHandler.GetValue(Constants.TamamAttendanceConfig.Section,
                        Constants.TamamAttendanceConfig.DashboardAttendanceStatsShowLeftEarlyAfterShiftEndKey),
                    out showLeftEarlyAfterShiftEnd))
                {
                    showLeftEarlyAfterShiftEnd = true;
                }
                if (!bool.TryParse(Broker.ConfigurationHandler.GetValue(Constants.TamamAttendanceConfig.Section,
                    Constants.TamamAttendanceConfig.DashboardAttendanceStatsShowWorkedLessAfterShiftEndKey),out showWorkedLessAfterShiftEnd))
                {
                    showWorkedLessAfterShiftEnd = true;
                }

                var dataHandlerResponse = dataHandler.GetDepartmentAttendanceStatsByTerminalIds(DateTime.Now.Date, showLeftEarlyAfterShiftEnd, showWorkedLessAfterShiftEnd, securityContext);
                if (dataHandlerResponse.Result == false)
                {
                    context.Response.Set(dataHandlerResponse);
                    return;
                }

                #endregion
                #region audit ...

                // audit ...
                //TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceStatsGetAuditMessageSuccessful , "" );

                #endregion

                context.Response.Set(dataHandlerResponse);

                #region Cache

                //Broker.Cache.Add<List<AttendanceStatsByTerminalIds>>(TamamCacheClusters.Attendance, cacheKey, dataHandlerResponse.Result);

                #endregion

            }, requestContext);

            return context.Response;
        }

        
        #endregion
        #region Holiday Policy

        public ExecutionResponse<bool> HandleHolidayPolicy( Guid personId , Policy policy , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                if ( policy.PolicyTypeId != Guid.Parse( PolicyTypes.HolidayPolicyType ) ) return;
                var holidayPolicy = new HolidayPolicy( policy );
                List<DateTime> dates = Enumerable.Range( 0 , ( int ) ( ( holidayPolicy.DateTo.Value - holidayPolicy.DateFrom.Value ).TotalDays ) + 1 ).Select( n => holidayPolicy.DateFrom.Value.AddDays( n ) ).ToList();
                foreach ( var date in dates )
                {
                    var scheduleEvents = GetScheduleEvent( personId , date ).Result;
                    if ( scheduleEvents != null )
                    {
                        foreach ( var SE in scheduleEvents )
                        {
                            SE.IsDirty = true;
                            UpdateScheduleEvent( SE );
                        }
                    }
                }
                context.Response.Set( ResponseState.Success , true );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Attendance );

                #endregion

            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> HandleNativeHolidays( Guid personId , DateTime from , DateTime to , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                List<DateTime> dates = Enumerable.Range( 0 , ( int ) ( ( to - from ).TotalDays ) + 1 ).Select( n => from.AddDays( n ) ).ToList();
                foreach ( var date in dates )
                {
                    var scheduleEvents = GetScheduleEvent( personId , date ).Result;
                    if ( scheduleEvents != null )
                    {
                        foreach ( var SE in scheduleEvents )
                        {
                            SE.IsDirty = true;
                            UpdateScheduleEvent( SE );
                        }
                    }
                }

                //foreach ( var date in dates )
                //{
                //    var scheduleEvents = GetScheduleEvent( personId , date ).Result;
                //    if ( scheduleEvents != null )
                //    {
                //        foreach ( var SE in scheduleEvents )
                //        {
                //            SE.IsDirty = true;
                //            UpdateScheduleEvent( SE );
                //        }
                //    }
                //}
                context.Response.Set( ResponseState.Success , true );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Attendance );

                #endregion

            } , requestContext );

            return context.Response;
        }

        #endregion
        #region Attendance ...

        public ExecutionResponse<ScheduleEventSearchResult> GetAttendances( AttendanceFilters filters , RequestContext requestContext )
        {
            var context = new ExecutionContext<ScheduleEventSearchResult>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "AttendanceHandler_GetAttendances" + filters+ requestContext;
                var cached = Broker.Cache.Get<ScheduleEventSearchResult>( TamamCacheClusters.Attendance , cacheKey );
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
                                        actionId: TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId ,
                                        actionKey: TamamConstants.AuthorizationConstants.AttendanceGetActionKey ,
                                        messageForDenied: TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure ,
                                        messageForFailure: TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure ,
                                        messageForSuccess: TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageSuccessful
                                        );

                var response = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type , null );
                    return;
                }

                var securityContext = response.Result;

                //// authentication ...
                //if ( !TamamServiceBroker.Authenticate ( requestContext ) )
                //{
                //    context.Response.Set ( ResponseState.AuthenticationError , null , new List<ModelMetaPair> () );
                //    return;
                //}

                //// authorization ...
                //if ( !TamamServiceBroker.Authorize ( requestContext , TamamConstants.AuthorizationConstants.AttendanceGetActionKey ) )
                //{
                //    TamamServiceBroker.Audit ( requestContext , TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure , "" );
                //    context.Response.Set ( ResponseState.AccessDenied , null , new List<ModelMetaPair> () );
                //    return;
                //}

                #endregion
                #region validation ...

                if ( filters == null )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure , "" );
                    context.Response.Set( ResponseState.ValidationError , null , new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region data layer ...

                // data layer ...
                var dataHandlerResponse = dataHandler.GetScheduleEvents(filters, securityContext);
                if (dataHandlerResponse.Result == null)
                {
                    context.Response.Set(dataHandlerResponse);
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure , "" );
                    return;
                }

                #endregion
                #region audit ...

                // audit ...
                //TamamServiceBroker.Audit ( requestContext , TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageSuccessful , "" );

                #endregion

                context.Response.Set(dataHandlerResponse);

                #region Cache
               
                Broker.Cache.Add<ScheduleEventSearchResult>( TamamCacheClusters.Attendance , cacheKey , dataHandlerResponse.Result );
                
                #endregion

            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> CreateManualAttendance( AttendanceEventMetadata attendanceData , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError , false , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext , TamamConstants.AuthorizationConstants.AttendanceCreateActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceCreateAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceCreateAuditMessageFailure , "" );
                    context.Response.Set( ResponseState.AccessDenied , false , new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region validation ...

                var validator = new AttendanceValidator( attendanceData , AttendanceValidator.ValidationMode.Create );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceEditAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceEditAuditMessageFailure , "" );
                    context.Response.Set( ResponseState.ValidationError , false , validator.ErrorsDetailed );

                    return;
                }

                #endregion
                #region data layer ...

                if ( !CreateManualAttendanceInstance( attendanceData , false , requestContext , context ) ) return;

                #endregion
                #region audit ...

                // audit ...
                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceCreateAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceCreateAuditMessageSuccessful , "" );

                #endregion

                context.Response.Set( ResponseState.Success , true );

                #region Cache

                Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

                #endregion

            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> CreateManualAttendance( List<AttendanceEventMetadata> attendanceDataList , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError , false , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext , TamamConstants.AuthorizationConstants.AttendanceCreateActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceCreateAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceCreateAuditMessageFailure , "" );
                    context.Response.Set( ResponseState.AccessDenied , false , new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region validation ...

                if ( attendanceDataList == null || !attendanceDataList.Any() )
                {
                    context.Response.Set( ResponseState.InvalidInput , false , new List<ModelMetaPair>() { new ModelMetaPair( "PersonId" , ValidationResources.PersonIdEmpty ) } );
                    return;
                }


                var validaionErrors = new List<ModelMetaPair>();
                foreach ( var item in attendanceDataList )
                {
                    var validator = new AttendanceValidator( item , AttendanceValidator.ValidationMode.Create );
                    if ( validator.IsValid != null && !validator.IsValid.Value )
                    {
                        TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceEditAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceEditAuditMessageFailure , "" );
                        //context.Response.Set(ResponseState.ValidationError, false, validator.ErrorsDetailed);
                        validaionErrors.AddRange( validator.ErrorsDetailed );
                    }
                }

                // return if violating validations
                if ( validaionErrors.Any() )
                {
                    context.Response.Set( ResponseState.ValidationError , false , validaionErrors );
                    return;
                }

                #endregion
                #region data layer ...

                foreach ( var item in attendanceDataList )
                {
                    if ( !CreateManualAttendanceInstance( item , false , requestContext , context ) )
                    {
                        context.Response.Set( context.Response );
                        return;
                    }

                    // update schedule events..
                    var scheduleEvents = dataHandler.GetScheduleEvent( ( Guid ) item.PersonId.Value , item.Date ).Result;
                    foreach ( var schEvent in scheduleEvents )
                    {
                        schEvent.IsDirty = true;
                        dataHandler.UpdateScheduleEvent( schEvent );
                    }
                }

                #endregion
                #region audit ...

                // audit ...
                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceCreateAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceCreateAuditMessageSuccessful , "" );

                #endregion

                context.Response.Set( ResponseState.Success , true );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Attendance );

                #endregion

            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> EditAttendance( AttendanceEventMetadata attendanceData , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError , false , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext , TamamConstants.AuthorizationConstants.AttendanceEditActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceEditAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.AttendanceEditAuditMessageFailure , attendanceData == null ? "" : attendanceData.PersonId.HasValue ? "" : attendanceData.PersonId.Value.ToString() , attendanceData == null ? "" : attendanceData.Date.ToString( "dd/MM/yyyy" ) ) , "" );
                    context.Response.Set( ResponseState.AccessDenied , false , new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region validation ...

                var validator = new AttendanceValidator( attendanceData , AttendanceValidator.ValidationMode.Edit );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceEditAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.AttendanceEditAuditMessageFailure , attendanceData == null ? "" : attendanceData.PersonId.HasValue ? "" : attendanceData.PersonId.Value.ToString() , attendanceData == null ? "" : attendanceData.Date.ToString( "dd/MM/yyyy" ) ) , "" );
                    context.Response.Set( ResponseState.ValidationError , false , validator.ErrorsDetailed );

                    return;
                }

                #endregion
                #region logic ...

                // 1. get schedule event ...
                var scheduleEvent = dataHandler.GetScheduleEvent( attendanceData.ScheduleId.Value ).Result;

                // 2. mark it as dirty
                scheduleEvent.IsDirty = true;

                // 3. get raw data from schedule event
                Guid rawDataId;

                var attendanceEvent = scheduleEvent.AttendanceEvents.FirstOrDefault(x => x.Id == attendanceData.AttendanceId);
                if ( attendanceEvent != null )
                {
                    rawDataId = attendanceData.Mode == AttendanceEventType.In ? attendanceEvent.InTimeRawId : attendanceEvent.OutTimeRawId;
                }
                else
                {
                    rawDataId = attendanceData.Mode == AttendanceEventType.In ? scheduleEvent.InTimeRawId : scheduleEvent.OutTimeRawId;
                }

                var response_raw = dataHandler.GetAttendanceRaw( rawDataId );
                if ( response_raw.Type != ResponseState.Success && response_raw.Type != ResponseState.NotFound )
                {
                    context.Response.Set( response_raw.Type , false , null );
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceEditAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.AttendanceEditAuditMessageFailure , attendanceData == null ? "" : attendanceData.PersonId.HasValue ? "" : attendanceData.PersonId.Value.ToString() , attendanceData == null ? "" : attendanceData.Date.ToString( "dd/MM/yyyy" ) ) , "" );

                    return;
                }

                // Create a new AttendanceRawData when there is no one with the same rawDataId...
                if ( response_raw.Type == ResponseState.NotFound )
                {
                    // 4. create raw data if not found ...

                    if ( !CreateManualAttendanceInstance( attendanceData , true , requestContext , context ) ) return;
                }
                else
                {
                    // 5. update raw data and mark it as dirty if found

                    var rawData = response_raw.Result;

                    var oldDateTime = rawData.AttendanceDateTime;
                    var updatedDateTime = new DateTime( attendanceData.Date.Year , attendanceData.Date.Month , attendanceData.Date.Day , attendanceData.Time.Value.Hour , attendanceData.Time.Value.Minute , attendanceData.Time.Value.Second );

                   
                    rawData.IsOriginal = false;
                    //rawData.IsManual = true;
                    rawData.ConsiderAsAttendance = true;
                    rawData.RawComment = attendanceData.Comment;
                    rawData.CreationDate = DateTime.Now;
                    rawData.AttendanceDateTime = updatedDateTime;
                    rawData.Username = requestContext.Person.AuthenticationInfo.Username;
                    rawData.ManualAttendanceStatus = ManualAttendanceStatus.Pending;                  

                    var historyId = Guid.NewGuid();
                    var historyItem = new AttendanceRawDataHistoryItem( historyId , oldDateTime , updatedDateTime , rawData.PersonId , DateTime.Now , requestContext.Person.AuthenticationInfo.Username , attendanceData.Comment , ManualAttendanceStatus.Pending );

                    rawData.AddHistoryItem( historyItem );

                    // data layer ...
                    var response_rawEdit = dataHandler.EditAttendanceRaw( rawData );
                    if ( response_rawEdit.Type != ResponseState.Success )
                    {
                        context.Response.Set( response_raw.Type , false , null );
                        TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceEditAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.AttendanceEditAuditMessageFailure , attendanceData == null ? "" : attendanceData.PersonId.HasValue ? "" : attendanceData.PersonId.Value.ToString() , attendanceData == null ? "" : attendanceData.Date.ToString( "dd/MM/yyyy" ) ) , "" );

                        return;
                    }

                    // Get History Item ..
                    var RawDataHistoryItem = dataHandler.GetRawDataHistoryItem( historyId ).Result;

                    // Start Manual Edit WF ....
                    if ( !Broker.WorkflowEngine.Initialize( RawDataHistoryItem , manualEditWorkflowDefinition ) )
                    {
                        context.Response.Set( ResponseState.SystemError , false );
                        return;
                    }

                }

                // 6. save schedule event

                // because of sometimes WF also mark the SE as Dirty,
                // and AttendanceEngine service start working directly for those SE, 
                // we must make sure that SE is still existing for updating..
                var response_SE_Get = dataHandler.GetScheduleEvent( scheduleEvent.Id );
                if ( response_SE_Get.Type == ResponseState.Success )
                {
                    var response_SEEdit = dataHandler.UpdateScheduleEvent( scheduleEvent );
                    if ( response_SEEdit.Type != ResponseState.Success && response_SEEdit.Type != ResponseState.Failure )
                    {
                        context.Response.Set( response_raw.Type , false , null );
                        TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceEditAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.AttendanceEditAuditMessageFailure , attendanceData == null ? "" : attendanceData.PersonId.HasValue ? "" : attendanceData.PersonId.Value.ToString() , attendanceData == null ? "" : attendanceData.Date.ToString( "dd/MM/yyyy" ) ) , "" );

                        return;
                    }
                }

                #endregion
                #region audit ...

                // audit ...
                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceEditAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.AttendanceEditAuditMessageSuccessful , attendanceData == null ? "" : attendanceData.PersonId.HasValue ? "" : attendanceData.PersonId.Value.ToString() , attendanceData == null ? "" : attendanceData.Date.ToString( "dd/MM/yyyy" ) ) , "" );

                #endregion

                context.Response.Set( ResponseState.Success , true );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Attendance );

                #endregion

            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> RestoreAttendanceOriginalValue( AttendanceEventMetadata attendanceData , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError , false , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext , TamamConstants.AuthorizationConstants.AttendanceEditActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceEditAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.AttendanceEditAuditMessageFailure , attendanceData == null ? "" : attendanceData.PersonId.HasValue ? "" : attendanceData.PersonId.Value.ToString() , attendanceData == null ? "" : attendanceData.Date.ToString( "dd/MM/yyyy" ) ) , "" );
                    context.Response.Set( ResponseState.AccessDenied , false , new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region validation ...

                var validator = new AttendanceValidator( attendanceData , AttendanceValidator.ValidationMode.Delete );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceEditAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.AttendanceEditAuditMessageFailure , attendanceData == null ? "" : attendanceData.PersonId.HasValue ? "" : attendanceData.PersonId.Value.ToString() , attendanceData == null ? "" : attendanceData.Date.ToString( "dd/MM/yyyy" ) ) , "" );
                    context.Response.Set( ResponseState.ValidationError , false , validator.ErrorsDetailed );

                    return;
                }

                #endregion
                #region logic ...

                // 1. get schedule event ...
                var scheduleEvent = dataHandler.GetScheduleEvent( attendanceData.ScheduleId.Value ).Result;

                // 2. mark it as dirty
                scheduleEvent.IsDirty = true;

                // 3. get raw data from schedule event
                Guid rawDataId;

                var attendanceEvent = scheduleEvent.AttendanceEvents.FirstOrDefault(x => x.Id == attendanceData.AttendanceId);
                if ( attendanceEvent != null )
                {
                    rawDataId = attendanceData.Mode == AttendanceEventType.In ? attendanceEvent.InTimeRawId : attendanceEvent.OutTimeRawId;
                }
                else
                {
                    rawDataId = attendanceData.Mode == AttendanceEventType.In ? scheduleEvent.InTimeRawId : scheduleEvent.OutTimeRawId;
                }

                var response_raw = dataHandler.GetAttendanceRaw( rawDataId );
                if ( response_raw.Type != ResponseState.Success )
                {
                    context.Response.Set( response_raw.Type , false , null );
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceEditAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.AttendanceEditAuditMessageFailure , attendanceData == null ? "" : attendanceData.PersonId.HasValue ? "" : attendanceData.PersonId.Value.ToString() , attendanceData == null ? "" : attendanceData.Date.ToString( "dd/MM/yyyy" ) ) , "" );

                    return;
                }

                // 4. update raw data and mark it as dirty if found

                var rawData = response_raw.Result;
                if ( rawData.IsManual )
                {
                    // delete Associated WF instances..
                    foreach ( AttendanceRawDataHistoryItem item in rawData.DataHistoryItems )
                    {
                        Broker.WorkflowEngine.CancelAndDelete( item , manualEditWorkflowDefinition.Id );
                    }

                    // no original version, delete it
                    var response_rawDelete = dataHandler.DeleteAttendanceRaw( rawData );
                    if ( response_rawDelete.Type != ResponseState.Success )
                    {
                        context.Response.Set( response_raw.Type , false , null );
                        TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceEditAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.AttendanceEditAuditMessageFailure , attendanceData == null ? "" : attendanceData.PersonId.HasValue ? "" : attendanceData.PersonId.Value.ToString() , attendanceData == null ? "" : attendanceData.Date.ToString( "dd/MM/yyyy" ) ) , "" );
                        return;
                    }
                }
                else
                {
                    // delete Associated WF instances..
                    foreach ( AttendanceRawDataHistoryItem item in rawData.DataHistoryItems )
                    {
                        Broker.WorkflowEngine.CancelAndDelete( item , manualEditWorkflowDefinition.Id );
                    }

                    // restore it..
                    var historyItem = rawData.DataHistoryItems.Where(x => x.ManualAttendanceStatus == ManualAttendanceStatus.Approved || x.ManualAttendanceStatus == ManualAttendanceStatus.Pending).OrderBy(x => x.UpdateTime).FirstOrDefault();
                    if ( historyItem == null )
                    {
                        rawData.IsOriginal = true;
                        rawData.ConsiderAsAttendance = true;
                        rawData.RawComment = attendanceData.Comment;
                        rawData.CreationDate = DateTime.Now;
                        rawData.ManualAttendanceStatus = ManualAttendanceStatus.NotSet;
                        rawData.Username = requestContext.Person.AuthenticationInfo.Username;
                    }
                    else
                    {
                        var originalValue = historyItem.ValueOld.Value;

                        rawData.IsOriginal = true;
                        rawData.ConsiderAsAttendance = true;
                        rawData.RawComment = attendanceData.Comment;
                        rawData.CreationDate = DateTime.Now;
                        rawData.AttendanceDateTime = originalValue;
                        rawData.ManualAttendanceStatus = ManualAttendanceStatus.NotSet;
                        rawData.Username = requestContext.Person.AuthenticationInfo.Username;
                    }

                    var response_rawEdit = dataHandler.EditAttendanceRaw( rawData );
                    if ( response_rawEdit.Type != ResponseState.Success )
                    {
                        context.Response.Set( response_raw.Type , false , null );
                        TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceEditAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.AttendanceEditAuditMessageFailure , attendanceData == null ? "" : attendanceData.PersonId.HasValue ? "" : attendanceData.PersonId.Value.ToString() , attendanceData == null ? "" : attendanceData.Date.ToString( "dd/MM/yyyy" ) ) , "" );
                        return;
                    }

                    // delete associated History Items..
                    var response_HistoryItems_Delete = dataHandler.DeleteAttendanceRawDataHistoryItems( rawData.DataHistoryItems.ToList() );
                    if ( response_HistoryItems_Delete.Type != ResponseState.Success )
                    {
                        context.Response.Set( response_HistoryItems_Delete.Type , false , null );
                        TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceEditAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.AttendanceEditAuditMessageFailure , attendanceData == null ? "" : attendanceData.PersonId.HasValue ? "" : attendanceData.PersonId.Value.ToString() , attendanceData == null ? "" : attendanceData.Date.ToString( "dd/MM/yyyy" ) ) , "" );
                        return;
                    }


                    // restore it
                    //var historyList = rawData.DataHistoryItems.OrderBy( x => x.UpdateTime ).ToList();
                    //var originalValue = historyList[0].ValueOld.Value;
                    //var oldDateTime = rawData.AttendanceDateTime;
                    //var updatedDateTime = originalValue;

                    //rawData.IsOriginal = true;
                    //rawData.RawComment = attendanceData.Comment;
                    //rawData.CreationDate = DateTime.Now;
                    //rawData.AttendanceDateTime = updatedDateTime;
                    //rawData.Username = requestContext.Person.AuthenticationInfo.Username;
                    //rawData.AddHistoryItem( Guid.NewGuid() , oldDateTime , updatedDateTime , rawData.PersonId , DateTime.Now , requestContext.CallerUsername , attendanceData.Comment , rawData.ManualAttendanceStatus );

                    //var response_rawEdit = DataHandler.EditAttendanceRaw( rawData );
                    //if ( response_rawEdit.Type != ResponseState.Success )
                    //{
                    //    context.Response.Set( response_raw.Type , false , null );
                    //    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceEditAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.AttendanceEditAuditMessageFailure , attendanceData == null ? "" : attendanceData.PersonId.HasValue ? "" : attendanceData.PersonId.Value.ToString() , attendanceData == null ? "" : attendanceData.Date.ToString( "dd/MM/yyyy" ) ) , "" );
                    //    return;
                    //}
                }

                // 5. save schedule event
                var response_SEEdit = dataHandler.UpdateScheduleEvent( scheduleEvent );
                if ( response_SEEdit.Type != ResponseState.Success )
                {
                    context.Response.Set( response_raw.Type , false , null );
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceEditAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.AttendanceEditAuditMessageFailure , attendanceData == null ? "" : attendanceData.PersonId.HasValue ? "" : attendanceData.PersonId.Value.ToString() , attendanceData == null ? "" : attendanceData.Date.ToString( "dd/MM/yyyy" ) ) , "" );
                    return;
                }

                #endregion
                #region audit ...

                // audit ...
                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceEditAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.AttendanceEditAuditMessageSuccessful , attendanceData == null ? "" : attendanceData.PersonId.HasValue ? "" : attendanceData.PersonId.Value.ToString() , attendanceData == null ? "" : attendanceData.Date.ToString( "dd/MM/yyyy" ) ) , "" );

                #endregion

                context.Response.Set( ResponseState.Success , true );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Attendance );

                #endregion

            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<AttendanceRawData> GetAttendanceRawData( Guid id )
        {
            var context = new ExecutionContext<AttendanceRawData>();
            context.Execute(() =>
            {
                #region Cache

                var cacheKey = "AttendanceHandler_GetAttendanceRawData" + id;
                var cached = Broker.Cache.Get<AttendanceRawData>(TamamCacheClusters.Attendance, cacheKey);
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Data layer ...

                var dataHandlerResponse = dataHandler.GetAttendanceRaw(id);
                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                Broker.Cache.Add<AttendanceRawData>(TamamCacheClusters.Attendance, cacheKey, dataHandlerResponse.Result);

                #endregion
            });

            return context.Response;
        }
        public ExecutionResponse<bool> MarkScheduleEventAsDirty(string personnelIds, DateTime startDate, DateTime endDate, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            context.Execute(() =>
            {
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.AttendanceAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.AttendanceReprocessActionKey,
                    actionKey: TamamConstants.AuthorizationConstants.AttendanceReprocessActionKey,
                    messageForDenied: TamamConstants.AuthorizationConstants.AttendanceReprocessAuditMessageFailure,
                    messageForFailure: TamamConstants.AuthorizationConstants.AttendanceReprocessAuditMessageFailure,
                    messageForSuccess: TamamConstants.AuthorizationConstants.AttendanceReprocessAuditMessageSuccessful
                    );

                var response = TamamServiceBroker.Secure(context.ActionContext, requestContext);
                if (response.Type != ResponseState.Success)
                {
                    context.Response.Set(response.Type, false);
                    return;
                }

                var securityContext = response.Result;

                #endregion
                #region Validations

                if (string.IsNullOrWhiteSpace(personnelIds))
                {
                    context.Response.Set(ResponseState.ValidationError, false,
                        new List<ModelMetaPair> {new ModelMetaPair("PersonId", ValidationResources.PersonIdEmpty)});
                }

                #endregion

                var markResponse = dataHandler.MarkScheduleEventAsDirty(personnelIds, startDate, endDate);
                if (markResponse.Type != ResponseState.Success)
                {
                    #region audit ...

                    // audit ...
                    TamamServiceBroker.Audit(requestContext, TamamConstants.AuthorizationConstants.AttendanceReprocessAuditActionId, TamamConstants.AuthorizationConstants.AttendanceAuditModuleId, TamamConstants.AuthorizationConstants.AttendanceReprocessAuditMessageFailure, "");

                    #endregion
                    context.Response.Set(markResponse.Type, false);
                }

                #region audit ...

                // audit ...
                TamamServiceBroker.Audit(requestContext, TamamConstants.AuthorizationConstants.AttendanceReprocessAuditActionId, TamamConstants.AuthorizationConstants.AttendanceAuditModuleId, TamamConstants.AuthorizationConstants.AttendanceReprocessAuditMessageSuccessful, "");
                #endregion
                context.Response.Set(ResponseState.Success, true);

                #region Cache

                Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

                #endregion
            });

            return context.Response;
        }
        public ExecutionResponse<List<WorkflowCheckPoint>> ApprovalStepsGet( Guid targetId , RequestContext requestContext )
        {
            var context = new ExecutionContext<List<WorkflowCheckPoint>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "AttendanceHandler_ApprovalStepsGet" + targetId + requestContext;
                var cached = Broker.Cache.Get<List<WorkflowCheckPoint>>(TamamCacheClusters.Attendance, cacheKey);
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Security

                #endregion

                var instance = Broker.WorkflowEngine.GetInstance( targetId , violationsWorkflowDefinition.Id );
                if ( instance == null )
                {
                    context.Response.Set( ResponseState.NotFound , null );
                    return;
                }

                var checkPointsStack = instance.CPS;
                List<WorkflowCheckPoint> checkPoints;
                if ( checkPointsStack != null && checkPointsStack.Count > 0 )
                {
                    checkPoints = instance.CheckPoints;
                    checkPoints.AddRange( checkPointsStack.ToList() );
                }
                else
                {
                    checkPoints = instance.CheckPoints;
                    //checkPoints = instance.CheckPoints.Count >= instance.CheckPointsTemplates.Count ? instance.CheckPoints : instance.CheckPointsTemplates;
                }

                context.Response.Set( ResponseState.Success , checkPoints );

                #region Cache

                Broker.Cache.Add<List<WorkflowCheckPoint>>( TamamCacheClusters.Attendance , cacheKey , checkPoints );

                #endregion

                #endregion
            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<List<WorkflowCheckPoint>> AttendanceManualEditApprovalStepsGet(Guid targetId, RequestContext requestContext)
        {
            var context = new ExecutionContext<List<WorkflowCheckPoint>>();
            context.Execute(() =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "AttendanceHandler_AttendanceManualEditApprovalStepsGet" + targetId + requestContext;
                var cached = Broker.Cache.Get<List<WorkflowCheckPoint>>(TamamCacheClusters.Attendance, cacheKey);
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Security

                #endregion

                var historyItem = dataHandler.GetRawDataHistoryItem(targetId).Result;
                var instance = Broker.WorkflowEngine.GetInstance(targetId, manualEditWorkflowDefinition.Id);
                if (instance == null)
                {
                    context.Response.Set(ResponseState.NotFound, null);
                    return;
                }

                var checkPointsStack = instance.CPS;
                List<WorkflowCheckPoint> checkPoints;
                if ( checkPointsStack != null && checkPointsStack.Count > 0 )
                {
                    checkPoints = instance.CheckPoints;
                    checkPoints.AddRange( checkPointsStack.ToList() );
                }
                else
                {
                    checkPoints = instance.CheckPoints;
                    //checkPoints = instance.CheckPoints.Count >= instance.CheckPointsTemplates.Count ? instance.CheckPoints : instance.CheckPointsTemplates;
                }

                context.Response.Set( ResponseState.Success , checkPoints );

                #region Cache

                Broker.Cache.Add<List<WorkflowCheckPoint>>( TamamCacheClusters.Attendance , cacheKey , checkPoints );

                #endregion

                #endregion
            }, requestContext);
            return context.Response;
        }

        #region Helpers

        private bool CreateManualAttendanceInstance( AttendanceEventMetadata attendanceData , bool editMode , RequestContext requestContext , ExecutionContext<bool> context )
        {
            // create ...
            var attendanceDateTime = new DateTime( attendanceData.Date.Year , attendanceData.Date.Month , attendanceData.Date.Day , attendanceData.Time.Value.Hour , attendanceData.Time.Value.Minute , attendanceData.Time.Value.Second );
            var rawDataId = Guid.NewGuid();

            var rawData = new AttendanceRawData(rawDataId, attendanceDateTime, attendanceData.Mode, attendanceData.PersonId.Value, requestContext.Person.AuthenticationInfo.Username, attendanceData.Comment, false, true, editMode,true);

            // Add History Item..
            var historyId = Guid.NewGuid();
            var historyItem = new AttendanceRawDataHistoryItem( historyId , null , rawData.AttendanceDateTime , attendanceData.PersonId.Value , DateTime.Now , requestContext.Person.AuthenticationInfo.Username , attendanceData.Comment , ManualAttendanceStatus.Pending )
            {
                AttendanceRawDataId = rawDataId
            };
            rawData.AddHistoryItem( historyItem );

            var response_C = dataHandler.CreateAttendanceRaw( rawData );
            if ( response_C.Type != ResponseState.Success )
            {
                context.Response.Set( response_C.Type , false , response_C.MessageDetailed );
                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceCreateAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceCreateAuditMessageFailure , "" );
                return false;
            }

            #region Obselete

            // get updated instance (ORM sensitive)
            //var response_G = DataHandler.GetAttendanceRaw( response_C.Result );
            //if ( response_G.Type != ResponseState.Success )
            //{
            //    context.Response.Set( response_G.Type , false , response_G.MessageDetailed );
            //    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceCreateAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceCreateAuditMessageFailure , "" );
            //    return false;
            //}
            //rawData = response_G.Result;
            
            #endregion

            // Start Manual Edit WF ....
            if ( !Broker.WorkflowEngine.Initialize( historyItem , manualEditWorkflowDefinition ) )
            {
                context.Response.Set( ResponseState.SystemError , false );
                return false;
            }

            #region Obselete

            //var workFlowInstance = Broker.WorkflowEngine.GetInstance( rawData.Id , ManualEditWorkflowDefinition.Id );

            // TODO : need more analysis to how will History must behave...

            // persist instance Id inside HistoryItem..
            //var oldHistoryItem = DataHandler.GetRawDataHistoryItem( historyItem.Id ).Result;
            //oldHistoryItem.WorkflowInstanceId = workFlowInstance.Id;
            //var response_H = DataHandler.EditRawDataHistoryItem( oldHistoryItem );
            //if ( response_H.Type != ResponseState.Success )
            //{
            //    context.Response.Set( response_H.Type , false , response_H.MessageDetailed );
            //    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceCreateAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceCreateAuditMessageFailure , "" );
            //    return false;
            //}

            // Obselete ...
            // add history ...
            //rawData.AddHistoryItem( null , rawData.AttendanceDateTime , DateTime.Now , requestContext.Person.AuthenticationInfo.Username , attendanceData.Comment , rawData.ManualAttendanceStatus , workFlowInstance.Id );

            //var response_E = DataHandler.EditAttendanceRaw( rawData );
            //if ( response_E.Type != ResponseState.Success )
            //{
            //    context.Response.Set( response_E.Type , false , response_E.MessageDetailed );
            //    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceCreateAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceCreateAuditMessageFailure , "" );
            //    return false;
            //}
            
            #endregion

            return true;
        }

        #endregion

        #endregion
        #region Approval Workflow Support

        public ExecutionResponse<bool> ReviewAttendanceViolation( AttendanceViolationReview review , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError , false );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext , TamamConstants.AuthorizationConstants.ReviewAttendanceViolationActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.ReviewAttendanceViolationAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.ReviewLeaveAuditMessageFailure , review == null ? "" : review.ScheduleEventId.ToString() );
                    context.Response.Set( ResponseState.AccessDenied , false );
                    return;
                }

                #endregion

                // model
                var SE = dataHandler.GetScheduleEvent( review.ScheduleEventId ).Result;

                #region Validation


                #endregion
                #region workflow ...

                var attendanceApproval = new AttendanceApprovalWorkflowDefinition();
                var command = review.Status.ToString();
                var approvalData = new AttendanceJustificationWorkflowData()
                {
                    ScheduleEventId = review.ScheduleEventId.ToString() ,
                    PersonId = review.ReviewerId.ToString() ,
                    Metadata = review.Comment ,
                    Command = command.ToLower() ,
                };

                if ( !Broker.WorkflowEngine.Invoke( SE , attendanceApproval.Id , approvalData ) )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.ReviewAttendanceViolationAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.ReviewAttendanceViolationAuditMessageFailure , review == null ? "" : review.ScheduleEventId.ToString() );
                    context.Response.Set( ResponseState.SystemError , false );
                    return;
                }

                #endregion

                // audit ...
                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.ReviewAttendanceViolationAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.ReviewLeaveAuditMessageSuccessful , review == null ? "" : review.ScheduleEventId.ToString() );
                context.Response.Set( ResponseState.Success , true );

                #region Cache

                Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

                #endregion

                #endregion
            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> ReviewAttendanceManualEdit( AttendanceManualEditReview review , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError , false );
                    return;
                }

                // authorization ...
                // TODO : Add Review Attendance Manual Edit Action ...
                if ( !TamamServiceBroker.Authorize( requestContext , TamamConstants.AuthorizationConstants.ReviewAttendanceManualEditActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.ReviewAttendanceManualEditAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.ReviewAttendanceManualEditAuditMessageFailure , review == null ? "" : review.AttendanceRawDataHistoryId.ToString() );
                    context.Response.Set( ResponseState.AccessDenied , false );
                    return;
                }

                #endregion

                // model
                var historyItem = dataHandler.GetRawDataHistoryItem( review.AttendanceRawDataHistoryId ).Result;

                #region workflow ...

                var definition = new AttendanceManualEditApprovalWorkflowDefinition();
                var command = review.Status.ToString();
                var approvalData = new AttendanceManualEditReviewWorkflowData()
                {
                    TargetId = review.AttendanceRawDataHistoryId.ToString() ,
                    PersonId = review.ReviewerId.ToString() ,
                    Metadata = review.Comment ,
                    Command = command.ToLower() ,
                };

                if ( !Broker.WorkflowEngine.Invoke( historyItem , definition.Id , approvalData ) )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.ReviewAttendanceViolationAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.ReviewAttendanceViolationAuditMessageFailure , review == null ? "" : review.AttendanceRawDataHistoryId.ToString() );
                    context.Response.Set( ResponseState.SystemError , false );
                    return;
                }

                #endregion

                // audit ...
                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.ReviewAttendanceManualEditAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.ReviewAttendanceManualEditAuditMessageSuccessful , review == null ? "" : review.AttendanceRawDataHistoryId.ToString() );
                context.Response.Set( ResponseState.Success , true );

                #region Cache

                Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

                #endregion
                
                #endregion
            } , requestContext );

            return context.Response;
        }

        #endregion

        #endregion
        #region ISystemAttendanceHandler

        public ExecutionResponse<List<ScheduleEvent>> GetScheduleEventsDirty()
        {
            var context = new ExecutionContext<List<ScheduleEvent>>();
            context.Execute(() =>
            {
                #region Cache

                const string cacheKey = "AttendanceHandler_GetScheduleEventsDirty";
                var cached = Broker.Cache.Get<List<ScheduleEvent>>(TamamCacheClusters.Attendance, cacheKey);
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Data layer ...

                // data layer ...
                var dataHandlerResponse = dataHandler.GetDirtyScheduleEvents();

                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                Broker.Cache.Add<List<ScheduleEvent>>(TamamCacheClusters.Attendance, cacheKey, dataHandlerResponse.Result);

                #endregion
            });

            return context.Response;
        }

        public ExecutionResponse<List<ScheduleEvent>> GetScheduleEventsMissedOut()
        {
            var context = new ExecutionContext<List<ScheduleEvent>>();
            context.Execute(() =>
            {
                #region Cache

                const string cacheKey = "AttendanceHandler_GetScheduleEventsMissedOut";
                var cached = Broker.Cache.Get<List<ScheduleEvent>>(TamamCacheClusters.Attendance, cacheKey);
                if (cached != null)
                {
                    context.Response.Set(ResponseState.Success, cached);
                    return;
                }

                #endregion
                #region Data layer ...

                // data layer ...
                var dataHandlerResponse = dataHandler.GetScheduleEventsMissedOut();

                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                Broker.Cache.Add<List<ScheduleEvent>>(TamamCacheClusters.Attendance, cacheKey, dataHandlerResponse.Result);

                #endregion
            });

            return context.Response;
        }

        public ExecutionResponse<bool> DeleteScheduleEvents(Guid personId, DateTime date)
        {
            var context = new ExecutionContext<bool>();
            context.Execute(() =>
            {
                #region data layer ...

                var dataHandlerResponse = dataHandler.DeletePersonScheduleEvents(personId, date);
                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

                #endregion
            });
            return context.Response;
        }
        public ExecutionResponse<List<AttendanceRawData>> GetAttendanceRaw( Guid personId , DateTime date, bool considerAsAttendance )
        {
            var context = new ExecutionContext<List<AttendanceRawData>>();
            context.Execute(() =>
            {
                #region Cache

                var cacheKey = "AttendanceHandler_GetAttendanceRaw" + personId + date;
                var cached = Broker.Cache.Get<List<AttendanceRawData>>(TamamCacheClusters.Attendance, cacheKey);
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Data layer ...

                // data layer ...
                var dataHandlerResponse = dataHandler.GetAttendanceRaw( personId , date,considerAsAttendance );

                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                Broker.Cache.Add<List<AttendanceRawData>>(TamamCacheClusters.Attendance, cacheKey, dataHandlerResponse.Result);

                #endregion
            });

            return context.Response;
        }
        public ExecutionResponse<bool> ApprovalIntegrityMaintainByOwner( Guid ownerId )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                var criteria = new AttendanceFilters { Personnel = new List<Guid> { ownerId } , JustificationStatus = JustificationStatus.InProgress };
                var response = dataHandler.GetScheduleEvents( criteria , SystemSecurityContext.Instance );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( ResponseState.SystemError , false );
                    return;
                }
                var SEs = response.Result.Result;
                foreach ( var SE in SEs )
                {
                    if ( !Broker.WorkflowEngine.Maintain( SE , violationsWorkflowDefinition ) )
                    {
                        context.Response.Set( ResponseState.SystemError , false );
                        return;
                    }
                }

                context.Response.Set( ResponseState.Success , true );

                #region Cache

                Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

                #endregion
                
                #endregion
            } , SystemRequestContext.Instance );

            return context.Response;
        }
        public ExecutionResponse<ScheduleEvent> GetScheduleEvent( Guid id )
        {
            var context = new ExecutionContext<ScheduleEvent>();
            context.Execute(() =>
            {
                #region Cache

                var cacheKey = "AttendanceHandler_GetScheduleEvent" + id;
                var cached = Broker.Cache.Get<ScheduleEvent>(TamamCacheClusters.Attendance, cacheKey);
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Data layer ...

                var dataHandlerResponse = dataHandler.GetScheduleEventDetailed(id);
                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                Broker.Cache.Add<ScheduleEvent>(TamamCacheClusters.Attendance, cacheKey, dataHandlerResponse.Result);

                #endregion
            });
            return context.Response;
        }
        public ExecutionResponse<bool> DeleteAttendanceRawData( AttendanceRawData rawData )
        {
            var context = new ExecutionContext<bool>();
            context.Execute(() =>
            {
                #region data layer ...

                var dataHandlerResponse = dataHandler.DeleteAttendanceRaw(rawData);
                context.Response.Set(dataHandlerResponse);

                #endregion
                #region Cache

                Broker.Cache.Invalidate(TamamCacheClusters.Attendance);

                #endregion

            });
            return context.Response;
        }
        public ExecutionResponse<bool> CanPersonReviewManualEdit( Guid TargetId , Guid ReviewerId )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "AttendanceHandler_CanPersonReviewManualEdit" + TargetId.ToString() + ReviewerId.ToString();
                var cached = Broker.Cache.Get<bool?>( TamamCacheClusters.Attendance , cacheKey );
                if ( cached != null ) return context.Response.Set( ResponseState.Success , cached.Value );

                #endregion

                // approval instance ...
                var definition = new AttendanceManualEditApprovalWorkflowDefinition();
                var approvalInstance = Broker.WorkflowEngine.GetInstance( TargetId , definition.Id );
                if ( approvalInstance == null || approvalInstance.Status != WorkflowInstanceStatus.InProgress ) return context.Response.Set( ResponseState.NotFound , false );

                // pending step ...
                var currentStep = approvalInstance.CurrentStep;
                var currentAction = currentStep != null ? currentStep.Action : null;
                var currentData = currentAction != null ? currentAction.Data as AttendanceManualEditReviewWorkflowData : null;
                var expectedReviewer = currentData != null ? currentData.ApproverIdExpected : null;

                // error ...
                if ( expectedReviewer == null )
                {
                    #region Cache

                    Broker.Cache.Add<bool?>( TamamCacheClusters.Attendance , cacheKey , false );

                    #endregion
                    return context.Response.Set( ResponseState.Success , false );
                }

                // check if logged in person is the expected reviewer
                if ( ReviewerId == new Guid( expectedReviewer ) )
                {
                    #region Cache

                    Broker.Cache.Add<bool?>( TamamCacheClusters.Attendance , cacheKey , true );

                    #endregion
                    return context.Response.Set( ResponseState.Success , true );
                }

                //  check if logged in person is a delegate for expected reviewer
                var canActResponse = TamamServiceBroker.PersonnelHandler.CanActFor( ReviewerId , new Guid( expectedReviewer ) , SystemRequestContext.Instance );
                if ( canActResponse.Type != ResponseState.Success )
                {
                    return context.Response.Set( ResponseState.Failure , false );
                }

                #region Cache

                Broker.Cache.Add<bool?>( TamamCacheClusters.Attendance , cacheKey , canActResponse.Result );

                #endregion
                return context.Response.Set( ResponseState.Success , canActResponse.Result );

            } , SystemRequestContext.Instance );
            return context.Response;
        }

        #endregion
        #region IReadOnlyAttendanceHandler

        public ExecutionResponse<List<ScheduleEventCurrentlyInDTO>> GetScheduleEventCurrentlyIn( AttendanceFilters filters , RequestContext requestContext )
        {
            var context = new ExecutionContext<List<ScheduleEventCurrentlyInDTO>>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "AttendanceHandler_GetScheduleEventCurrentlyIn" + filters + requestContext;
                var cached = Broker.Cache.Get<List<ScheduleEventCurrentlyInDTO>>( TamamCacheClusters.Attendance , cacheKey );
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
                                        actionId: TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId ,
                                        actionKey: TamamConstants.AuthorizationConstants.AttendanceGetActionKey ,
                                        messageForDenied: TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure ,
                                        messageForFailure: TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure ,
                                        messageForSuccess: TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageSuccessful
                                        );

                var response = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type , null );
                    return;
                }

                var securityContext = response.Result;
                #endregion
                #region validation ...

                if ( filters == null )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure , "" );
                    context.Response.Set( ResponseState.ValidationError , null , new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region data layer ...

                // data layer ...
                var dataHandlerResponse = dataHandler.GetScheduleEventsCurrentlyIn( filters , securityContext );
                if ( dataHandlerResponse.Result == null )
                {
                    context.Response.Set( dataHandlerResponse );
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure , "" );
                    return;
                }

                #endregion
                #region audit ...

                // audit ...
                //TamamServiceBroker.Audit ( requestContext , TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageSuccessful , "" );

                #endregion

                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Add<List<ScheduleEventCurrentlyInDTO>>( TamamCacheClusters.Attendance , cacheKey , dataHandlerResponse.Result );

                #endregion

            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<List<ScheduleEventHighLightDTO>> GetScheduleEventsHighLight( AttendanceFilters filters , RequestContext requestContext )
        {
            var context = new ExecutionContext<List<ScheduleEventHighLightDTO>>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "AttendanceHandler_GetScheduleEventsHighLight" + filters + requestContext;
                var cached = Broker.Cache.Get<List<ScheduleEventHighLightDTO>>( TamamCacheClusters.Attendance , cacheKey );
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
                                        actionId: TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId ,
                                        actionKey: TamamConstants.AuthorizationConstants.AttendanceGetActionKey ,
                                        messageForDenied: TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure ,
                                        messageForFailure: TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure ,
                                        messageForSuccess: TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageSuccessful
                                        );

                var response = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type , null );
                    return;
                }

                var securityContext = response.Result;
                #endregion
                #region validation ...

                if ( filters == null )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure , "" );
                    context.Response.Set( ResponseState.ValidationError , null , new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region data layer ...

                // data layer ...
                var dataHandlerResponse = dataHandler.GetScheduleEventsHighLight( filters , securityContext );
                if ( dataHandlerResponse.Result == null )
                {
                    context.Response.Set( dataHandlerResponse );
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId , TamamConstants.AuthorizationConstants.AttendanceAuditModuleId , TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure , "" );
                    return;
                }

                #endregion
                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Add<List<ScheduleEventHighLightDTO>>( TamamCacheClusters.Attendance , cacheKey , dataHandlerResponse.Result );

                #endregion

            } , requestContext );

            return context.Response;
        }

        public ExecutionResponse<List<PersonnelPrivilege>> GetPersonnelPrivileges(AttendanceFilters filters, RequestContext requestContext)
        {
            var context = new ExecutionContext<List<PersonnelPrivilege>>();
            context.Execute(() =>
            {
                #region Cache

                var cacheKey = "AttendanceHandler_GetPersonnelPrivileges" + filters + requestContext;
                var cached = Broker.Cache.Get<List<PersonnelPrivilege>>(TamamCacheClusters.Attendance, cacheKey);
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
                                        actionId: TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId,
                                        actionKey: TamamConstants.AuthorizationConstants.AttendanceGetActionKey,
                                        messageForDenied: TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure,
                                        messageForFailure: TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure,
                                        messageForSuccess: TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageSuccessful
                                        );

                var response = TamamServiceBroker.Secure(context.ActionContext, requestContext);
                if (response.Type != ResponseState.Success)
                {
                    context.Response.Set(response.Type, null);
                    return;
                }

                var securityContext = response.Result;
                #endregion
                #region validation ...

                if (filters == null)
                {
                    TamamServiceBroker.Audit(requestContext, TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId, TamamConstants.AuthorizationConstants.AttendanceAuditModuleId, TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure, "");
                    context.Response.Set(ResponseState.ValidationError, null, new List<ModelMetaPair>());
                    return;
                }

                #endregion
                #region data layer ...

                // data layer ...
                var dataHandlerResponse = dataHandler.GetPersonnelPrivileges(filters, securityContext);
                if (dataHandlerResponse.Result == null)
                {
                    context.Response.Set(dataHandlerResponse);
                    TamamServiceBroker.Audit(requestContext, TamamConstants.AuthorizationConstants.AttendanceGetAuditActionId, TamamConstants.AuthorizationConstants.AttendanceAuditModuleId, TamamConstants.AuthorizationConstants.AttendanceGetAuditMessageFailure, "");
                    return;
                }

                #endregion
                context.Response.Set(dataHandlerResponse);

                #region Cache

                Broker.Cache.Add<List<PersonnelPrivilege>>(TamamCacheClusters.Attendance, cacheKey, dataHandlerResponse.Result);

                #endregion

            }, requestContext);

            return context.Response;
        }

        #endregion
    }
}