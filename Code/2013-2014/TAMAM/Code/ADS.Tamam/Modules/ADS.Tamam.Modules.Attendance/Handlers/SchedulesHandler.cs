using System;
using System.Linq;
using System.Collections.Generic;

using LinqKit;

using ADS.Common.Context;
using ADS.Common.Handlers;
using ADS.Common.Utilities;
using ADS.Common.Validation;
using ADS.Tamam.Common.Data;
using XCore.Framework.Utilities;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.DTO;
using ADS.Tamam.Common.Data.Contracts;
using ADS.Common.Bases.Events.Handlers;
using ADS.Tamam.Common.Data.Validation;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Modules.Attendance.Events;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Schedules;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;

namespace ADS.Tamam.Modules.Attendance.Handlers
{
    public class SchedulesHandler : ISchedulesHandler, ISystemSchedulesHandler, IReadOnlySchedulesHandler
    {
        #region props.

        public bool Initialized { get; private set; }
        public string Name { get { return "SchedulesHandler"; } }

        private readonly SchedulesDataHandler dataHandler;

        #endregion
        #region cst.

        public SchedulesHandler()
        {
            #region LOG
            XLogger.Info( "Initializing ..." );
            #endregion

            if ( !TamamDataBroker.Initialized )
            {
                #region LOG
                XLogger.Error( "Initialization Failed, underlying handlers are not registered or initilaized successfully." );
                #endregion
                return;
            }

            this.dataHandler = TamamDataBroker.GetRegisteredDataLayer<SchedulesDataHandler>( TamamConstants.SchedulesDataHandlerName );
            if ( this.dataHandler == null || !this.dataHandler.Initialized )
            {
                #region LOG
                XLogger.Error( "Initialization Failed, underlying handlers are not registered or initilaized successfully." );
                #endregion
                Initialized = false;
            }

            #region LOG
            XLogger.Info( "SchedulesHandler ... Initialized" );
            #endregion

            Initialized = true;
        }

        #endregion

        #region ISchedulesHandler

        #region Shifts

        public ExecutionResponse<Guid> CreateShift( Shift shift , RequestContext requestContext )
        {
            var context = new ExecutionContext<Guid>();
            context.Execute( () =>
            {
                #region ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError , Guid.Empty , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext , TamamConstants.AuthorizationConstants.CreateShiftActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.CreateShiftAuditActionId , TamamConstants.AuthorizationConstants.ShiftAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.CreateShiftAuditMessageFailure , shift.Name ) , string.Empty );
                    context.Response.Set( ResponseState.AccessDenied , Guid.Empty );
                    return;
                }

                #endregion
                #region validation

                var validator = new ShiftValidator( shift , TamamConstants.ValidationMode.Create );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.CreateShiftAuditActionId , TamamConstants.AuthorizationConstants.ShiftAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.CreateShiftAuditMessageFailure , shift.Name ) , string.Empty );
                    context.Response.Set( ResponseState.ValidationError , Guid.Empty , validator.ErrorsDetailed );
                    return;
                }

                #endregion
                #region data layer

                var dataHandlerResponse = dataHandler.CreateShift( shift );

                // audit ...
                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.CreateShiftAuditActionId , TamamConstants.AuthorizationConstants.ShiftAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.CreateShiftAuditMessageSuccessful , shift.Name ) , string.Empty );

                #endregion

                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Schedules );

                #endregion

                #endregion

            } , requestContext );
            return context.Response;
        }
        public ExecutionResponse<bool> EditShift( Shift shift , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError , false , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext ,
                        TamamConstants.AuthorizationConstants.EditShiftActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.EditShiftAuditActionId ,
                        TamamConstants.AuthorizationConstants.ShiftAuditModuleId ,
                        string.Format( TamamConstants.AuthorizationConstants.EditShiftAuditMessageFailure , shift.Name ) ,
                        string.Empty );
                    context.Response.Set( ResponseState.AccessDenied , false , new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region Validation

                // validation ...
                var validator = new ShiftValidator( shift , TamamConstants.ValidationMode.Edit );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditShiftAuditActionId , TamamConstants.AuthorizationConstants.ShiftAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.EditShiftAuditMessageFailure , shift.Name ) , string.Empty );
                    context.Response.Set( ResponseState.ValidationError , false , validator.ErrorsDetailed );
                    return;
                }

                # endregion
                #region DataLayer

                var dataHandlerResponse = dataHandler.EditShift( shift );
                if ( !dataHandlerResponse.Result )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditShiftAuditActionId , TamamConstants.AuthorizationConstants.ShiftAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.EditShiftAuditMessageFailure , shift.Name ) , string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                # endregion

                var state = EventsBroker.Handle( new ShiftEditEvent( shift.Id ) );

                // audit trail ...
                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditShiftAuditActionId , TamamConstants.AuthorizationConstants.ShiftAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.EditShiftAuditMessageSuccessful , shift.Name ) , string.Empty );

                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Schedules );

                #endregion

                #endregion
            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> DeleteShift( Guid shiftId , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError , false , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext ,
                        TamamConstants.AuthorizationConstants.DeleteShiftActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.DeleteShiftAuditActionId ,
                        TamamConstants.AuthorizationConstants.ShiftAuditModuleId ,
                         string.Format( TamamConstants.AuthorizationConstants.DeleteShiftAuditMessageFailure , shiftId ) , string.Empty );
                    context.Response.Set( ResponseState.AccessDenied , false , new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // validation ...
                var shift = dataHandler.GetShift( shiftId );
                var validator = new ShiftValidator( shift.Result , TamamConstants.ValidationMode.Delete );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.DeleteShiftAuditActionId , TamamConstants.AuthorizationConstants.ShiftAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.DeleteShiftAuditMessageFailure , shiftId ) , string.Empty );
                    context.Response.Set( ResponseState.ValidationError , false , validator.ErrorsDetailed );
                    return;
                }

                // data layer ...
                var dataHandlerResponse = dataHandler.DeleteShift( shiftId );
                if ( !dataHandlerResponse.Result )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.DeleteShiftAuditActionId , TamamConstants.AuthorizationConstants.ShiftAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.DeleteShiftAuditMessageFailure , shiftId ) , string.Empty ); context.Response.Set( dataHandlerResponse );
                    return;
                }

                // audit trail ...
                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.DeleteShiftAuditActionId , TamamConstants.AuthorizationConstants.ShiftAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.DeleteShiftAuditMessageSuccessful , shiftId ) , string.Empty );

                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Schedules );

                #endregion

                #endregion
            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<Shift> GetShift( Guid shiftId , RequestContext requestContext )
        {
            var context = new ExecutionContext<Shift>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "SchedulesHandler_GetShift" + shiftId + requestContext;
                if ( !requestContext.IgnoreCache )
                {
                    var cached = Broker.Cache.Get<Shift>( TamamCacheClusters.Schedules , cacheKey );
                    if ( cached != null )
                    {
                        context.Response.Set( ResponseState.Success , cached );
                        return;
                    }
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
                if (
                    !TamamServiceBroker.Authorize( requestContext ,
                        TamamConstants.AuthorizationConstants.GetShiftActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.GetShiftAuditActionId ,
                        TamamConstants.AuthorizationConstants.ShiftAuditModuleId ,
                        TamamConstants.AuthorizationConstants.GetShiftAuditMessageFailure , string.Empty );
                    context.Response.Set( ResponseState.AccessDenied , null , new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.GetShift( shiftId );
                if ( dataHandlerResponse.Result == null )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.GetShiftAuditActionId , TamamConstants.AuthorizationConstants.ShiftAuditModuleId , TamamConstants.AuthorizationConstants.GetShiftAuditMessageFailure , string.Empty ); context.Response.Set( dataHandlerResponse );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                #region Cache

                if ( !requestContext.IgnoreCache )
                {
                    Broker.Cache.Add<Shift>( TamamCacheClusters.Schedules , cacheKey , dataHandlerResponse.Result );
                }

                #endregion

                #endregion
            } , requestContext );
            return context.Response;
        }
        public ExecutionResponse<List<Shift>> GetShifts( bool? isActive , RequestContext requestContext )
        {
            var context = new ExecutionContext<List<Shift>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "SchedulesHandler_GetShifts" + isActive + requestContext;
                var cached = Broker.Cache.Get<List<Shift>>( TamamCacheClusters.Schedules , cacheKey );
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
                if (
                    !TamamServiceBroker.Authorize( requestContext ,
                        TamamConstants.AuthorizationConstants.GetShiftActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.GetShiftAuditActionId ,
                        TamamConstants.AuthorizationConstants.ShiftAuditModuleId ,
                        TamamConstants.AuthorizationConstants.GetShiftAuditMessageFailure , string.Empty );
                    context.Response.Set( ResponseState.AccessDenied , null , new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.GetShifts( isActive );
                if ( dataHandlerResponse.Result == null )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.GetShiftAuditActionId , TamamConstants.AuthorizationConstants.ShiftAuditModuleId , TamamConstants.AuthorizationConstants.GetShiftAuditMessageFailure , string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Add<List<Shift>>( TamamCacheClusters.Schedules , cacheKey , dataHandlerResponse.Result );

                #endregion

                #endregion
            } , requestContext );
            return context.Response;
        }
        public ExecutionResponse<bool> ChangeShiftStatus( Guid shiftId , bool isActive , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError , false , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext ,
                        TamamConstants.AuthorizationConstants.EditShiftActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.EditShiftAuditActionId ,
                        TamamConstants.AuthorizationConstants.ShiftAuditModuleId ,
                        string.Format( TamamConstants.AuthorizationConstants.EditShiftStatusAuditMessageFailure ,
                            shiftId.ToString() ) , string.Empty );
                    context.Response.Set( ResponseState.AccessDenied , false , new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                if ( !isActive )
                {
                    // validation ...
                    var shift = dataHandler.GetShift( shiftId );
                    var validator = new ShiftValidator( shift.Result , TamamConstants.ValidationMode.Deactivate );
                    if ( validator.IsValid != null && !validator.IsValid.Value )
                    {
                        TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditShiftAuditActionId , TamamConstants.AuthorizationConstants.ShiftAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.EditShiftStatusAuditMessageFailure , shiftId ) , string.Empty );
                        context.Response.Set( ResponseState.ValidationError , false , validator.ErrorsDetailed );
                        return;
                    }
                }

                // data layer ...
                var dataHandlerResponse = dataHandler.ChangeShiftStatus( shiftId , isActive );
                if ( !dataHandlerResponse.Result )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditShiftAuditActionId , TamamConstants.AuthorizationConstants.ShiftAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.EditShiftStatusAuditMessageFailure , shiftId ) , string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                // audit ...
                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditShiftAuditActionId , TamamConstants.AuthorizationConstants.ShiftAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.EditShiftStatusAuditMessageSuccessful , shiftId , isActive ? "Active" : "Not Active" ) , string.Empty );

                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Schedules );

                #endregion

                #endregion
            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> SoftDeleteShift( Guid shiftId , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError , false , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext ,
                        TamamConstants.AuthorizationConstants.DeleteShiftActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.DeleteShiftAuditActionId ,
                        TamamConstants.AuthorizationConstants.ShiftAuditModuleId ,
                        string.Format( TamamConstants.AuthorizationConstants.DeleteShiftAuditMessageFailure , shiftId ) , string.Empty );
                    context.Response.Set( ResponseState.AccessDenied , false , new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region Change status to be deactivated
                //Call Change status with false as active status, ChangeShiftStatus will validate
                var dataHandlerResponse = ChangeShiftStatus( shiftId , false , requestContext );
                if ( !dataHandlerResponse.Result )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditShiftAuditActionId , TamamConstants.AuthorizationConstants.ShiftAuditModuleId ,
                        string.Format( TamamConstants.AuthorizationConstants.DeleteShiftAuditMessageFailure , shiftId ) , string.Empty );
                    context.Response.Set( dataHandlerResponse.Type , false , dataHandlerResponse.MessageDetailed );
                    return;
                }
                #endregion
                #region Cahnge Name by adding "D" and 4 random digits

                var shift = GetShift( shiftId , requestContext ).Result;
                Random random = new Random();
                var randameNo = random.Next( 0 , 999 );
                shift.Name = shift.Name + " D" + randameNo;
                shift.NameCultureVarient = shift.NameCultureVarient + "D" + randameNo;
                var editResponse = dataHandler.EditShift( shift );
                if ( editResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.DeleteShiftAuditActionId ,
                        TamamConstants.AuthorizationConstants.ShiftAuditModuleId ,
                        string.Format( TamamConstants.AuthorizationConstants.DeleteShiftAuditMessageFailure , shiftId ) , string.Empty );
                    context.Response.Set( editResponse.Type , false );
                    return;
                }

                #endregion
                // audit ...
                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.DeleteShiftAuditActionId , TamamConstants.AuthorizationConstants.ShiftAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.DeleteShiftAuditMessageSuccessful , shiftId ) , string.Empty );

                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Schedules );

                #endregion

                #endregion
            } , requestContext );
            return context.Response;
        }

        #endregion
        #region ScheduleTemplates

        public ExecutionResponse<List<ScheduleTemplate>> GetScheduleTemplates( bool? isActive , RequestContext requestContext )
        {
            var context = new ExecutionContext<List<ScheduleTemplate>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "SchedulesHandler_GetScheduleTemplates" + isActive + requestContext;
                var cached = Broker.Cache.Get<List<ScheduleTemplate>>( TamamCacheClusters.Schedules , cacheKey );
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
                if (
                    !TamamServiceBroker.Authorize( requestContext ,
                        TamamConstants.AuthorizationConstants.GetScheduleTemplatesActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.GetScheduleTemplatesAuditActionId ,
                        TamamConstants.AuthorizationConstants.ScheduleTemplatesAuditModuleId ,
                        TamamConstants.AuthorizationConstants.GetScheduleTemplatesAuditMessageFailure , string.Empty );
                    context.Response.Set( ResponseState.AccessDenied , null , new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.GetScheduleTemplates( isActive );
                if ( dataHandlerResponse.Result == null )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.GetScheduleTemplatesAuditActionId , TamamConstants.AuthorizationConstants.ScheduleTemplatesAuditModuleId , TamamConstants.AuthorizationConstants.GetScheduleTemplatesAuditMessageFailure , string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                // audit trail ...
                //TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.GetScheduleTemplatesAuditActionId , TamamConstants.AuthorizationConstants.ScheduleTemplatesAuditModuleId , TamamConstants.AuthorizationConstants.GetScheduleTemplatesAuditMessageSuccessful , string.Empty );

                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Add<List<ScheduleTemplate>>( TamamCacheClusters.Schedules , cacheKey , dataHandlerResponse.Result );

                #endregion

                #endregion
            } , requestContext );
            return context.Response;
        }
        public ExecutionResponse<ScheduleTemplate> GetScheduleTemplate( Guid templateId , RequestContext requestContext )
        {
            var context = new ExecutionContext<ScheduleTemplate>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "SchedulesHandler_GetScheduleTemplate" + templateId + requestContext;
                if ( !requestContext.IgnoreCache )
                {
                    var cached = Broker.Cache.Get<ScheduleTemplate>( TamamCacheClusters.Schedules , cacheKey );
                    if ( cached != null )
                    {
                        context.Response.Set( ResponseState.Success , cached );
                        return;
                    }
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
                if ( !TamamServiceBroker.Authorize( requestContext , TamamConstants.AuthorizationConstants.GetScheduleTemplatesActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.GetScheduleTemplatesAuditActionId , TamamConstants.AuthorizationConstants.ScheduleTemplatesAuditModuleId , TamamConstants.AuthorizationConstants.GetScheduleTemplatesAuditMessageFailure , string.Empty );
                    context.Response.Set( ResponseState.AccessDenied , null , new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.GetScheduleTemplate( templateId );
                if ( dataHandlerResponse.Result == null )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.GetScheduleTemplatesAuditActionId , TamamConstants.AuthorizationConstants.ScheduleTemplatesAuditModuleId , TamamConstants.AuthorizationConstants.GetScheduleTemplatesAuditMessageFailure , string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                #region Cache

                if ( !requestContext.IgnoreCache )
                {
                    Broker.Cache.Add<ScheduleTemplate>( TamamCacheClusters.Schedules , cacheKey , dataHandlerResponse.Result );
                }

                #endregion

                #endregion
            } , requestContext );
            return context.Response;
        }
        public ExecutionResponse<Guid> CreateScheduleTemplate( ScheduleTemplate scheduleTemplate , RequestContext requestContext )
        {
            var context = new ExecutionContext<Guid>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError , Guid.Empty , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext ,
                        TamamConstants.AuthorizationConstants.CreateScheduleTemplatesActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.CreateScheduleTemplatesAuditActionId ,
                        TamamConstants.AuthorizationConstants.ScheduleTemplatesAuditModuleId ,
                        string.Format( TamamConstants.AuthorizationConstants.CreateScheduleTemplatesAuditMessageFailure ,
                            scheduleTemplate.Name ) , string.Empty );
                    context.Response.Set( ResponseState.AccessDenied , Guid.Empty , new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // validation ...
                var validator = new ScheduleTemplateValidator( scheduleTemplate , TamamConstants.ValidationMode.Create );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.CreateScheduleTemplatesAuditActionId , TamamConstants.AuthorizationConstants.ScheduleTemplatesAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.CreateScheduleTemplatesAuditMessageFailure , scheduleTemplate.Name ) , string.Empty );
                    context.Response.Set( ResponseState.ValidationError , Guid.Empty , validator.ErrorsDetailed );
                    return;
                }

                // data layer ...
                var dataHandlerResponse = dataHandler.CreateScheduleTemplate( scheduleTemplate );
                if ( dataHandlerResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.CreateScheduleTemplatesAuditActionId , TamamConstants.AuthorizationConstants.ScheduleTemplatesAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.CreateScheduleTemplatesAuditMessageFailure , scheduleTemplate.Name ) , string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                // audit ...
                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.CreateScheduleTemplatesAuditActionId , TamamConstants.AuthorizationConstants.ScheduleTemplatesAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.CreateScheduleTemplatesAuditMessageSuccessful , scheduleTemplate.Name ) , string.Empty );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Schedules );

                #endregion

                #endregion
            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> EditScheduleTemplate( ScheduleTemplate scheduleTemplate , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError , false , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext ,
                        TamamConstants.AuthorizationConstants.EditScheduleTemplatesActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.EditScheduleTemplatesAuditActionId ,
                        TamamConstants.AuthorizationConstants.ScheduleTemplatesAuditModuleId ,
                        string.Format( TamamConstants.AuthorizationConstants.EditScheduleTemplatesAuditMessageFailure ,
                            scheduleTemplate.Name ) , string.Empty );
                    context.Response.Set( ResponseState.AccessDenied , false , new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region Validation

                var validator = new ScheduleTemplateValidator( scheduleTemplate , TamamConstants.ValidationMode.Edit );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.EditScheduleTemplatesAuditActionId ,
                        TamamConstants.AuthorizationConstants.ScheduleTemplatesAuditModuleId ,
                        string.Format( TamamConstants.AuthorizationConstants.EditScheduleTemplatesAuditMessageFailure ,
                            scheduleTemplate.Name ) , string.Empty );
                    context.Response.Set( ResponseState.ValidationError , false , validator.ErrorsDetailed );
                    return;
                }

                #endregion
                # region Data Layer

                var dataHandlerResponse = dataHandler.EditScheduleTemplate( scheduleTemplate );
                if ( !dataHandlerResponse.Result )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditScheduleTemplatesAuditActionId , TamamConstants.AuthorizationConstants.ScheduleTemplatesAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.EditScheduleTemplatesAuditMessageFailure , scheduleTemplate.Name ) , string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                # endregion

                var state = EventsBroker.Handle( new ScheduleTemplateEditEvent( scheduleTemplate.Id ) );

                // audit ...
                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditScheduleTemplatesAuditActionId , TamamConstants.AuthorizationConstants.ScheduleTemplatesAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.EditScheduleTemplatesAuditMessageSuccessful , scheduleTemplate.Name ) , string.Empty );

                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Schedules );

                #endregion

                #endregion
            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> ChangeScheduleTemplateStatus( Guid templateId , bool isActive , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError , false , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext ,
                        TamamConstants.AuthorizationConstants.EditScheduleTemplatesActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.EditScheduleTemplatesAuditActionId ,
                        TamamConstants.AuthorizationConstants.ScheduleTemplatesAuditModuleId ,
                        string.Format( TamamConstants.AuthorizationConstants.EditScheduleTemplatesAuditMessageFailure ,
                            templateId.ToString() ) , string.Empty );
                    context.Response.Set( ResponseState.AccessDenied , false , new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                if ( !isActive )
                {
                    // validation ...
                    var template = dataHandler.GetScheduleTemplate( templateId );
                    var validator = new ScheduleTemplateValidator( template.Result ,
                        TamamConstants.ValidationMode.Deactivate );
                    if ( validator.IsValid != null && !validator.IsValid.Value )
                    {
                        TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditScheduleTemplatesAuditActionId , TamamConstants.AuthorizationConstants.ScheduleTemplatesAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.EditScheduleTemplatesAuditMessageFailure , templateId ) , string.Empty );
                        context.Response.Set( ResponseState.ValidationError , false , validator.ErrorsDetailed );
                        return;
                    }
                }

                // data layer ...
                var dataHandlerResponse = dataHandler.ChangeScheduleTemplateStatus( templateId , isActive );
                if ( !dataHandlerResponse.Result )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditScheduleTemplatesAuditActionId , TamamConstants.AuthorizationConstants.ScheduleTemplatesAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.EditScheduleTemplatesAuditMessageFailure , templateId ) , string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                // audit ...
                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditScheduleTemplatesAuditActionId , TamamConstants.AuthorizationConstants.ScheduleTemplatesAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.EditScheduleTemplatesAuditMessageSuccessful , templateId ) , string.Empty );

                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Schedules );

                #endregion

                #endregion
            } , requestContext );

            return context.Response;
        }

        #endregion
        #region Schedule

        public ExecutionResponse<Guid> CreateSchedule( Schedule schedule , RequestContext requestContext )
        {
            var context = new ExecutionContext<Guid>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError , Guid.Empty , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext ,
                        TamamConstants.AuthorizationConstants.CreateScheduleActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.CreateScheduleAuditActionId ,
                        TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                        string.Format( TamamConstants.AuthorizationConstants.CreateScheduleAuditMessageFailure ,
                            schedule.Id ) , schedule.Name );
                    context.Response.Set( ResponseState.AccessDenied , Guid.Empty , new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region Handle Null inner list

                if ( schedule.SchedulePersonnel == null ) schedule.SchedulePersonnel = new List<SchedulePerson>();
                if ( schedule.ScheduleDepartments == null ) schedule.ScheduleDepartments = new List<ScheduleDepartment>();

                #endregion
                #region Validation

                var validator = new ScheduleValidator( schedule , TamamConstants.ValidationMode.Create );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.CreateScheduleAuditActionId ,
                        TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                        string.Format( TamamConstants.AuthorizationConstants.CreateScheduleAuditMessageFailure ,
                            schedule.Id ) , schedule.Name );
                    context.Response.Set( ResponseState.ValidationError , Guid.Empty , validator.ErrorsDetailed );
                    return;
                }

                #endregion
                #region data layer

                var dataHandlerResponse = dataHandler.CreateSchedule( schedule );
                if ( dataHandlerResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.CreateScheduleAuditActionId , TamamConstants.AuthorizationConstants.ScheduleAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.CreateScheduleAuditMessageFailure , schedule.Id ) , schedule.Name );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }
                var scheduleId = dataHandlerResponse.Result;

                #endregion

                var state = EventsBroker.Handle( new ScheduleCreateEvent( scheduleId ) );

                #region audit trail

                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.CreateScheduleAuditActionId , TamamConstants.AuthorizationConstants.ScheduleAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.CreateScheduleAuditMessageSuccessful , schedule.Name ) , schedule.Name );

                #endregion

                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Schedules );

                #endregion

                #endregion
            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<Guid> EditSchedule( Schedule schedule , RequestContext requestContext )
        {
            var context = new ExecutionContext<Guid>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                    actionId: TamamConstants.AuthorizationConstants.EditScheduleAuditActionId ,
                    actionKey: TamamConstants.AuthorizationConstants.EditScheduleActionKey ,
                    messageForDenied:
                        string.Format( TamamConstants.AuthorizationConstants.EditScheduleAuditMessageFailure , schedule.Id ) ,
                    messageForFailure:
                        string.Format( TamamConstants.AuthorizationConstants.EditScheduleAuditMessageFailure , schedule.Id ) ,
                    messageForSuccess:
                        string.Format( TamamConstants.AuthorizationConstants.EditScheduleAuditMessageSuccessful ,
                            schedule.Id )
                    );

                var securityResponse = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                if ( securityResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( securityResponse.Type , Guid.Empty );
                    return;
                }

                var securityContext = securityResponse.Result;

                #endregion
                #region Update Model ...

                if ( schedule.SchedulePersonnel == null ) schedule.SchedulePersonnel = new List<SchedulePerson>();
                if ( schedule.ScheduleDepartments == null ) schedule.ScheduleDepartments = new List<ScheduleDepartment>();

                #endregion
                #region Validation

                var validator = new ScheduleValidator( schedule , TamamConstants.ValidationMode.Edit );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditScheduleAuditActionId , TamamConstants.AuthorizationConstants.ScheduleAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.EditScheduleAuditMessageFailure , schedule.Id ) , schedule.Id.ToString() );
                    context.Response.Set( ResponseState.ValidationError , Guid.Empty , validator.ErrorsDetailed );
                    return;
                }

                #endregion

                #region Get Old Schedule Departments ...

                var oldScheduleDepartmentsResponse = dataHandler.GetEffectiveScheduleDepartments_ScheduleId( schedule.Id );
                if ( oldScheduleDepartmentsResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditScheduleAuditActionId , TamamConstants.AuthorizationConstants.ScheduleAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.EditScheduleAuditMessageFailure , schedule.Id ) , schedule.Id.ToString() );
                    context.Response.Set( ResponseState.SystemError , Guid.Empty );
                    return;
                }
                var oldScheduleDepartments = oldScheduleDepartmentsResponse.Result.Select( esd => esd.Department ).ToList();

                #endregion
                #region Get Old Schedule People ...

                var oldSchedulePeopleIdsResponse = dataHandler.GetEffectiveSchedulePersonnel_ScheduleId( schedule.Id , SystemSecurityContext.Instance );
                if ( oldSchedulePeopleIdsResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditScheduleAuditActionId , TamamConstants.AuthorizationConstants.ScheduleAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.EditScheduleAuditMessageFailure , schedule.Id ) , schedule.Id.ToString() );
                    context.Response.Set( ResponseState.SystemError , Guid.Empty );
                    return;
                }
                var oldSchedulePeople = oldSchedulePeopleIdsResponse.Result.Select( esp => esp.Person ).ToList();

                #endregion

                #region data layer ...

                var result = dataHandler.EditSchedule( schedule , securityContext );
                if ( result.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditScheduleAuditActionId , TamamConstants.AuthorizationConstants.ScheduleAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.EditScheduleAuditMessageFailure , schedule.Id ) , schedule.Id.ToString() );
                    context.Response.Set( result );
                    return;
                }

                #endregion

                var state = EventsBroker.Handle( new ScheduleEditEvent( schedule.Id , oldScheduleDepartments , oldSchedulePeople ) );

                # endregion

                #region audit trail ...

                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditScheduleAuditActionId , TamamConstants.AuthorizationConstants.ScheduleAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.EditScheduleAuditMessageSuccessful , schedule.Id ) , schedule.Id.ToString() );

                #endregion

                context.Response.Set( result );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Schedules );

                #endregion

            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> ChangeScheduleStatus( Guid scheduleId , bool status , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                var schedule = GetSchedule( scheduleId , requestContext ).Result;

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError , false , new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext ,
                        TamamConstants.AuthorizationConstants.DeleteScheduleActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.EditScheduleAuditActionId ,
                        TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                        string.Format( TamamConstants.AuthorizationConstants.EditScheduleStatusAuditMessageFailure ,
                            schedule.Id ) , string.Empty );
                    context.Response.Set( ResponseState.AccessDenied , false , new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region Validation

                var validator = new ScheduleValidator( schedule , TamamConstants.ValidationMode.Deactivate );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.EditScheduleAuditActionId ,
                        TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                        string.Format( TamamConstants.AuthorizationConstants.EditScheduleStatusAuditMessageFailure ,
                            schedule.Id ) , string.Empty );
                    context.Response.Set( ResponseState.ValidationError , false , validator.ErrorsDetailed );
                    return;
                }

                #endregion
                #region Get Old Schedule Departments ...

                var oldScheduleDepartmentsResponse = dataHandler.GetEffectiveScheduleDepartments_ScheduleId( schedule.Id );
                if ( oldScheduleDepartmentsResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.EditScheduleAuditActionId ,
                        TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                        string.Format( TamamConstants.AuthorizationConstants.EditScheduleAuditMessageFailure , schedule.Id ) ,
                        schedule.Id.ToString() );
                    context.Response.Set( ResponseState.SystemError , false );
                    return;
                }
                var oldScheduleDepartments = oldScheduleDepartmentsResponse.Result.Select( esd => esd.Department ).ToList();

                #endregion
                #region Get Old Schedule People ...

                var schedulePeopleResponse = dataHandler.GetEffectiveSchedulePersonnel_ScheduleId( schedule.Id , SystemSecurityContext.Instance );
                if ( schedulePeopleResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.EditScheduleAuditActionId ,
                        TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                        string.Format( TamamConstants.AuthorizationConstants.EditScheduleAuditMessageFailure , schedule.Id ) ,
                        schedule.Id.ToString() );
                    context.Response.Set( ResponseState.SystemError , status );
                    return;
                }
                var schedulePeople = schedulePeopleResponse.Result.Select( esp => esp.Person ).ToList();

                #endregion

                #region Data Layer ...

                var dataHandlerResponse = dataHandler.ChangeScheduleStatus( schedule , status );
                if ( !dataHandlerResponse.Result )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditScheduleAuditActionId , TamamConstants.AuthorizationConstants.ScheduleAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.EditScheduleStatusAuditMessageFailure , schedule.Id ) , string.Empty );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                #endregion

                var state = EventsBroker.Handle( new ScheduleChangeStatusEvent( schedule.Id , oldScheduleDepartments , schedulePeople ) );

                #region Audit trail

                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.EditScheduleAuditActionId , TamamConstants.AuthorizationConstants.ScheduleAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.EditScheduleStatusAuditMessageSuccessful , schedule.Id , status ? "Active" : "Not Active" ) , string.Empty );

                #endregion

                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Schedules );

                #endregion

                #endregion
            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> SoftDeleteSchedule( Guid scheduleId , RequestContext requestContext )
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
                if (
                    !TamamServiceBroker.Authorize( requestContext ,
                        TamamConstants.AuthorizationConstants.DeleteScheduleActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.DeleteScheduleAuditActionId ,
                        TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                        string.Format( TamamConstants.AuthorizationConstants.DeleteScheduleAuditMessageFailure , scheduleId ) , string.Empty );
                    context.Response.Set( ResponseState.AccessDenied , false );
                    return;
                }

                context.ActionContext = new ActionContext
                  (
                  moduleId: TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                  actionId: TamamConstants.AuthorizationConstants.DeleteScheduleAuditActionId ,
                  actionKey: TamamConstants.AuthorizationConstants.DeleteScheduleActionKey ,
                  messageForDenied: string.Format( TamamConstants.AuthorizationConstants.DeleteScheduleAuditMessageFailure , scheduleId ) ,
                  messageForFailure: string.Format( TamamConstants.AuthorizationConstants.DeleteScheduleAuditMessageFailure , scheduleId ) ,
                  messageForSuccess: string.Format( TamamConstants.AuthorizationConstants.DeleteScheduleAuditMessageSuccessful , scheduleId )
                  );

                var securityResponse = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                if ( securityResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( securityResponse.Type , false );
                    return;
                }

                var securityContext = securityResponse.Result;

                #endregion

                #region Data Layer ...
                var scheduleResponse = GetSchedule( scheduleId , requestContext );
                if ( scheduleResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.DeleteScheduleAuditActionId , TamamConstants.AuthorizationConstants.ScheduleAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.DeleteScheduleAuditMessageFailure , scheduleId ) , string.Empty );
                    context.Response.Set( scheduleResponse.Type , false , scheduleResponse.MessageDetailed );
                    return;
                }
                var schedule = scheduleResponse.Result;
                #region Change status to be deactivated

                //Call Change status with false as active status, ChangeScheduleStatus will validate and run the events to maintain data
                var dataHandlerResponse = ChangeScheduleStatus( scheduleId , false , requestContext );
                if ( !dataHandlerResponse.Result )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.DeleteScheduleAuditActionId , TamamConstants.AuthorizationConstants.ScheduleAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.DeleteScheduleAuditMessageFailure , scheduleId ) , string.Empty );
                    context.Response.Set( dataHandlerResponse.Type , false , dataHandlerResponse.MessageDetailed );
                    return;
                }

                #endregion
                #region Change Name by adding "D" and 4 random digits

                Random random = new Random();
                var randameNo = random.Next( 0 , 9999 );
                schedule.Name = schedule.Name + "D" + randameNo;
                schedule.NameCultureVarient = schedule.NameCultureVarient + "D" + randameNo;
                var editResponse = dataHandler.EditSchedule( schedule , securityContext );
                if ( editResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.DeleteScheduleAuditActionId , TamamConstants.AuthorizationConstants.ScheduleAuditModuleId , string.Format( TamamConstants.AuthorizationConstants.DeleteScheduleAuditMessageFailure , scheduleId ) , string.Empty );
                    context.Response.Set( editResponse.Type , false );
                    return;
                }

                #endregion

                #endregion

                #region Audit trail

                TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.DeleteScheduleAuditActionId ,
                    TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                    string.Format( TamamConstants.AuthorizationConstants.DeleteScheduleAuditMessageSuccessful , scheduleId ) , string.Empty );

                #endregion

                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Schedules );

                #endregion

                #endregion
            } , requestContext );

            return context.Response;
        }

        public ExecutionResponse<Schedule> GetSchedule( Guid id , RequestContext requestContext )
        {
            var context = new ExecutionContext<Schedule>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "SchedulesHandler_GetSchedule" + id + requestContext;
                if ( !requestContext.IgnoreCache )
                {
                    var cached = Broker.Cache.Get<Schedule>( TamamCacheClusters.Schedules , cacheKey );
                    if ( cached != null )
                    {
                        context.Response.Set( ResponseState.Success , cached );
                        return;
                    }
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                    actionId: TamamConstants.AuthorizationConstants.GetScheduleAuditActionId ,
                    actionKey: TamamConstants.AuthorizationConstants.GetScheduleActionKey ,
                    messageForDenied: string.Empty ,
                    messageForFailure: TamamConstants.AuthorizationConstants.GetScheduleAuditMessageFailure ,
                    messageForSuccess: string.Empty
                    );

                var securityResponse = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                if ( securityResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( securityResponse.Type , null );
                    return;
                }

                var securityContext = securityResponse.Result;

                #endregion

                var dataHandlerResponse = dataHandler.GetSchedule( id , securityContext );
                context.Response.Set( dataHandlerResponse );

                #region Cache

                if ( !requestContext.IgnoreCache )
                {
                    Broker.Cache.Add<Schedule>( TamamCacheClusters.Schedules , cacheKey , dataHandlerResponse.Result );
                }

                #endregion

                #endregion
            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<List<Schedule>> GetSchedules( List<Guid> ids , RequestContext requestContext )
        {
            var context = new ExecutionContext<List<Schedule>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "SchedulesHandler_GetSchedules" + ListToString( ids ) + requestContext;
                if ( !requestContext.IgnoreCache )
                {
                    var cached = Broker.Cache.Get<List<Schedule>>( TamamCacheClusters.Schedules , cacheKey );
                    if ( cached != null )
                    {
                        context.Response.Set( ResponseState.Success , cached );
                        return;
                    }
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                    actionId: TamamConstants.AuthorizationConstants.GetScheduleAuditActionId ,
                    actionKey: TamamConstants.AuthorizationConstants.GetScheduleActionKey ,
                    messageForDenied: string.Empty ,
                    messageForFailure: TamamConstants.AuthorizationConstants.GetScheduleAuditMessageFailure ,
                    messageForSuccess: string.Empty
                    );

                var securityResponse = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                if ( securityResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( securityResponse.Type , null );
                    return;
                }

                var securityContext = securityResponse.Result;

                #endregion

                var dataHandlerResponse = dataHandler.GetSchedules( ids , securityContext );
                context.Response.Set( dataHandlerResponse );

                #region Cache

                if ( !requestContext.IgnoreCache )
                {
                    Broker.Cache.Add<List<Schedule>>( TamamCacheClusters.Schedules , cacheKey , dataHandlerResponse.Result );
                }

                #endregion

                #endregion
            } , requestContext );

            return context.Response;
        }

        public ExecutionResponse<List<Schedule>> GetSchedules( DateTime startDate , DateTime? endDate , List<Guid> personIds , List<Guid> departmentIds , RequestContext requestContext )
        {
            var context = new ExecutionContext<List<Schedule>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "SchedulesHandler_GetSchedules" + startDate.ToShortDateString() + ( endDate.HasValue ? endDate.Value.ToShortDateString() : string.Empty ) + ListToString<Guid>( departmentIds ) + ListToString<Guid>( personIds ) + requestContext;
                var cached = Broker.Cache.Get<List<Schedule>>( TamamCacheClusters.Schedules , cacheKey );
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
                    actionId: TamamConstants.AuthorizationConstants.GetScheduleAuditActionId ,
                    actionKey: TamamConstants.AuthorizationConstants.GetScheduleActionKey ,
                    messageForDenied: string.Empty ,
                    messageForFailure: string.Empty ,
                    messageForSuccess: string.Empty
                    );

                var response = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type , null );
                    return;
                }

                var securityContext = response.Result;

                #endregion

                var dataHandlerResponse = dataHandler.GetSchedules( startDate , endDate , personIds , departmentIds , securityContext );
                context.Response.Set( ResponseState.Success , dataHandlerResponse.Result );

                #region Cache

                Broker.Cache.Add<List<Schedule>>( TamamCacheClusters.Schedules , cacheKey , dataHandlerResponse.Result );

                #endregion

                #endregion
            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<List<ScheduleDay>> GetScheduleDetails( Schedule schedule , DateTime startDate , DateTime endDate , RequestContext requestContext )
        {
            var context = new ExecutionContext<List<ScheduleDay>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "SchedulesHandler_GetScheduleDetails" + schedule.Id + startDate.ToShortDateString() + endDate.ToShortDateString() + requestContext;
                var cached = Broker.Cache.Get<List<ScheduleDay>>( TamamCacheClusters.Schedules , cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                    actionId: TamamConstants.AuthorizationConstants.GetScheduleAuditActionId ,
                    actionKey: TamamConstants.AuthorizationConstants.GetScheduleActionKey ,
                    messageForDenied: string.Empty ,
                    messageForFailure: TamamConstants.AuthorizationConstants.GetScheduleAuditMessageFailure ,
                    messageForSuccess: string.Empty
                    );

                var securityResponse = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                if ( securityResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( securityResponse.Type , null );
                    return;
                }

                var securityContext = securityResponse.Result;

                #endregion

                // validation ...
                if ( endDate < startDate )
                {
                    TamamServiceBroker.Audit( requestContext , TamamConstants.AuthorizationConstants.GetScheduleAuditActionId , TamamConstants.AuthorizationConstants.ScheduleAuditModuleId , TamamConstants.AuthorizationConstants.GetScheduleAuditMessageFailure , string.Empty ); context.Response.Set( ResponseState.ValidationError , null , new List<ModelMetaPair> { new ModelMetaPair( string.Empty , "Invalid date range" ) } );
                    return;
                }

                // adjust dates ranges to schedule times ...
                //startDate = startDate < schedule.StartDate ? schedule.StartDate : startDate;
                endDate = schedule.EndDate.HasValue && endDate > schedule.EndDate ? schedule.EndDate.Value : endDate;
                // int startDateOffset = ( int ) ( schedule.StartDate - startDate ).TotalDays;

                // working days ...
                // List<DateTime> dates = Enumerable.Range( 0 , ( int ) ( ( endDate - startDate ).TotalDays ) + 1 ).Select( n => startDate.AddDays( n ) ).ToList();
                var daysCount = ( int ) ( ( endDate - schedule.StartDate ).TotalDays ) + 1;
                if ( daysCount <= 0 )
                {
                    context.Response.Set( ResponseState.Success , new List<ScheduleDay>() );
                    return;
                }

                List<DateTime> dates = Enumerable.Range( 0 , daysCount ).Select( n => schedule.StartDate.AddDays( n ) ).ToList();

                var template = dataHandler.GetScheduleTemplate( schedule.ScheduleTemplateId ).Result;
                var details = template.TemplateDetails;
                var result = new List<ScheduleDay>();
                int counter = 1;
                int repeatLenght = 0;
                switch ( template.RepeatType )
                {
                    case 1:
                        repeatLenght = template.Repeat;
                        break; //day
                    case 2:
                        repeatLenght = template.Repeat * 7;
                        break; // week
                    case 3:
                        repeatLenght = template.Repeat * 31;
                        break; // month
                }

                if ( template.RepeatType == 2 )
                {
                    //get offset
                    var seq = details.Where( det => det.DayId == ( int ) dates[0].DayOfWeek ).OrderBy( o => o.Sequence ).FirstOrDefault();
                    if ( seq != null )
                    {
                        counter = seq.Sequence;
                    }
                }

                if ( template.RepeatType == 3 )
                {
                    //get offset
                    var seq = details.Where( det => det.Sequence == ( int ) dates[0].Day ).OrderBy( o => o.Sequence ).FirstOrDefault();
                    if ( seq != null )
                    {
                        counter = seq.Sequence;
                    }
                }

                if ( template.RepeatType == 1 || template.RepeatType == 2 )
                {
                    for ( int i = 0 ; i < dates.Count ; i++ )
                    {
                        var t = details.FirstOrDefault( det => det.Sequence == counter );
                        if ( t != null && ( dates[i] >= startDate.Date && dates[i] <= endDate.Date ) )
                        {
                            var d = new ScheduleDay( t , schedule.Id ) { Day = dates[i] };
                            result.Add( d );
                        }
                        counter++;
                        if ( counter > repeatLenght ) counter = 1;
                    }
                }
                else
                {
                    for ( int i = 0 ; i < dates.Count ; i++ )
                    {
                        var t = details.FirstOrDefault( det => det.Sequence == dates[i].Day );
                        if ( t != null && ( dates[i] >= startDate.Date && dates[i] <= endDate.Date ) )
                        {
                            var d = new ScheduleDay( t , schedule.Id ) { Day = dates[i] };
                            result.Add( d );
                        }
                        counter++;
                        if ( counter > repeatLenght ) counter = 1;
                    }
                }

                context.Response.Set( ResponseState.Success , result );

                #region Cache

                Broker.Cache.Add<List<ScheduleDay>>( TamamCacheClusters.Schedules , cacheKey , result );

                #endregion

                #endregion
            } , requestContext );
            return context.Response;
        }
        public ExecutionResponse<List<ScheduleDay>> GetScheduleDetails( Schedule schedule , DateTime startDate , DateTime endDate , List<Guid> departmentsIds , List<Guid> personnelIds , RequestContext requestContext )
        {
            var context = new ExecutionContext<List<ScheduleDay>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "SchedulesHandler_GetScheduleDetails_schedule" + schedule.Id + "_departmentsIds" + ListToString<Guid>( departmentsIds ) + "_personnelIds" + ListToString<Guid>( personnelIds )
                    + "_startDate" + startDate.ToShortDateString() + "_endDate" + endDate.ToShortDateString() + "_requestContext" + requestContext;
                var cached = Broker.Cache.Get<List<ScheduleDay>>( TamamCacheClusters.Schedules , cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                    actionId: TamamConstants.AuthorizationConstants.GetScheduleAuditActionId ,
                    actionKey: TamamConstants.AuthorizationConstants.GetScheduleActionKey ,
                    messageForDenied: string.Empty ,
                    messageForFailure: TamamConstants.AuthorizationConstants.GetScheduleAuditMessageFailure ,
                    messageForSuccess: string.Empty
                    );

                var securityResponse = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                if ( securityResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( securityResponse.Type , null );
                    return;
                }

                var securityContext = securityResponse.Result;

                #endregion
                #region Validation

                if ( endDate < startDate )
                {
                    context.Response.Set( ResponseState.ValidationError , null , new List<ModelMetaPair> { new ModelMetaPair( "" , "Invalid date range" ) } );
                    return;
                }

                #endregion
                #region Init ...

                var schedulesDetails = new List<ScheduleDay>();
                var personnelIdsList = new List<Guid>( personnelIds ?? new List<Guid>() );
                var departmentsIdsList = new List<Guid>( departmentsIds ?? new List<Guid>() );

                #endregion
                #region visibility

                var visibilityPersonnelIds = securityContext.PersonnelRange == null ? new List<Guid>() : XModel.ToListOfGuid( securityContext.PersonnelRange );
                var visibilityDepartmentsIds = securityContext.DepartmentsRange == null ? new List<Guid>() : securityContext.DepartmentsRange.ToGuidList();

                if ( securityContext != null && !( securityContext is SystemSecurityContext ) )
                {
                    if ( securityContext.VisibilityMode == AuthorizationVisibilityMode.Personnel )
                    {
                        personnelIdsList = ( personnelIdsList.Any() )
                                         ? personnelIdsList.Intersect( visibilityPersonnelIds ).ToList()
                                         : visibilityPersonnelIds;

                        // empty departmentsIdsList ids means select all
                        // but no need to refill it as effective schedules are already in personnelIdsList
                    }
                    else if ( securityContext.VisibilityMode == AuthorizationVisibilityMode.Departments )
                    {
                        departmentsIdsList = ( departmentsIdsList.Any() )
                                           ? departmentsIdsList.Intersect( visibilityDepartmentsIds ).ToList()
                                           : visibilityDepartmentsIds;

                        if ( !personnelIdsList.Any() )
                        {
                            var personnelResult = TamamServiceBroker.PersonnelHandler.GetPersonnel( new PersonSearchCriteria { Departments = departmentsIdsList } , SystemRequestContext.Instance );
                            // empty personnel ids means select all  

                            if ( personnelResult.Type == ResponseState.Success )
                            {
                                personnelIdsList = personnelResult.Result.Persons.Select( x => x.Id ).ToList();
                            }
                        }
                    }
                    else
                    {
                        //Admin mode do if there is no filtered add add all dep to filter b them 
                        if ( !( departmentsIdsList.Any() || personnelIdsList.Any() ) )
                        {
                            var response_GetScheduleDetails = GetScheduleDetails( schedule , startDate , endDate , SystemRequestContext.Instance );
                            context.Response.Set( response_GetScheduleDetails );
                            return;
                        }
                    }
                }

                #endregion

                #region SchedulePersonnel

                var schedulePersonnel = new List<EffectiveSchedulePerson>();

                #region SchedulePersonnel at departments

                if ( departmentsIdsList != null && departmentsIdsList.Any() )
                {
                    var schedulePersonnelResponse =
                        dataHandler.GetEffectiveSchedulePersonnel_DepartmentId( departmentsIdsList , SystemSecurityContext.Instance );
                    if ( schedulePersonnelResponse.Type != ResponseState.Success )
                    {
                        context.Response.Set( ResponseState.SystemError , null );
                        return;
                    }
                    var schedulePersonnel_FilteredByDepartments = schedulePersonnelResponse.Result;
                    var schedulePersonnel_FilteredByDepartmentsAndSchedule =
                        schedulePersonnel_FilteredByDepartments.Where( sp => sp.ScheduleId == schedule.Id );
                    schedulePersonnel.AddRange( schedulePersonnel_FilteredByDepartmentsAndSchedule );
                }

                #endregion

                if ( personnelIdsList != null && personnelIdsList.Any() )
                {
                    foreach ( var personId in personnelIdsList )
                    {
                        var schedulePersonnel_FilteredByPerson = GetEffectiveSchedulePersonnel_PersonId( personId , requestContext ).Result;
                        var schedulePersonnel_FilteredByPersonAndSchedule = schedulePersonnel_FilteredByPerson.Where( x => x.ScheduleId == schedule.Id );
                        schedulePersonnel.AddRange( schedulePersonnel_FilteredByPersonAndSchedule );
                    }
                }

                foreach ( var schedulePerson in schedulePersonnel )
                {
                    var startDateFilter = GetInterSectedStartDate( startDate , endDate , schedulePerson.StartDate , schedulePerson.EndDate );
                    var endDateFilter = GetInterSectedEndDate( startDate , endDate , schedulePerson.StartDate , schedulePerson.EndDate );

                    if ( !startDateFilter.HasValue || !endDateFilter.HasValue ) continue;
                    var getDetailsResponse = GetScheduleDetails( schedule , startDateFilter.Value , endDateFilter.Value , SystemRequestContext.Instance );
                    if ( getDetailsResponse.Type != ResponseState.Success )
                    {
                        context.Response.Set( ResponseState.SystemError , null );
                        return;
                    }

                    foreach ( var sDetail in getDetailsResponse.Result )
                    {
                        if ( !schedulesDetails.Any( sd => sd.ScheduleId == sDetail.ScheduleId && sd.Day == sDetail.Day ) )
                        {
                            schedulesDetails.Add( sDetail );
                        }
                    }
                }

                #endregion
                #region ScheduleDepartments

                if ( departmentsIdsList != null && departmentsIdsList.Any() )
                {
                    var scheduleDepartments = new List<EffectiveScheduleDepartment>();
                    foreach ( var departmentId in departmentsIdsList )
                    {
                        var scheduleDepartmentsResponse = GetEffectiveScheduleDepartments_DepartmentId( departmentId ,
                            requestContext );
                        if ( scheduleDepartmentsResponse.Type != ResponseState.Success )
                        {
                            context.Response.Set( ResponseState.SystemError , null );
                            return;
                        }
                        var scheduleDepartments_FilteredByDepartment = scheduleDepartmentsResponse.Result;
                        var schedulePersonnel_FilteredByDepartmentAndSchedule =
                            scheduleDepartments_FilteredByDepartment.Where( sp => sp.ScheduleId == schedule.Id );
                        scheduleDepartments.AddRange( schedulePersonnel_FilteredByDepartmentAndSchedule );
                    }

                    foreach ( var scheduleDepartment in scheduleDepartments )
                    {
                        var startDateFilter = GetInterSectedStartDate( startDate , endDate , scheduleDepartment.StartDate , scheduleDepartment.EndDate );
                        var endDateFilter = GetInterSectedEndDate( startDate , endDate , scheduleDepartment.StartDate , scheduleDepartment.EndDate );

                        if ( !startDateFilter.HasValue || !endDateFilter.HasValue ) continue;
                        var getDetailsResponse = GetScheduleDetails( schedule , startDateFilter.Value , endDateFilter.Value , requestContext );
                        if ( getDetailsResponse.Type != ResponseState.Success )
                        {
                            context.Response.Set( getDetailsResponse.Type , null );
                            return;
                        }

                        foreach ( var sDetail in getDetailsResponse.Result )
                        {
                            if ( !schedulesDetails.Any( sd => sd.ScheduleId == sDetail.ScheduleId && sd.Day == sDetail.Day ) )
                            {
                                schedulesDetails.Add( sDetail );
                            }
                        }
                    }
                }

                schedulesDetails = schedulesDetails.OrderBy( sd => sd.Day ).ToList();

                #endregion

                context.Response.Set( ResponseState.Success , schedulesDetails );

                #region Cache

                Broker.Cache.Add<List<ScheduleDay>>( TamamCacheClusters.Schedules , cacheKey , schedulesDetails );

                #endregion

                #endregion
            } , requestContext );
            return context.Response;
        }
        public ExecutionResponse<ScheduleDaysGrouped> GetScheduleDetails( DateTime startDate , DateTime endDate , List<Guid> departmentsIds , List<Guid> personnelIds , RequestContext requestContext )
        {
            var context = new ExecutionContext<ScheduleDaysGrouped>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "SchedulesHandler_GetScheduleDetails"
                             + "_departmentsIds" + ListToString<Guid>( departmentsIds )
                             + "_personnelIds" + ListToString<Guid>( personnelIds )
                             + "_startDate" + startDate.ToShortDateString()
                             + "_endDate" + endDate.ToShortDateString()
                             + "_requestContext" + requestContext;

                var cached = Broker.Cache.Get<ScheduleDaysGrouped>( TamamCacheClusters.Schedules , cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                                        (
                                        moduleId: TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                                        actionId: TamamConstants.AuthorizationConstants.GetScheduleAuditActionId ,
                                        actionKey: TamamConstants.AuthorizationConstants.GetScheduleActionKey ,
                                        messageForDenied: string.Empty ,
                                        messageForFailure: TamamConstants.AuthorizationConstants.GetScheduleAuditMessageFailure ,
                                        messageForSuccess: string.Empty
                                        );

                var securityResponse = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                if ( securityResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( securityResponse.Type , null );
                    return;
                }

                var securityContext = securityResponse.Result;

                #endregion

                #region schedules

                var result = new ScheduleDaysGrouped();

                // admin or HR ...
                if ( ( securityContext.PersonnelRange == null || !securityContext.PersonnelRange.Any() ) && ( securityContext.DepartmentsRange == null || !securityContext.DepartmentsRange.Any() ) )
                {
                    var schedules = GetSchedules( startDate , endDate , personnelIds , departmentsIds , requestContext ).Result;
                    foreach ( var schedule in schedules ) { result.Add( schedule , GetScheduleDetails( schedule , startDate , endDate , SystemRequestContext.Instance ).Result ); }
                }

                #endregion
                else
                {
                    #region Get effective schedules

                    var response = dataHandler.GetSchedulesEffective( startDate , endDate , personnelIds , departmentsIds , securityContext );
                    if ( response.Type != ResponseState.Success )
                    {
                        context.Response.Set( response.Type , null );
                        return;
                    }

                    #endregion
                    #region combine effectives

                    List<IEffectiveSchedule> effectives = response.Result != null ? response.Result.OrderBy( x => x.StartDate ).ToList() : new List<IEffectiveSchedule>();
                    List<IEffectiveSchedule> effectivesCombined = new List<IEffectiveSchedule>();
                    List<Guid> scheduleIds = effectives.Select( x => x.ScheduleId ).Distinct().ToList();

                    foreach ( var scheduleId in scheduleIds )
                    {
                        var effectivesFiltered = effectives.Where( x => x.ScheduleId == scheduleId ).ToList();
                        effectivesCombined.Add( effectivesFiltered[0] );

                        for ( int i = 0, j = 0 ; i < effectivesFiltered.Count ; i++ )
                        {
                            j = effectivesCombined.Count - 1;

                            var combined = XIntervals.Combine( effectivesFiltered[i] , effectivesCombined[j] );
                            if ( combined == null )
                            {
                                effectivesCombined.Add( effectivesFiltered[i] );
                            }
                            else
                            {
                                effectivesCombined.RemoveAt( j );
                                effectivesCombined.Add( new EffectiveSchedulePerson()
                                {
                                    ScheduleId = scheduleId ,
                                    StartDate = combined.Start.GetValueOrDefault() ,
                                    EndDate = combined.End ,
                                } );
                            }
                        }
                    }

                    #endregion
                    #region Get Details

                    var schedules = GetSchedules( scheduleIds , SystemRequestContext.Instance ).Result;
                    foreach ( var effective in effectivesCombined )
                    {
                        #region dates

                        var filterDateRange = XIntervals.Intersect( new XIntervals.DateRange( startDate , endDate ) , new XIntervals.DateRange( effective.StartDate , effective.EndDate ) );
                        if ( filterDateRange == null ) continue;

                        #endregion

                        var schedule = schedules.Where( x => x.Id == effective.ScheduleId ).FirstOrDefault();
                        var details = GetScheduleDetails( schedule , filterDateRange.Start.Value , filterDateRange.End.Value , SystemRequestContext.Instance ).Result;

                        result.Add( schedule , details );
                    }

                    #endregion
                }

                context.Response.Set( ResponseState.Success , result );

                #region Cache

                Broker.Cache.Add<ScheduleDaysGrouped>( TamamCacheClusters.Schedules , cacheKey , result );

                #endregion

                #endregion
            } , requestContext );
            return context.Response;
        }

        public ExecutionResponse<List<ScheduleDay>> GetScheduleDays( DateTime date , RequestContext requestContext )
        {
            var context = new ExecutionContext<List<ScheduleDay>>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "SchedulesHandler_GetScheduleDays" + date.ToShortDateString() + requestContext;
                var cached = Broker.Cache.Get<List<ScheduleDay>>( TamamCacheClusters.Schedules , cacheKey );
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
                if (
                    !TamamServiceBroker.Authorize( requestContext ,
                        TamamConstants.AuthorizationConstants.GetScheduleActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.GetScheduleAuditActionId ,
                        TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                        TamamConstants.AuthorizationConstants.GetScheduleAuditMessageFailure , string.Empty );

                    context.Response.Set( ResponseState.AccessDenied , null , new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                var schedules = GetSchedules( date , date , null , null , SystemRequestContext.Instance ).Result;
                var result = new List<ScheduleDay>();
                if ( schedules != null )
                {
                    foreach ( var schedule in schedules )
                    {
                        var day = GetScheduleDetails( schedule , date , date , requestContext ).Result.FirstOrDefault();
                        if ( day != null )
                        {
                            day.ScheduleId = schedule.Id;
                            result.Add( day );
                        }
                    }
                }
                context.Response.Set( ResponseState.Success , result );

                #region Cache

                Broker.Cache.Add<List<ScheduleDay>>( TamamCacheClusters.Schedules , cacheKey , result );

                #endregion

            } , requestContext );
            return context.Response;
        }
        public ExecutionResponse<ScheduleDay> GetPersonScheduleDay( Guid personId , DateTime date , RequestContext requestContext )
        {
            var context = new ExecutionContext<ScheduleDay>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "SchedulesHandler_GetPersonScheduleDay" + personId + date.ToShortDateString() + requestContext;
                var cached = Broker.Cache.Get<ScheduleDay>( TamamCacheClusters.Schedules , cacheKey );
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
                if (
                    !TamamServiceBroker.Authorize( requestContext ,
                        TamamConstants.AuthorizationConstants.GetScheduleActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.GetScheduleAuditActionId ,
                        TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                        TamamConstants.AuthorizationConstants.GetScheduleAuditMessageFailure , string.Empty );

                    context.Response.Set( ResponseState.AccessDenied , null , new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                var personnelIds = new List<Guid> { personId };
                var schedules = GetSchedules( date , date , personnelIds , null , requestContext ).Result;
                foreach ( var schedule in schedules )
                {
                    var day = GetScheduleDetails( schedule , date , date , null , personnelIds , requestContext ).Result.FirstOrDefault();
                    if ( day != null )
                    {
                        // day.ScheduleId = schedule.Id;
                        context.Response.Set( ResponseState.Success , day );
                        #region Cache

                        Broker.Cache.Add<ScheduleDay>( TamamCacheClusters.Schedules , cacheKey , day );

                        #endregion
                        break;
                    }
                }
                if ( schedules.Count == 0 ) context.Response.Set( ResponseState.NotFound , null );


            } , requestContext );
            return context.Response;
        }
        public ExecutionResponse<List<Person>> GetScheduleActivePersons( Guid scheduleId , DateTime date , RequestContext requestContext )
        {
            var context = new ExecutionContext<List<Person>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "SchedulesHandler_GetScheduleActivePersons" + scheduleId + date.Date.ToString() + requestContext;
                var cached = Broker.Cache.Get<List<Person>>( TamamCacheClusters.Schedules , cacheKey );
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
                if (
                    !TamamServiceBroker.Authorize( requestContext ,
                        TamamConstants.AuthorizationConstants.GetScheduleActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.GetScheduleAuditActionId ,
                        TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                        TamamConstants.AuthorizationConstants.GetScheduleAuditMessageFailure , string.Empty );
                    context.Response.Set( ResponseState.AccessDenied , null , new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                var dataHandlerResponse = dataHandler.GetScheduleActivePersons( scheduleId , date );
                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Add<List<Person>>( TamamCacheClusters.Schedules , cacheKey , dataHandlerResponse.Result );

                #endregion

                #endregion
            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> TransferSchedule( Guid scheduleId , Guid personId , DateTime startDate , DateTime? endDate , RequestContext requestContext )
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
                if (
                    !TamamServiceBroker.Authorize( requestContext ,
                        TamamConstants.AuthorizationConstants.TransferScheduleActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.TransferScheduleAuditActionId ,
                        TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                        string.Format( TamamConstants.AuthorizationConstants.TransferScheduleAuditMessageFailure ,
                            scheduleId ) , scheduleId.ToString() );
                    context.Response.Set( ResponseState.AccessDenied , false , new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region Validation

                var schedulePerson = new SchedulePerson()
                {
                    Id = Guid.NewGuid() ,
                    PersonId = personId ,
                    ScheduleId = scheduleId ,
                    StartDate = startDate ,
                    EndDate = endDate
                };
                var validator = new SchedulePersonValidator( schedulePerson , TamamConstants.ValidationMode.Create );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext ,
                        TamamConstants.AuthorizationConstants.TransferScheduleAuditActionId ,
                        TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                        string.Format( TamamConstants.AuthorizationConstants.TransferScheduleAuditMessageFailure ,
                            scheduleId ) , string.Empty );
                    context.Response.Set( ResponseState.ValidationError , false , validator.ErrorsDetailed );
                    return;
                }

                # endregion
                # region logic

                DateTime maxDate = DateTime.MaxValue;

                // get direct schedules associated with this person in the specified period ..
                var response = dataHandler.GetSchedulePersonnel_PersonId( personId );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( ResponseState.Failure , false );
                    return;
                }

                var schedulePersonnel = response.Result;

                // sort by start date
                schedulePersonnel = schedulePersonnel.OrderBy( x => x.StartDate.Date ).ToList();

                // set null EndDate into DateTime.MaxValue
                if ( !endDate.HasValue ) endDate = DateTime.MaxValue;
                foreach ( SchedulePerson item in schedulePersonnel )
                {
                    if ( !item.EndDate.HasValue ) item.EndDate = DateTime.MaxValue;
                }


                // Prepare the new list
                var updatedList = new List<SchedulePerson>();

                foreach ( SchedulePerson item in schedulePersonnel )
                {
                    // 1- Check if the schedule item (take place before or after the associated period)
                    if ( ( startDate.Date > item.StartDate.Date && startDate.Date > item.EndDate.Value.Date ) ||
                        ( endDate.Value.Date < item.StartDate.Date && endDate.Value.Date < item.EndDate.Value.Date ) )
                    {
                        updatedList.Add( item );
                    }
                    // 2- Check if the schedule item (precede the associated period and intersect with endDate)
                    else if ( startDate.Date > item.StartDate.Date && startDate.Date == item.EndDate.Value.Date )
                    {
                        item.EndDate = startDate.AddDays( -1 );
                        updatedList.Add( item );
                    }
                    // 3- Check if the schedule item (precede the associated period and intersect with that period)
                    else if ( startDate.Date > item.StartDate.Date &&
                             ( item.EndDate.Value.Date > startDate.Date && item.EndDate.Value.Date <= endDate.Value.Date ) )
                    {
                        item.EndDate = startDate.AddDays( -1 );
                        updatedList.Add( item );
                    }
                    // 4- Check if the schedule item contains the associated period
                    else if ( item.StartDate.Date < startDate.Date && item.EndDate.Value.Date > endDate.Value.Date )
                    {
                        // divide schedule item into two items
                        var firstItem = new SchedulePerson()
                        {
                            ScheduleId = item.ScheduleId ,
                            PersonId = item.PersonId ,
                            StartDate = item.StartDate ,
                            EndDate = startDate.AddDays( -1 )
                        };
                        var secondItem = new SchedulePerson()
                        {
                            ScheduleId = item.ScheduleId ,
                            PersonId = item.PersonId ,
                            StartDate = endDate.Value.AddDays( 1 ) ,
                            EndDate = item.EndDate
                        };
                        updatedList.Add( firstItem );
                        updatedList.Add( secondItem );
                    }
                    // 5- Check if the schedule item enclosed inside the period
                    else if ( item.StartDate.Date >= startDate.Date && item.EndDate.Value.Date <= endDate.Value.Date )
                    {
                        continue;
                    }
                    // 6- Check if the schedule item intersects with the period in EndDate
                    else if ( item.StartDate.Date == endDate.Value.Date )
                    {
                        item.StartDate = item.StartDate.AddDays( 1 );
                        updatedList.Add( item );
                    }
                    // 7- Check if the schedule item intersects with the period in EndDate
                    else if ( ( item.StartDate.Date >= startDate.Date && item.StartDate.Date <= endDate.Value.Date ) &&
                             item.EndDate.Value.Date > endDate.Value.Date )
                    {
                        if ( endDate.Value.Date > maxDate.AddDays( -1 ) ) continue;

                        item.StartDate = endDate.Value.AddDays( 1 );
                        updatedList.Add( item );
                    }
                }

                // transferred schedule..
                var transferred = new SchedulePerson
                {
                    ScheduleId = scheduleId ,
                    PersonId = personId ,
                    StartDate = startDate ,
                    EndDate = endDate
                };
                updatedList.Add( transferred );

                // reverse EndDates into null values..
                foreach ( SchedulePerson item in updatedList )
                {
                    if ( item.EndDate.HasValue && item.EndDate.Value.Date > maxDate.AddDays( -1 ).Date )
                        item.EndDate = null;
                }

                var updateResponse = dataHandler.UpdateSchedulePersonnel( personId , updatedList );

                if ( updateResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( updateResponse );
                    return;
                }

                #region Events

                var response_LeaveCredits = TamamServiceBroker.LeavesHandler.RecalculateLeaveCredit( personId ,
                    SystemRequestContext.Instance );
                if ( response_LeaveCredits.Type != ResponseState.Success )
                {
                    // TODO: Cannot Recalculate Leaves Credit
                }

                // excuses
                var response_excuses = TamamServiceBroker.LeavesHandler.RecalculateExcuseDuration( personId ,
                    SystemRequestContext.Instance );
                if ( response_excuses.Type != ResponseState.Success )
                {
                    // TODO: Cannot Recalculate Excuses
                }

                // Person Effective Schedule 
                var response_updatePersonSchedule = ReCalculateEffectiveSchedulesForPerson( personId );
                if ( response_updatePersonSchedule.Type != ResponseState.Success )
                {
                    context.Response.Set( response_updatePersonSchedule );
                    return;
                }

                #endregion

                context.Response.Set( updateResponse );

                # endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Schedules );

                #endregion

            } , requestContext );

            return context.Response;
        }

        //? xxxxxxxxxxxxxxxxxxxx

        // V1
        //public ExecutionResponse<List<ScheduleDay>> GetScheduleDetails(Schedule schedule, List<Guid> departmentsIds, List<Guid> personnelIds, DateTime startDate, DateTime endDate, RequestContext requestContext)
        //{
        //    var context = new ExecutionContext<List<ScheduleDay>>();
        //    context.Execute(() =>
        //    {
        //        #region logic ...

        //        #region Security

        //        context.ActionContext = new ActionContext
        //                                (
        //                                moduleId: TamamConstants.AuthorizationConstants.ScheduleAuditModuleId,
        //                                actionId: TamamConstants.AuthorizationConstants.GetScheduleAuditActionId,
        //                                actionKey: TamamConstants.AuthorizationConstants.GetScheduleActionKey,
        //                                messageForDenied: string.Empty,
        //                                messageForFailure: TamamConstants.AuthorizationConstants.GetScheduleAuditMessageFailure,
        //                                messageForSuccess: string.Empty
        //                                );

        //        var securityResponse = TamamServiceBroker.Secure(context.ActionContext, requestContext);
        //        if (securityResponse.Type != ResponseState.Success)
        //        {
        //            context.Response.Set(securityResponse.Type, null);
        //            return;
        //        }

        //        var securityContext = securityResponse.Result;

        //        #endregion

        //        // validation ...
        //        if (endDate < startDate)
        //        {
        //            context.Response.Set(ResponseState.ValidationError, null, new List<ModelMetaPair> { new ModelMetaPair(string.Empty, "Invalid date range") });
        //            return;
        //        }

        //        #region Init ...

        //        var schedulesDetails = new List<ScheduleDay>();

        //        #endregion

        //        #region SchedulePersonnel
        //        if (personnelIds!=null && personnelIds.Any())
        //        {
        //            var schedulePersonnel = new List<EffectiveSchedulePerson>();
        //            foreach (var personId in personnelIds)
        //            {
        //                var schedulePersonnelResponse = GetEffectiveSchedulePersonnel_PersonId(personId, requestContext);
        //                if (schedulePersonnelResponse.Type != ResponseState.Success)
        //                {
        //                    context.Response.Set(ResponseState.SystemError, null);
        //                    return;
        //                }
        //                var schedulePersonnel_FilteredByPerson = schedulePersonnelResponse.Result;
        //                var schedulePersonnel_FilteredByPersonAndSchedule = schedulePersonnel_FilteredByPerson.Where(sp => sp.ScheduleId == schedule.Id);
        //                schedulePersonnel.AddRange(schedulePersonnel_FilteredByPersonAndSchedule);
        //            }

        //            foreach (var schedulePerson in schedulePersonnel)
        //            {
        //                var startDateFilter = GetInterSectedStartDate(startDate, endDate, schedulePerson.StartDate, schedulePerson.EndDate);
        //                var endDateFilter = GetInterSectedEndDate(startDate, endDate, schedulePerson.StartDate, schedulePerson.EndDate);

        //                if (!startDateFilter.HasValue || !endDateFilter.HasValue) continue;
        //                var getDetailsResponse = GetScheduleDetails(schedule, startDateFilter.Value, endDateFilter.Value, requestContext);
        //                if (getDetailsResponse.Type != ResponseState.Success)
        //                {
        //                    context.Response.Set(ResponseState.SystemError, null);
        //                    return;
        //                }

        //                foreach (var sDetail in getDetailsResponse.Result)
        //                {
        //                    if (!schedulesDetails.Any(sd => sd.ScheduleId == sDetail.ScheduleId && sd.Day == sDetail.Day))
        //                    {
        //                        schedulesDetails.Add(sDetail);
        //                    }
        //                }
        //            }
        //        }
        //        #endregion

        //        #region ScheduleDepartments
        //        if (departmentsIds != null && departmentsIds.Any())
        //        {
        //            var scheduleDepartments = new List<EffectiveScheduleDepartment>();
        //            foreach (var departmentId in departmentsIds)
        //            {
        //                var scheduleDepartmentsResponse = GetEffectiveScheduleDepartments_DepartmentId(departmentId, requestContext);
        //                if (scheduleDepartmentsResponse.Type != ResponseState.Success)
        //                {
        //                    context.Response.Set(ResponseState.SystemError, null);
        //                    return;
        //                }
        //                var scheduleDepartments_FilteredByDepartment = scheduleDepartmentsResponse.Result;
        //                var schedulePersonnel_FilteredByDepartmentAndSchedule = scheduleDepartments_FilteredByDepartment.Where(sp => sp.ScheduleId == schedule.Id);
        //                scheduleDepartments.AddRange(schedulePersonnel_FilteredByDepartmentAndSchedule);
        //            }

        //            foreach (var scheduleDepartment in scheduleDepartments)
        //            {
        //                var startDateFilter = GetInterSectedStartDate(startDate, endDate, scheduleDepartment.StartDate, scheduleDepartment.EndDate);
        //                var endDateFilter = GetInterSectedEndDate(startDate, endDate, scheduleDepartment.StartDate, scheduleDepartment.EndDate);

        //                if (!startDateFilter.HasValue || !endDateFilter.HasValue) continue;
        //                var getDetailsResponse = GetScheduleDetails(schedule, startDateFilter.Value, endDateFilter.Value, requestContext);
        //                if (getDetailsResponse.Type != ResponseState.Success)
        //                {
        //                    context.Response.Set(ResponseState.SystemError, null);
        //                    return;
        //                }

        //                foreach (var sDetail in getDetailsResponse.Result)
        //                {
        //                    if (!schedulesDetails.Any(sd => sd.ScheduleId == sDetail.ScheduleId && sd.Day == sDetail.Day))
        //                    {
        //                        schedulesDetails.Add(sDetail);
        //                    }
        //                }
        //            }
        //        }
        //        #endregion

        //        context.Response.Set(ResponseState.Success, schedulesDetails.OrderBy(sd => sd.Day).ToList());

        //        #endregion
        //    }, requestContext);
        //    return context.Response;
        //}

        #region Schedule Personnel - Direct

        public ExecutionResponse<List<SchedulePerson>> GetSchedulePersonnel_ScheduleId( Guid scheduleId , RequestContext requestContext )
        {
            var context = new ExecutionContext<List<SchedulePerson>>();
            context.Execute( () =>
            {
                #region logic ...

                #region

                var cacheKey = "SchedulesHandler_GetSchedulePersonnel_ScheduleId" + scheduleId + requestContext;
                var cached = Broker.Cache.Get<List<SchedulePerson>>( TamamCacheClusters.Schedules , cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                    actionId: TamamConstants.AuthorizationConstants.GetScheduleAuditActionId ,
                    actionKey: TamamConstants.AuthorizationConstants.GetScheduleActionKey ,
                    messageForDenied: string.Empty ,
                    messageForFailure: TamamConstants.AuthorizationConstants.GetScheduleAuditMessageFailure ,
                    messageForSuccess: string.Empty
                    );

                var securityResponse = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                if ( securityResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( securityResponse.Type , null );
                    return;
                }

                var securityContext = securityResponse.Result;

                #endregion

                var dataHandlerResponse = dataHandler.GetSchedulePersonnel_ScheduleId( scheduleId , securityContext );
                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Add<List<SchedulePerson>>( TamamCacheClusters.Schedules , cacheKey , dataHandlerResponse.Result );

                #endregion

                #endregion

            } , requestContext );
            return context.Response;
        }

        #endregion
        #region Schedule Personnel - Effective

        public ExecutionResponse<List<EffectiveSchedulePerson>> GetEffectiveSchedulePersonnel_ScheduleId( Guid scheduleId , RequestContext requestContext )
        {
            var context = new ExecutionContext<List<EffectiveSchedulePerson>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "SchedulesHandler_GetEffectiveSchedulePersonnel_ScheduleId" + scheduleId + requestContext;
                var cached = Broker.Cache.Get<List<EffectiveSchedulePerson>>( TamamCacheClusters.Schedules , cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                    actionId: TamamConstants.AuthorizationConstants.GetScheduleAuditActionId ,
                    actionKey: TamamConstants.AuthorizationConstants.GetScheduleActionKey ,
                    messageForDenied: string.Empty ,
                    messageForFailure: TamamConstants.AuthorizationConstants.GetScheduleAuditMessageFailure ,
                    messageForSuccess: string.Empty
                    );

                var securityResponse = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                if ( securityResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( securityResponse.Type , null );
                    return;
                }

                var securityContext = securityResponse.Result;

                #endregion

                var dataHandlerResponse = dataHandler.GetEffectiveSchedulePersonnel_ScheduleId( scheduleId , securityContext );
                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Add<List<EffectiveSchedulePerson>>( TamamCacheClusters.Schedules , cacheKey , dataHandlerResponse.Result );

                #endregion
                #endregion
            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<List<EffectiveSchedulePerson>> GetEffectiveSchedulePersonnel_PersonId( Guid personId , RequestContext requestContext )
        {
            var context = new ExecutionContext<List<EffectiveSchedulePerson>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "SchedulesHandler_GetEffectiveSchedulePersonnel_PersonId" + personId + requestContext;
                var cached = Broker.Cache.Get<List<EffectiveSchedulePerson>>( TamamCacheClusters.Schedules , cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                    actionId: TamamConstants.AuthorizationConstants.GetScheduleAuditActionId ,
                    actionKey: TamamConstants.AuthorizationConstants.GetScheduleActionKey ,
                    messageForDenied: string.Empty ,
                    messageForFailure: TamamConstants.AuthorizationConstants.GetScheduleAuditMessageFailure ,
                    messageForSuccess: string.Empty
                    );

                var securityResponse = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                if ( securityResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( securityResponse.Type , null );
                    return;
                }

                var securityContext = securityResponse.Result;

                #endregion

                var dataHandlerResponse = dataHandler.GetEffectiveSchedulePersonnel_PersonId( personId );
                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Add<List<EffectiveSchedulePerson>>( TamamCacheClusters.Schedules , cacheKey , dataHandlerResponse.Result );

                #endregion

                #endregion

            } , requestContext );

            return context.Response;
        }

        #endregion
        #region Schedule Department - Direct

        public ExecutionResponse<List<ScheduleDepartment>> GetScheduleDepartments_ScheduleId( Guid scheduleId , RequestContext requestContext )
        {
            var context = new ExecutionContext<List<ScheduleDepartment>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "SchedulesHandler_GetScheduleDepartments_ScheduleId" + scheduleId + requestContext;
                var cached = Broker.Cache.Get<List<ScheduleDepartment>>( TamamCacheClusters.Schedules , cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                    actionId: TamamConstants.AuthorizationConstants.GetScheduleAuditActionId ,
                    actionKey: TamamConstants.AuthorizationConstants.GetScheduleActionKey ,
                    messageForDenied: string.Empty ,
                    messageForFailure: TamamConstants.AuthorizationConstants.GetScheduleAuditMessageFailure ,
                    messageForSuccess: string.Empty
                    );

                var securityResponse = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                if ( securityResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( securityResponse.Type , null );
                    return;
                }

                var securityContext = securityResponse.Result;

                #endregion

                var dataHandlerResponse = dataHandler.GetScheduleDepartments_ScheduleId( scheduleId , securityContext );
                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Add<List<ScheduleDepartment>>( TamamCacheClusters.Schedules , cacheKey , dataHandlerResponse.Result );

                #endregion

                #endregion
            } , requestContext );

            return context.Response;
        }

        #endregion
        #region Schedule Department - Effective

        public ExecutionResponse<List<EffectiveScheduleDepartment>> GetEffectiveScheduleDepartments_ScheduleId( Guid scheduleId , RequestContext requestContext )
        {
            var context = new ExecutionContext<List<EffectiveScheduleDepartment>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "SchedulesHandler_GetEffectiveScheduleDepartments_ScheduleId" + scheduleId + requestContext;
                var cached = Broker.Cache.Get<List<EffectiveScheduleDepartment>>( TamamCacheClusters.Schedules , cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success , cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                    actionId: TamamConstants.AuthorizationConstants.GetScheduleAuditActionId ,
                    actionKey: TamamConstants.AuthorizationConstants.GetScheduleActionKey ,
                    messageForDenied: string.Empty ,
                    messageForFailure: TamamConstants.AuthorizationConstants.GetScheduleAuditMessageFailure ,
                    messageForSuccess: string.Empty
                    );

                var securityResponse = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                if ( securityResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( securityResponse.Type , null );
                    return;
                }

                var securityContext = securityResponse.Result;

                #endregion

                var dataHandlerResponse = dataHandler.GetEffectiveScheduleDepartments_ScheduleId( scheduleId );
                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Add<List<EffectiveScheduleDepartment>>( TamamCacheClusters.Schedules , cacheKey , dataHandlerResponse.Result );

                #endregion

                #endregion
            } , requestContext );

            return context.Response;
        }
        public ExecutionResponse<List<EffectiveScheduleDepartment>> GetEffectiveScheduleDepartments_DepartmentId( Guid departmentId , RequestContext requestContext )
        {
            var context = new ExecutionContext<List<EffectiveScheduleDepartment>>();
            context.Execute( () =>
            {
                #region logic ...
                #region Get Cache

                var cacheKey = "SchedulesHandler_GetEffectiveScheduleDepartments_DepartmentId" + departmentId +
                               requestContext;

                var cachedEffectiveScheduleDepartments =
                    Broker.Cache.Get<List<EffectiveScheduleDepartment>>( TamamCacheClusters.Schedules , cacheKey );

                if ( cachedEffectiveScheduleDepartments != null )
                {
                    context.Response.Set( ResponseState.Success , cachedEffectiveScheduleDepartments );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                    actionId: TamamConstants.AuthorizationConstants.GetScheduleAuditActionId ,
                    actionKey: TamamConstants.AuthorizationConstants.GetScheduleActionKey ,
                    messageForDenied: string.Empty ,
                    messageForFailure: TamamConstants.AuthorizationConstants.GetScheduleAuditMessageFailure ,
                    messageForSuccess: string.Empty
                    );

                var securityResponse = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                if ( securityResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( securityResponse.Type , null );
                    return;
                }

                var securityContext = securityResponse.Result;

                #endregion

                var dataHandlerResponse = dataHandler.GetEffectiveScheduleDepartments_DepartmentId( departmentId );
                context.Response.Set( dataHandlerResponse );
                #region Set Cache

                Broker.Cache.Add<List<EffectiveScheduleDepartment>>( TamamCacheClusters.Schedules , cacheKey ,
                    dataHandlerResponse.Result );

                #endregion
                #endregion
            } , requestContext );

            return context.Response;
        }

        #endregion

        // Recalculte Effective ...
        //public ExecutionResponse<bool> RecalculateEffectiveDepartmentsSchedules(RequestContext requestContext)
        //{
        //    var context = new ExecutionContext<bool>();
        //    context.Execute(() =>
        //    {
        //        #region logic ...

        //        #region Security

        //        context.ActionContext = new ActionContext
        //                                (
        //                                moduleId: TamamConstants.AuthorizationConstants.ScheduleAuditModuleId,
        //                                actionId: TamamConstants.AuthorizationConstants.EditScheduleAuditActionId,
        //                                actionKey: TamamConstants.AuthorizationConstants.EditScheduleActionKey,
        //                                messageForDenied: string.Empty,
        //                                messageForFailure: string.Empty,
        //                                messageForSuccess: string.Empty
        //                                );

        //        var securityResponse = TamamServiceBroker.Secure(context.ActionContext, requestContext);
        //        if (securityResponse.Type != ResponseState.Success)
        //        {
        //            context.Response.Set(securityResponse.Type, false);
        //            return false;
        //        }

        //        var securityContext = securityResponse.Result;

        //        #endregion

        //        #region Get departments
        //        var departmentsResponse = TamamServiceBroker.OrganizationHandler.GetDepartments(SystemRequestContext.Instance);
        //        if (departmentsResponse.Type != ResponseState.Success)
        //        {
        //            context.Response.Set(departmentsResponse.Type, false);
        //            return false;
        //        }
        //        var departments = departmentsResponse.Result;
        //        #endregion

        //        // var effectiveSchedulesDepartments_Result = new List<EffectiveScheduleDepartment>();
        //        var roots = departments.Where(d => d.ParentDepartmentId == null).ToList();
        //        foreach (var department in roots)
        //        {
        //            var recalculatationResponse = ReCalculateEffectiveSchedulesForDepartment(department);
        //            if (!recalculatationResponse.Result)
        //            {
        //                return context.Response.Set(ResponseState.SystemError, false);
        //            }
        //        }

        //        return context.Response.Set(ResponseState.Success, true);

        //        #endregion
        //    }, requestContext);

        //    return context.Response;
        //}
        public ExecutionResponse<bool> ReCalculateEffectiveSchedulesForDepartment( Guid departmentId )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                var getDepartmentResponse = TamamServiceBroker.OrganizationHandler.GetDepartment( departmentId , SystemRequestContext.Instance );
                if ( getDepartmentResponse.Type != ResponseState.Success )
                {
                    return context.Response.Set( ResponseState.SystemError , false );
                }
                var department = getDepartmentResponse.Result;

                var reCalculateEffectiveSchedulesForDepartmentResponse = ReCalculateEffectiveSchedulesForDepartment( department );
                if ( reCalculateEffectiveSchedulesForDepartmentResponse.Type != ResponseState.Success )
                {
                    return context.Response.Set( ResponseState.SystemError , false );
                }

                return context.Response.Set( ResponseState.Success , true );

            } , SystemRequestContext.Instance );

            return context.Response;
        }
        public ExecutionResponse<bool> ReCalculateEffectiveSchedulesForDepartment( Department department )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region Update Effective Schedules For Department

                var scheduleDepartmentsresponse = dataHandler.GetScheduleDepartments_DepartmentId( department.Id );
                if ( scheduleDepartmentsresponse.Type != ResponseState.Success )
                {
                    return context.Response.Set( ResponseState.SystemError , false );
                }
                var directSchedulesDepartments = scheduleDepartmentsresponse.Result;

                var parentEffectiveSchedules = new List<EffectiveScheduleDepartment>();
                if ( department.ParentDepartmentId.HasValue )
                {
                    var parentEffectiveSchedulesResponse = dataHandler.GetEffectiveScheduleDepartments_DepartmentId( department.ParentDepartmentId.Value );
                    if ( parentEffectiveSchedulesResponse.Type != ResponseState.Success )
                    {
                        return context.Response.Set( ResponseState.SystemError , false );
                    }
                    parentEffectiveSchedules = parentEffectiveSchedulesResponse.Result;
                }

                var effectiveSchedulesDepartmentResult = IntersectSchedules( department.Id , parentEffectiveSchedules , directSchedulesDepartments );

                var dataHandlerResponse = dataHandler.UpdateEffectiveSchedulesDepartments( department.Id , effectiveSchedulesDepartmentResult );
                if ( dataHandlerResponse.Type != ResponseState.Success )
                {
                    return context.Response.Set( ResponseState.SystemError , false );
                }

                #endregion
                #region Update Effective Schedules for Department Personnel

                var departmentPersonnelResponse = TamamServiceBroker.PersonnelHandler.GetPersonnel( new PersonSearchCriteria { Departments = new List<Guid> { department.Id } } , SystemRequestContext.Instance );
                if ( departmentPersonnelResponse.Type != ResponseState.Success )
                {
                    return context.Response.Set( ResponseState.SystemError , false );
                }
                var departmentPersonnel = departmentPersonnelResponse.Result.Persons;

                foreach ( var person in departmentPersonnel )
                {
                    var recalculateForPersonResponse = ReCalculateEffectiveSchedulesForPerson( person );
                    if ( recalculateForPersonResponse.Type != ResponseState.Success )
                    {
                        return context.Response.Set( ResponseState.SystemError , false );
                    }
                }

                #endregion
                #region Recursion: Update Effective Schedules For child Departments

                var childDepartmentsResponse = TamamServiceBroker.OrganizationHandler.GetDepartmentsByParentId( department.Id , SystemRequestContext.Instance );
                if ( childDepartmentsResponse.Type != ResponseState.Success )
                {
                    return context.Response.Set( ResponseState.SystemError , false );
                }
                var childDepartments = childDepartmentsResponse.Result;

                foreach ( var childDepartment in childDepartments )
                {
                    var recalculatationResponse = ReCalculateEffectiveSchedulesForDepartment( childDepartment );
                    if ( recalculatationResponse.Type != ResponseState.Success )
                    {
                        return context.Response.Set( ResponseState.SystemError , false );
                    }
                }

                #endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Schedules );

                #endregion

                return context.Response.Set( ResponseState.Success , true );

            } , SystemRequestContext.Instance );
            return context.Response;
        }
        public ExecutionResponse<bool> ReCalculateEffectiveSchedulesForPerson( Guid personId )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                var getPersonresponse = TamamServiceBroker.PersonnelHandler.GetPerson( personId , SystemRequestContext.Instance );
                if ( getPersonresponse.Type != ResponseState.Success )
                {
                    return context.Response.Set( ResponseState.SystemError , false );
                }

                var person = getPersonresponse.Result;
                var reCalculateEffectiveSchedulesForPersonResponse = ReCalculateEffectiveSchedulesForPerson( person );
                if ( reCalculateEffectiveSchedulesForPersonResponse.Type != ResponseState.Success )
                {
                    return context.Response.Set( ResponseState.SystemError , false );
                }

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Schedules );

                #endregion

                return context.Response.Set( ResponseState.Success , true );
            } , SystemRequestContext.Instance );

            return context.Response;
        }
        public ExecutionResponse<bool> ReCalculateEffectiveSchedulesForPerson( Person person )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                if ( person.AccountInfo.Activated )
                {
                    #region Update Effective Schedules For Person

                    var directSchedulesPersonnelResponse = dataHandler.GetSchedulePersonnel_PersonId( person.Id );
                    if ( directSchedulesPersonnelResponse.Type != ResponseState.Success )
                    {
                        return context.Response.Set( ResponseState.Failure , false );
                    }
                    var directSchedulesDepartments = directSchedulesPersonnelResponse.Result;

                    var parentEffectiveSchedulesResponse =
                        dataHandler.GetEffectiveScheduleDepartments_DepartmentId( person.AccountInfo.DepartmentId );
                    if ( parentEffectiveSchedulesResponse.Type != ResponseState.Success )
                    {
                        return context.Response.Set( ResponseState.Failure , false );
                    }
                    var parentEffectiveSchedules = parentEffectiveSchedulesResponse.Result;


                    var effectiveSchedulesPersonnelResult = IntersectSchedules( person.Id , person.AccountInfo.JoinDate ,
                        parentEffectiveSchedules , directSchedulesDepartments );

                    var DataHandlerResponse = dataHandler.UpdateEffectiveSchedulesPersonnel( person.Id ,
                        effectiveSchedulesPersonnelResult );
                    if ( DataHandlerResponse.Type != ResponseState.Success )
                    {
                        return context.Response.Set( ResponseState.SystemError , false );
                    }


                    #endregion
                }
                else
                {
                    var dataHandlerResponse = dataHandler.ClearEffectiveSchedulesPersonnel( person.Id );
                    if ( dataHandlerResponse.Type != ResponseState.Success )
                    {
                        return context.Response.Set( ResponseState.SystemError , false );
                    }
                }

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Schedules );

                #endregion

                return context.Response.Set( ResponseState.Success , true );

            } , SystemRequestContext.Instance );

            return context.Response;
        }

        #region [ NEW ]

        //public ExecutionResponse<List<ScheduleDay>> GetScheduleDetailsX( Schedule schedule , List<Guid> departmentsIds , List<Guid> personnelIds , DateTime startDate , DateTime endDate , RequestContext requestContext )
        //{
        //    var context = new ExecutionContext<List<ScheduleDay>>();
        //    context.Execute( () =>
        //    {
        //        #region logic ...

        //        #region Cache

        //        var cacheKey = "SchedulesHandler_GetScheduleDetails" + schedule.Id + ListToString<Guid>( departmentsIds ) + ListToString<Guid>( personnelIds ) + startDate.ToShortDateString() + endDate.ToShortDateString() + requestContext;
        //        var cached = Broker.Cache.Get<List<ScheduleDay>>( TamamCacheClusters.Schedules , cacheKey );
        //        if ( cached != null )
        //        {
        //            context.Response.Set( ResponseState.Success , cached );
        //            return;
        //        }

        //        #endregion
        //        #region Security

        //        context.ActionContext = new ActionContext
        //            (
        //            moduleId: TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
        //            actionId: TamamConstants.AuthorizationConstants.GetScheduleAuditActionId ,
        //            actionKey: TamamConstants.AuthorizationConstants.GetScheduleActionKey ,
        //            messageForDenied: string.Empty ,
        //            messageForFailure: TamamConstants.AuthorizationConstants.GetScheduleAuditMessageFailure ,
        //            messageForSuccess: string.Empty
        //            );

        //        var securityResponse = TamamServiceBroker.Secure( context.ActionContext , requestContext );
        //        if ( securityResponse.Type != ResponseState.Success )
        //        {
        //            context.Response.Set( securityResponse.Type , null );
        //            return;
        //        }

        //        var securityContext = securityResponse.Result;

        //        #endregion
        //        #region validation

        //        if ( endDate < startDate )
        //        {
        //            context.Response.Set( ResponseState.ValidationError , null , new List<ModelMetaPair> { new ModelMetaPair( string.Empty , "Invalid date range" ) } );
        //            return;
        //        }

        //        #endregion
        //        #region Init ...

        //        var schedulesDetails = new List<ScheduleDay>();

        //        #endregion
        //        #region SchedulePersonnel

        //        var schedulePersonnel = new List<EffectiveSchedulePerson>();

        //        if ( personnelIds != null && personnelIds.Any() )
        //        {
        //            var criteria = new SchedulesSearchCriteria()
        //            {
        //                ScheduleIds = new List<Guid>() { schedule.Id , } ,
        //                PersonnelIds = personnelIds ,
        //                DepartmentsIds = departmentsIds ,
        //            };

        //            var effectiveSchedulesResponse = GetEffectiveSchedules( criteria , requestContext );
        //            if ( effectiveSchedulesResponse.Type != ResponseState.Success )
        //            {
        //                context.Response.Set( ResponseState.SystemError , null );
        //                return;
        //            }
        //            schedulePersonnel.AddRange( effectiveSchedulesResponse.Result );
        //        }

        //        // here ... 
        //        foreach ( var schedulePerson in schedulePersonnel )
        //        {
        //            //XIntervals.Intersect( new XIntervals.TimeRange(startDate,endDate) ,  );

        //            var startDateFilter = GetInterSectedStartDate( startDate , endDate , schedulePerson.StartDate , schedulePerson.EndDate );
        //            var endDateFilter = GetInterSectedEndDate( startDate , endDate , schedulePerson.StartDate , schedulePerson.EndDate );

        //            if ( !startDateFilter.HasValue || !endDateFilter.HasValue ) continue;
        //            var getDetailsResponse = GetScheduleDetails( schedule , startDateFilter.Value , endDateFilter.Value , requestContext );
        //            if ( getDetailsResponse.Type != ResponseState.Success )
        //            {
        //                context.Response.Set( ResponseState.SystemError , null );
        //                return;
        //            }

        //            foreach ( var sDetail in getDetailsResponse.Result )
        //            {
        //                if ( !schedulesDetails.Any( sd => sd.ScheduleId == sDetail.ScheduleId && sd.Day == sDetail.Day ) )
        //                    schedulesDetails.Add( sDetail );
        //            }
        //        }

        //        #endregion
        //        #region ScheduleDepartments

        //        if ( departmentsIds != null && departmentsIds.Any() )
        //        {
        //            var scheduleDepartments = new List<EffectiveScheduleDepartment>();
        //            foreach ( var departmentId in departmentsIds )
        //            {
        //                var scheduleDepartmentsResponse = GetEffectiveScheduleDepartments_DepartmentId( departmentId , requestContext );
        //                if ( scheduleDepartmentsResponse.Type != ResponseState.Success )
        //                {
        //                    context.Response.Set( ResponseState.SystemError , null );
        //                    return;
        //                }
        //                var scheduleDepartments_FilteredByDepartment = scheduleDepartmentsResponse.Result;
        //                var schedulePersonnel_FilteredByDepartmentAndSchedule = scheduleDepartments_FilteredByDepartment.Where( sp => sp.ScheduleId == schedule.Id );
        //                scheduleDepartments.AddRange( schedulePersonnel_FilteredByDepartmentAndSchedule );
        //            }

        //            foreach ( var scheduleDepartment in scheduleDepartments )
        //            {
        //                var startDateFilter = GetInterSectedStartDate( startDate , endDate , scheduleDepartment.StartDate , scheduleDepartment.EndDate );
        //                var endDateFilter = GetInterSectedEndDate( startDate , endDate , scheduleDepartment.StartDate , scheduleDepartment.EndDate );

        //                if ( !startDateFilter.HasValue || !endDateFilter.HasValue ) continue;
        //                var getDetailsResponse = GetScheduleDetails( schedule , startDateFilter.Value , endDateFilter.Value , requestContext );
        //                if ( getDetailsResponse.Type != ResponseState.Success )
        //                {
        //                    context.Response.Set( ResponseState.SystemError , null );
        //                    return;
        //                }

        //                foreach ( var sDetail in getDetailsResponse.Result )
        //                {
        //                    if (
        //                        !schedulesDetails.Any( sd => sd.ScheduleId == sDetail.ScheduleId && sd.Day == sDetail.Day ) )
        //                    {
        //                        schedulesDetails.Add( sDetail );
        //                    }
        //                }
        //            }
        //        }

        //        schedulesDetails = schedulesDetails.OrderBy( sd => sd.Day ).ToList();

        //        #endregion

        //        context.Response.Set( ResponseState.Success , schedulesDetails );

        //        #region Cache

        //        Broker.Cache.Add<List<ScheduleDay>>( TamamCacheClusters.Schedules , cacheKey , schedulesDetails );

        //        #endregion

        //        #endregion
        //    } , requestContext );
        //    return context.Response;
        //}
        //public ExecutionResponse<List<EffectiveSchedulePerson>> GetEffectiveSchedules( SchedulesSearchCriteria criteria , RequestContext requestContext )
        //{
        //    var context = new ExecutionContext<List<EffectiveSchedulePerson>>();
        //    context.Execute( () =>
        //    {
        //        #region logic ...

        //        #region Cache

        //        var cacheKey = "SchedulesHandler_GetEffectiveSchedules" + criteria + requestContext;
        //        var cached = Broker.Cache.Get<List<EffectiveSchedulePerson>>( TamamCacheClusters.Schedules , cacheKey );
        //        if ( cached != null )
        //        {
        //            context.Response.Set( ResponseState.Success , cached );
        //            return;
        //        }

        //        #endregion
        //        #region Security

        //        context.ActionContext = new ActionContext
        //                                (
        //                                moduleId: TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
        //                                actionId: TamamConstants.AuthorizationConstants.GetScheduleAuditActionId ,
        //                                actionKey: TamamConstants.AuthorizationConstants.GetScheduleActionKey ,
        //                                messageForDenied: string.Empty ,
        //                                messageForFailure: TamamConstants.AuthorizationConstants.GetScheduleAuditMessageFailure ,
        //                                messageForSuccess: string.Empty
        //                                );

        //        var securityResponse = TamamServiceBroker.Secure( context.ActionContext , requestContext );
        //        if ( securityResponse.Type != ResponseState.Success )
        //        {
        //            context.Response.Set( securityResponse.Type , null );
        //            return;
        //        }

        //        var securityContext = securityResponse.Result;

        //        #endregion
        //        #region datalayer

        //        var response = dataHandler.GetEffectiveSchedules( criteria , securityContext );
        //        context.Response.Set( response );

        //        #endregion
        //        #region Cache

        //        Broker.Cache.Add<List<EffectiveSchedulePerson>>( TamamCacheClusters.Schedules , cacheKey , response.Result );

        //        #endregion

        //        #endregion

        //    } , requestContext );

        //    return context.Response;
        //}

        #endregion

        #endregion

        #endregion
        #region ISystemSchedulesHandler

        public ExecutionResponse<bool> IsValidWorkingHours( Guid personId , DateTime date , TimeSpan from , TimeSpan to )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                var duration = GetScheduledHoursCount( personId , date , from , to ).Result;
                var isValidTimes = duration == 0;

                // ...
                context.Response.Set( ResponseState.Success , isValidTimes );

                #endregion
            } , SystemRequestContext.Instance );
            return context.Response;
        }
        public ExecutionResponse<double> GetScheduledHoursCount( Guid personId , DateTime date , TimeSpan from , TimeSpan to )
        {
            var context = new ExecutionContext<double>();
            context.Execute( () =>
            {
                #region logic ...

                // available shifts ...
                var workingShifts = GetScheduledShifts( personId , date ).Result;
                var shifts = workingShifts.Select( x => x.Shift ).ToList();
                //The effective excuse duration will be the large intersected duration between the excuse times and shift times
                var excuse = new Excuse() { ExcuseDate = date , PersonId = personId , StartTime = date.Date.Add( from ) , EndTime = date.Date.Add( to ) };
                var duration = default( double ); var intersectDuration = default( double );
                foreach ( var shift in shifts )
                {
                    intersectDuration = GetExcuseEffectiveTime( excuse , shift ).Result;
                    if ( intersectDuration > duration )
                    {
                        duration = intersectDuration;
                    }
                }

                // ...
                context.Response.Set( ResponseState.Success , duration );

                #endregion
            } , SystemRequestContext.Instance );
            return context.Response;
        }
        public ExecutionResponse<double> GetExcuseEffectiveTime( Excuse excuse , Shift shift )
        {
            var context = new ExecutionContext<double>();
            context.Execute( () =>
            {
                #region logic ...

                // grace time ...
                var policy = GetShiftPolicy( GetPolicies( excuse.PersonId ) , shift ).Result;
                var shiftPolicy = new ShiftPolicy( policy );
                var graceEndTime = new TimeSpan( 0 , shiftPolicy.LateComeGrace , 0 );
                var nightShiftDelta = 0;
                var excuseTimeRange = new XIntervals.TimeRange( excuse.StartTime.TimeOfDay , excuse.EndTime.TimeOfDay );
                // ...                
                //if start time or end time didn't have value -flexible shift- so return the excuse duration as effective time
                if ( !shift.StartTime.HasValue || !shift.EndTime.HasValue )
                {
                    context.Response.Set( ResponseState.Success , excuseTimeRange.Hours.Value );
                    return;
                }
                if ( shift.IsNightShift )
                {
                    //if excuse end time is in shift range so the shift end is large than excuse so the delta will be 23-shift end
                    //else the end of the excuse will be the large time so the delta will be 23 - excuse end
                    //delta is the number of hour that need to be added on all times to make it in the same day
                    //so it will be the different between 23 -the last hour in the day- and the lager number i have in this handle
                    nightShiftDelta = 23 - ( ( excuse.EndTime.Hour >= shift.StartTime.Value.Hour || excuse.EndTime.Hour <= shift.EndTime.Value.Add( graceEndTime ).Hour ) ? shift.EndTime.Value.Add( graceEndTime ).Hour : ( excuse.EndTime.Hour ) );
                    excuseTimeRange = new XIntervals.TimeRange( ( excuse.StartTime.AddHours( nightShiftDelta ).TimeOfDay ) , excuse.EndTime.TimeOfDay );
                }

                var shiftTimeRange = new XIntervals.TimeRange(
                   shift.IsNightShift ? ( shift.StartTime.Value.AddHours( nightShiftDelta ).TimeOfDay ) : shift.StartTime.Value.TimeOfDay ,
                   shift.IsNightShift ? ( shift.EndTime.Value.Add( graceEndTime ).AddHours( nightShiftDelta ).TimeOfDay ) : shift.EndTime.Value.Add( graceEndTime ).TimeOfDay );

                var x = XIntervals.Intersect( excuseTimeRange , shiftTimeRange );

                var duration = Math.Min( x.Hours.GetValueOrDefault() , ( double ) shift.Duration );

                context.Response.Set( ResponseState.Success , duration );

                #endregion
            } , SystemRequestContext.Instance );
            return context.Response;
        }

        public ExecutionResponse<List<ScheduleTemplateDayShifts>> GetScheduledShifts( Guid personId , DateTime date )
        {
            var context = new ExecutionContext<List<ScheduleTemplateDayShifts>>();
            context.Execute( () =>
            {
                #region logic ...

                var shiftsDays = new List<ScheduleTemplateDayShifts>();

                // working days ...
                var schedules = GetSchedules( personId , date , date );
                var scheduleDays = GetSchedulesDays( personId , date , date , schedules );
                scheduleDays = GetSchedulesDaysEffective( personId , date , date , scheduleDays , false , false );

                // ...
                if ( scheduleDays != null && scheduleDays.Count > 0 )
                {
                    shiftsDays.AddRange( scheduleDays[0].DayShifts );
                }

                // ...
                context.Response.Set( ResponseState.Success , shiftsDays );

                #endregion
            } , SystemRequestContext.Instance );
            return context.Response;
        }
        public ExecutionResponse<List<DateTime>> GetScheduledDays( Guid personId , DateTime from , DateTime to , bool includeOffDays , bool includeLeaves )
        {
            var context = new ExecutionContext<List<DateTime>>();
            context.Execute( () =>
            {
                #region logic ...

                var schedules = GetSchedules( personId , from , to );
                var scheduleDays = GetSchedulesDays( personId , from , to , schedules );
                scheduleDays = GetSchedulesDaysEffective( personId , from , to , scheduleDays , includeOffDays , includeLeaves );

                // ...
                var days = scheduleDays.Select( x => x.Day ).Distinct().ToList();

                // ...
                context.Response.Set( ResponseState.Success , days );

                #endregion
            } , SystemRequestContext.Instance );
            return context.Response;
        }
        public ExecutionResponse<int> GetScheduledDaysCount( Guid personId , DateTime from , DateTime to , bool includeOffDays , bool includeLeaves )
        {
            var context = new ExecutionContext<int>();
            context.Execute( () =>
            {
                #region logic ...

                var days = GetScheduledDays( personId , from , to , includeOffDays , includeLeaves ).Result;
                var count = days != null ? days.Count : 0;

                // ...
                context.Response.Set( ResponseState.Success , count );

                #endregion
            } , SystemRequestContext.Instance );
            return context.Response;
        }

        // Policies
        public ExecutionResponse<Policy> GetShiftPolicy( List<Policy> policies , Shift shift )
        {
            var context = new ExecutionContext<Policy>();
            context.Execute( () =>
            {
                #region logic ...

                if ( policies == null )
                {
                    context.Response.Set( ResponseState.SystemError , null );
                    return;
                }

                var policy = policies.FirstOrDefault( x => x.PolicyTypeId == Guid.Parse( PolicyTypes.ShiftPolicyType ) );
                if ( policy != null )
                {
                    context.Response.Set( ResponseState.Success , policy );
                }
                else
                {
                    if ( shift == null )
                    {
                        context.Response.Set( ResponseState.NotFound , null );
                        return;
                    }

                    var response = TamamServiceBroker.OrganizationHandler.GetPolicy( shift.ShiftPolicyId , SystemRequestContext.Instance );
                    context.Response.Set( response );
                }

                #endregion

            } , SystemRequestContext.Instance );
            return context.Response;
        }
        public ExecutionResponse<Policy> GetShiftPolicy( Guid personId , Shift shift )
        {
            var context = new ExecutionContext<Policy>();
            context.Execute( () =>
            {
                #region logic ...

                var response = TamamServiceBroker.OrganizationHandler.GetPolicies( personId , SystemRequestContext.Instance );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type , null );
                    return;
                }

                var policies = response.Result;
                context.Response.Set( GetShiftPolicy( policies , shift ) );

                #endregion

            } , SystemRequestContext.Instance );
            return context.Response;
        }

        #endregion
        #region IReadOnlySchedulesHandler

        public ExecutionResponse<List<ScheduleDetailDTO>> GetScheduleDetailsDTOs( Guid personId , RequestContext requestContext )
        {
            var context = new ExecutionContext<List<ScheduleDetailDTO>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                //var cacheKey = "SchedulesHandler_GetScheduleDetails" + schedule.Id + startDate.ToShortDateString() + endDate.ToShortDateString() + requestContext;
                //var cached = Broker.Cache.Get<List<ScheduleDay>>( TamamCacheClusters.Schedules , cacheKey );
                //if ( cached != null )
                //{
                //    context.Response.Set( ResponseState.Success , cached );
                //    return;
                //}

                #endregion
                #region Security

                //context.ActionContext = new ActionContext
                //    (
                //    moduleId: TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                //    actionId: TamamConstants.AuthorizationConstants.GetScheduleAuditActionId ,
                //    actionKey: TamamConstants.AuthorizationConstants.GetScheduleActionKey ,
                //    messageForDenied: string.Empty ,
                //    messageForFailure: TamamConstants.AuthorizationConstants.GetScheduleAuditMessageFailure ,
                //    messageForSuccess: string.Empty
                //    );

                //var securityResponse = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                //if ( securityResponse.Type != ResponseState.Success )
                //{
                //    context.Response.Set( securityResponse.Type , null );
                //    return;
                //}

                //var securityContext = securityResponse.Result;

                #endregion

                var response = dataHandler.GetScheduleDetailsDTO( personId );
                context.Response.Set( response );

                #region Cache

                //Broker.Cache.Add<List<ScheduleDay>>( TamamCacheClusters.Schedules , cacheKey , result );

                #endregion

                #endregion
            } , requestContext );
            return context.Response;
        }
        public ExecutionResponse<bool> SetScheduleDetailsDTOs( List<ScheduleDetailDTO> DTOs , RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                //context.ActionContext = new ActionContext
                //    (
                //    moduleId: TamamConstants.AuthorizationConstants.ScheduleAuditModuleId ,
                //    actionId: TamamConstants.AuthorizationConstants.GetScheduleAuditActionId ,
                //    actionKey: TamamConstants.AuthorizationConstants.GetScheduleActionKey ,
                //    messageForDenied: string.Empty ,
                //    messageForFailure: TamamConstants.AuthorizationConstants.GetScheduleAuditMessageFailure ,
                //    messageForSuccess: string.Empty
                //    );

                //var securityResponse = TamamServiceBroker.Secure( context.ActionContext , requestContext );
                //if ( securityResponse.Type != ResponseState.Success )
                //{
                //    context.Response.Set( securityResponse.Type , null );
                //    return;
                //}

                //var securityContext = securityResponse.Result;

                #endregion

                var response = dataHandler.SetScheduleDetailsDTOs( DTOs );
                context.Response.Set( response );

                #endregion
            } , requestContext );
            return context.Response;
        }

        #endregion

        #region Helpers

        private List<EffectiveScheduleDepartment> IntersectSchedules( Guid departmentId , List<EffectiveScheduleDepartment> parentEffectiveSchedules , List<ScheduleDepartment> directAssociatedSchedules )
        {
            // validate ...
            var emptyA = parentEffectiveSchedules == null || parentEffectiveSchedules.Count == 0;
            var emptyB = directAssociatedSchedules == null || directAssociatedSchedules.Count == 0;

            if ( emptyA && emptyB ) return new List<EffectiveScheduleDepartment>();
            if ( emptyA )
                return
                    directAssociatedSchedules.Select(
                        das =>
                            new EffectiveScheduleDepartment()
                            {
                                ScheduleId = das.ScheduleId ,
                                StartDate = das.StartDate ,
                                EndDate = das.EndDate ,
                                DepartmentId = departmentId
                            } ).ToList();
            if ( emptyB )
                return
                    parentEffectiveSchedules.Select(
                        pes =>
                            new EffectiveScheduleDepartment()
                            {
                                ScheduleId = pes.ScheduleId ,
                                StartDate = pes.StartDate ,
                                EndDate = pes.EndDate ,
                                DepartmentId = departmentId
                            } ).ToList();
            ;

            // ReplaceNullsWithMaxDates ...
            parentEffectiveSchedules.Where( x => x.EndDate == null ).ForEach( x => x.EndDate = DateTime.MaxValue.Date );
            directAssociatedSchedules.Where( x => x.EndDate == null ).ForEach( x => x.EndDate = DateTime.MaxValue.Date );

            // ...
            var resultRanges = new List<EffectiveScheduleDepartment>();

            // order ...
            parentEffectiveSchedules = parentEffectiveSchedules.OrderBy( x => x.StartDate ).ToList();
            directAssociatedSchedules = directAssociatedSchedules.OrderBy( x => x.StartDate ).ToList();

            // priority : B ...
            resultRanges.AddRange(
                directAssociatedSchedules.Select(
                    das =>
                        new EffectiveScheduleDepartment()
                        {
                            ScheduleId = das.ScheduleId ,
                            StartDate = das.StartDate ,
                            EndDate = das.EndDate ,
                            DepartmentId = departmentId
                        } ).ToList() );

            // global range ...
            var GStart = XDate.Min( parentEffectiveSchedules[0].StartDate , directAssociatedSchedules[0].StartDate );
            var GEnd = XDate.Max( parentEffectiveSchedules[parentEffectiveSchedules.Count - 1].EndDate ,
                directAssociatedSchedules[directAssociatedSchedules.Count - 1].EndDate );

            // gaps ...
            for ( int i = 0 ; i < directAssociatedSchedules.Count ; i++ )
            {
                if ( GStart < directAssociatedSchedules[i].StartDate )
                {
                    var gap = new EffectiveScheduleDepartment()
                    {
                        StartDate = GStart.Value ,
                        EndDate = directAssociatedSchedules[i].StartDate.AddDays( -1 ) ,
                        DepartmentId = departmentId
                    };

                    resultRanges.AddRange( IntersectSchedulesWithGap( parentEffectiveSchedules , gap ) );
                }

                if ( directAssociatedSchedules[i].EndDate.Value != DateTime.MaxValue.Date )
                {
                    GStart = directAssociatedSchedules[i].EndDate.AddDays( 1 );
                }
                else
                {
                    break;
                }
            }

            // last gap ...
            if ( directAssociatedSchedules.Last().EndDate.Value != DateTime.MaxValue.Date )
            {
                if ( GStart <= GEnd )
                {
                    var gap = new EffectiveScheduleDepartment()
                    {
                        StartDate = GStart.Value ,
                        EndDate = GEnd ,
                        DepartmentId = departmentId
                    };
                    resultRanges.AddRange( IntersectSchedulesWithGap( parentEffectiveSchedules , gap ) );
                }
            }

            // ReplaceMaxDatesWithNulls ...
            resultRanges.Where( x => x.EndDate >= DateTime.MaxValue.AddDays( -1 ) ).ForEach( x => x.EndDate = null );

            var finalResult = resultRanges.OrderBy( x => x.StartDate ).ToList();

            return finalResult;
        }
        private List<EffectiveScheduleDepartment> IntersectSchedulesWithGap( List<EffectiveScheduleDepartment> parentEffectiveSchedules , EffectiveScheduleDepartment scheduleGap )
        {
            var resultRanges = new List<EffectiveScheduleDepartment>();
            var intersectedA =
                parentEffectiveSchedules.Where(
                    x => scheduleGap.StartDate <= x.EndDate && x.StartDate <= scheduleGap.EndDate ).ToList();

            foreach ( var effectiveSchedule in intersectedA )
            {
                resultRanges.Add(
                    new EffectiveScheduleDepartment()
                    {
                        ScheduleId = effectiveSchedule.ScheduleId ,
                        DepartmentId = scheduleGap.DepartmentId ,
                        StartDate = XDate.Max( scheduleGap.StartDate , effectiveSchedule.StartDate ).Value ,
                        EndDate = XDate.Min( scheduleGap.EndDate , effectiveSchedule.EndDate )
                    } );
            }

            return resultRanges;
        }
        private List<EffectiveSchedulePerson> IntersectSchedules( Guid personId , DateTime? personJoinDate , List<EffectiveScheduleDepartment> parentEffectiveSchedules , List<SchedulePerson> directAssociatedSchedules )
        {
            // ...
            var resultRanges = new List<EffectiveSchedulePerson>();

            // validate ...
            var emptyA = parentEffectiveSchedules == null || parentEffectiveSchedules.Count == 0;
            var emptyB = directAssociatedSchedules == null || directAssociatedSchedules.Count == 0;

            if ( emptyA && emptyB ) return new List<EffectiveSchedulePerson>();

            if ( emptyA )
            {
                foreach ( var das in directAssociatedSchedules )
                {
                    if ( !das.EndDate.HasValue || ( personJoinDate.HasValue && das.EndDate >= personJoinDate.Value ) )
                    {
                        var effectiveSchedulePerson = new EffectiveSchedulePerson();
                        effectiveSchedulePerson.ScheduleId = das.ScheduleId;
                        effectiveSchedulePerson.StartDate = ( personJoinDate.HasValue && personJoinDate > das.StartDate )
                            ? personJoinDate.Value
                            : das.StartDate;
                        effectiveSchedulePerson.EndDate = das.EndDate;
                        effectiveSchedulePerson.PersonId = personId;

                        resultRanges.Add( effectiveSchedulePerson );
                    }
                }

                return resultRanges;
            }
            if ( emptyB )
            {
                foreach ( var pes in parentEffectiveSchedules )
                {
                    if ( !pes.EndDate.HasValue || ( personJoinDate.HasValue && pes.EndDate >= personJoinDate.Value ) )
                    {
                        var effectiveSchedulePerson = new EffectiveSchedulePerson();
                        effectiveSchedulePerson.ScheduleId = pes.ScheduleId;
                        effectiveSchedulePerson.StartDate = ( personJoinDate.HasValue && personJoinDate > pes.StartDate )
                            ? personJoinDate.Value
                            : pes.StartDate;
                        effectiveSchedulePerson.EndDate = pes.EndDate;
                        effectiveSchedulePerson.PersonId = personId;

                        resultRanges.Add( effectiveSchedulePerson );
                    }
                }

                return resultRanges;
            }

            // ReplaceNullsWithMaxDates ...
            parentEffectiveSchedules.Where( x => x.EndDate == null ).ForEach( x => x.EndDate = DateTime.MaxValue.Date );
            directAssociatedSchedules.Where( x => x.EndDate == null ).ForEach( x => x.EndDate = DateTime.MaxValue.Date );


            // order ...
            parentEffectiveSchedules = parentEffectiveSchedules.OrderBy( x => x.StartDate ).ToList();
            directAssociatedSchedules = directAssociatedSchedules.OrderBy( x => x.StartDate ).ToList();

            // priority : B ...
            resultRanges.AddRange(
                directAssociatedSchedules.Select(
                    das =>
                        new EffectiveSchedulePerson()
                        {
                            ScheduleId = das.ScheduleId ,
                            StartDate = das.StartDate ,
                            EndDate = das.EndDate ,
                            PersonId = personId
                        } ).ToList() );

            // global range ...
            var GStart = XDate.Min( parentEffectiveSchedules[0].StartDate , directAssociatedSchedules[0].StartDate );
            GStart = XDate.Max( GStart , personJoinDate.HasValue ? personJoinDate.Value : DateTime.MinValue );
            // if hire date is greater than association date, then take the hire date ...
            var GEnd = XDate.Max( parentEffectiveSchedules[parentEffectiveSchedules.Count - 1].EndDate ,
                directAssociatedSchedules[directAssociatedSchedules.Count - 1].EndDate );

            if ( GStart > GEnd ) return new List<EffectiveSchedulePerson>();

            // gaps ...
            for ( int i = 0 ; i < directAssociatedSchedules.Count ; i++ )
            {
                if ( GStart < directAssociatedSchedules[i].StartDate )
                {
                    var gap = new EffectiveSchedulePerson()
                    {
                        StartDate = GStart.Value ,
                        EndDate = directAssociatedSchedules[i].StartDate.AddDays( -1 ) ,
                        PersonId = personId
                    };

                    resultRanges.AddRange( IntersectSchedulesWithGap( parentEffectiveSchedules , gap ) );
                }

                if ( directAssociatedSchedules[i].EndDate.Value != DateTime.MaxValue.Date )
                {
                    GStart = directAssociatedSchedules[i].EndDate.AddDays( 1 );
                }
                else
                {
                    break;
                }
            }

            // last gap ...
            if ( directAssociatedSchedules.Last().EndDate.Value != DateTime.MaxValue.Date )
            {
                if ( GStart <= GEnd )
                {
                    var gap = new EffectiveSchedulePerson()
                    {
                        StartDate = GStart.Value ,
                        EndDate = GEnd ,
                        PersonId = personId
                    };

                    resultRanges.AddRange( IntersectSchedulesWithGap( parentEffectiveSchedules , gap ) );
                }
            }

            // ReplaceMaxDatesWithNulls ...
            resultRanges.Where( x => x.EndDate >= DateTime.MaxValue.AddDays( -1 ) ).ForEach( x => x.EndDate = null );
            return resultRanges.OrderBy( x => x.StartDate ).ToList();
        }
        private List<EffectiveSchedulePerson> IntersectSchedulesWithGap( List<EffectiveScheduleDepartment> parentEffectiveSchedules , EffectiveSchedulePerson scheduleGap )
        {
            var resultRanges = new List<EffectiveSchedulePerson>();
            var intersectedA =
                parentEffectiveSchedules.Where(
                    x => scheduleGap.StartDate <= x.EndDate && x.StartDate <= scheduleGap.EndDate ).ToList();

            foreach ( var effectiveSchedule in intersectedA )
            {
                resultRanges.Add(
                    new EffectiveSchedulePerson()
                    {
                        ScheduleId = effectiveSchedule.ScheduleId ,
                        PersonId = scheduleGap.PersonId ,
                        StartDate = XDate.Max( scheduleGap.StartDate , effectiveSchedule.StartDate ).Value ,
                        EndDate = XDate.Min( scheduleGap.EndDate , effectiveSchedule.EndDate )
                    } );
            }

            return resultRanges;
        }
        private DateTime? GetInterSectedStartDate( DateTime d1Start , DateTime d1End , DateTime d2Start , DateTime? d2End )
        {
            d2End = ( d2End.HasValue ) ? d2End.Value.Date : DateTime.MaxValue;

            // d1                |-------------|
            // d2          |-------------|
            if ( d1Start.Date >= d2Start.Date && d1Start.Date <= d2End.Value.Date ) return d1Start.Date;

            // d1     |-------------|
            // d2            |-------------|
            if ( d1Start.Date <= d2Start.Date && d1End.Date >= d2Start.Date ) return d2Start.Date;


            // d1    |----------|
            // d2                   |---------|
            // OR
            // d1                   |---------|
            // d2     |---------|
            return null;
        }
        private DateTime? GetInterSectedEndDate( DateTime d1Start , DateTime d1End , DateTime d2Start , DateTime? d2End )
        {
            if ( !d2End.HasValue ) return d1End.Date;

            // d1      |-------------|
            // d2               |-------------|
            if ( d1End.Date >= d2Start.Date && d1End.Date <= d2End.Value.Date ) return d1End.Date;

            // d1              |-------------|
            // d2       |-------------|
            if ( d1Start.Date <= d2End.Value.Date && d1End.Date >= d2End.Value.Date ) return d2End.Value.Date;


            // d1    |----------|
            // d2                   |---------|
            // OR
            // d1                   |---------|
            // d2     |---------|
            return null;
        }

        private List<Holiday> GetHolidays( Guid personId , DateTime from , DateTime to )
        {
            return TamamServiceBroker.OrganizationHandler.GetHolidays( personId , from , to , SystemRequestContext.Instance ).Result;
        }
        internal List<Leave> GetActiveLeaves( Guid personId , DateTime from , DateTime to )
        {
            var criteria = new LeaveSearchCriteria( new List<Guid> { personId } , null , from , to , null , null ,
                                                    new List<int> { ( int ) LeaveStatus.Planned , ( int ) LeaveStatus.Pending , ( int ) LeaveStatus.Approved , ( int ) LeaveStatus.Taken } , false , 0 , 0 );

            var response = TamamServiceBroker.LeavesHandler.SearchLeaves( criteria , true , SystemRequestContext.Instance );
            return response.Type == ResponseState.Success ? response.Result.Leaves : null;
        }
        private List<Schedule> GetSchedules( Guid personId , DateTime from , DateTime to )
        {
            var response = TamamServiceBroker.SchedulesHandler.GetSchedules( from , to , new List<Guid> { personId } , new List<Guid> { } , SystemRequestContext.Instance );
            return response.Type == ResponseState.Success ? response.Result : null;
        }
        private List<ScheduleDay> GetSchedulesDays( Guid personId , DateTime from , DateTime to , List<Schedule> schedules )
        {
            if ( schedules == null ) return null;
            var scheduleDays = new List<ScheduleDay>();

            foreach ( var schedule in schedules )
            {
                var response = TamamServiceBroker.SchedulesHandler.GetScheduleDetails( schedule , from , to , null , new List<Guid> { personId } , SystemRequestContext.Instance );
                if ( response.Type == ResponseState.Success )
                {
                    scheduleDays.AddRange( response.Result );
                }
            }

            return scheduleDays;
        }
        private List<ScheduleDay> GetSchedulesDaysEffective( Guid personId , DateTime from , DateTime to , List<ScheduleDay> scheduleDays , bool includeOffDays , bool includeLeaves )
        {
            if ( scheduleDays == null ) return null;

            if ( !includeOffDays )
            {
                // exclude off days ...
                scheduleDays = scheduleDays.Where( x => x.IsDayOff == false ).ToList();

                // exclude holidays
                var holidaysDays = GetDays( GetHolidays( personId , from , to ) );
                scheduleDays = scheduleDays.Where( x => holidaysDays.Contains( x.Day ) == false ).ToList();
            }

            // exclude Leaves (active) ...
            if ( !includeLeaves )
            {
                var leaves = GetActiveLeaves( personId , from , to );
                if ( leaves != null && leaves.Count > 0 ) scheduleDays = null;
            }

            return scheduleDays;
        }

        private List<Policy> GetPolicies( Guid personId )
        {
            var response = TamamServiceBroker.OrganizationHandler.GetPolicies( personId , SystemRequestContext.Instance );
            return response.Type == ResponseState.Success ? response.Result : null;
        }
        //private ShiftPolicy GetShiftPolicy( List<Policy> policies , Shift shift )
        //{
        //    var policy = SystemBroker.OrganizationHandler.GetShiftPolicy( policies , shift ).Result;
        //    return new ShiftPolicy( policy );

        //    //return policies == null ? null : new ShiftPolicy( policies.FirstOrDefault( p => p.PolicyTypeId == Guid.Parse( PolicyTypes.ShiftPolicyType ) ) );
        //}

        private List<DateTime> GetDays( List<Holiday> holidays )
        {
            var list = new List<DateTime>();

            foreach ( var holiday in holidays )
            {
                for ( var d = holiday.StartDate ; d <= holiday.EndDate ; d = d.AddDays( 1 ) )
                {
                    list.Add( d );
                }
            }
            return list;
        }

        private string ListToString<T>( List<T> list )
        {
            if ( list != null ) return string.Join( "," , list.ToArray() );
            return string.Empty;
        }

        #endregion
    }
}