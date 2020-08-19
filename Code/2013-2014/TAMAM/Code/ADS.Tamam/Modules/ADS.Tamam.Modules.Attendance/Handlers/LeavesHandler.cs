using System;
using System.Collections.Generic;
using System.Linq;
using ADS.Common.Bases.Events.Handlers;
using ADS.Common.Context;
using ADS.Common.Handlers;
using ADS.Common.Utilities;
using ADS.Common.Validation;
using ADS.Common.Workflow.Enums;
using ADS.Common.Workflow.Models;
using ADS.Tamam.Common.Data;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Contracts;
using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Common.Data.Model.Domain.Policy;
using ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Data.Validation;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Handlers.Workflow.Leaves.Review.Models;
using ADS.Tamam.Common.Workflow.Leaves.Review.Data;
using ADS.Tamam.Common.Workflow.Leaves.Review.Definitions;
using ADS.Tamam.Modules.Attendance.Events;
using ADS.Tamam.Resources.Culture;
using LeavesConstants = ADS.Tamam.Resources.Culture.Leaves;
using ADS.Common;
using ADS.Common.Bases.Events.Models;
using ADS.Tamam.Common.Data.Model.DTO.Composite;
using ADS.Tamam.Common.Data.Model.DTO.Services;

namespace ADS.Tamam.Modules.Attendance.Handlers
{
    public class LeavesHandler : ILeavesHandler, ISystemLeavesHandler, IReadOnlyLeavesHandler
    {
        #region Properties

        public bool Initialized { get; private set; }
        public string Name { get { return "LeavesHandler"; } }

        private readonly LeavesDataHandler dataHandler;
        private readonly LeaveApprovalWorkflowDefinition approvalWorkflowDefinition = new LeaveApprovalWorkflowDefinition();

        #endregion
        #region cst.

        public LeavesHandler()
        {
            XLogger.Info( "LeavesHandler ... Initializing ..." );

            if ( !TamamDataBroker.Initialized )
            {
                XLogger.Error( "LeavesHandler ... Initialization Failed, underlying handlers are not registered or initilaized successfully." );
                return;
            }

            dataHandler = TamamDataBroker.GetRegisteredDataLayer<LeavesDataHandler>( TamamConstants.LeavesDataHandlerName );

            if ( dataHandler != null && dataHandler.Initialized )
            {
                XLogger.Info( "LeavesHandler ... Initialized" );
                Initialized = true;
            }
            else
            {
                XLogger.Error( "LeavesHandler ... Initialization Failed, underlying handlers are not registered or initilaized successfully." );
                Initialized = false;
            }
        }

        #endregion

        #region ILeavesHandler

        # region Leaves

        public ExecutionResponse<bool> CreateLeaves( List<Leave> leaves, RequestContext requestContext )
        {
            return this.CreateLeaves( leaves, false, requestContext );
        }
        public ExecutionResponse<bool> RequestLeave( Leave leave, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.RequestLeaveActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.RequestLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreatePersonAuditMessageFailure, leave == null ? string.Empty : leave.Id.ToString() ), leave == null ? string.Empty : leave.Id.ToString() );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region UpdateModel
                leave.IsNative = true;
                leave.Code = leave.Id.ToString();
                #endregion
                #region Validation

                var validator = new LeaveValidator( leave, LeaveValidator.ValidationMode.Create );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.RequestLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    return;
                }
                #endregion

                #region Leave Policy
                var person = GetPerson( leave.PersonId, requestContext );
                var policies = GetPolicies( person, requestContext );
                var leavePolicy = GetPoliciesLeaves( policies, ( LeaveTypes )leave.LeaveTypeId );
                if ( leavePolicy == null )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.RequestLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                    context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "LeaveTypeId", string.Format( ValidationResources.LeaveInvalidLeaveTypePolicy, person.GetLocalizedFullName() ) ) } );
                    return;
                }

                #endregion
                # region Check Allow Request & Days before Request

                // Not Allowed to request leaves
                if ( leavePolicy.AllowRequests.HasValue == false || leavePolicy.AllowRequests.Value == false )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.RequestLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                    context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "PersonId", ValidationResources.LeaveRequestNotAllowed ) } );
                    return;
                }

                // Check Days before Request
                // user can request vacations in the past 
                // or before the vacation date with DaysBeforeRequest policy value
                int diff = leave.StartDate.Subtract( DateTime.Today ).Days;
                if ( diff >= 0 ) // the requested leave is for today or future day
                {
                    //if the leave policy disable the planned leave so the system will return error
                    if ( leavePolicy.DisablePlannedLeaves.HasValue && leavePolicy.DisablePlannedLeaves.Value )
                    {
                        TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.RequestLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                        context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "PersonId", ValidationResources.PlannedLeavesNotAllow ) } );
                        return;
                    }
                    ///////////////////////////////////////////////////////////////////////
                    //0 will disable the validation
                    if ( leavePolicy.DaysBeforeRequest.HasValue && leavePolicy.DaysBeforeRequest.Value > diff )
                    {
                        TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.RequestLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );

                        var errorMessage = string.Format( ValidationResources.LeaveRequestDaysBefore, leavePolicy.DaysBeforeRequest.Value );
                        context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "PersonId", errorMessage ) } );
                        return;
                    }
                }

                // validate Days Limit for old dates..
                if ( leavePolicy.DaysLimitForOldLeaves.HasValue && leavePolicy.DaysLimitForOldLeaves.Value > 0 )
                {
                    var daysLimit = leavePolicy.DaysLimitForOldLeaves.Value;
                    var startDateLimit = DateTime.Today.AddDays( daysLimit * -1 );
                    if ( leave.StartDate < startDateLimit )
                    {
                        var errorMessage = string.Format( ValidationResources.LeaveStartDateExceedsOldLeavesDaysLimit, person.GetLocalizedFullName() );
                        context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "StartDate", errorMessage ) } );
                        return;
                    }
                }

                # endregion
                # region Check Allow Request For half day leaves / Require Attachments..

                // not allowed to request half days ?
                if ( leave.LeaveMode == LeaveMode.HalfDay && ( !leavePolicy.AllowHalfDays.HasValue || !leavePolicy.AllowHalfDays.Value ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreateLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                    context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "LeaveTypeId", string.Format( ValidationResources.LeaveHalfDayNotAllowed, person.GetLocalizedFullName() ) ) } );
                    return;
                }

                // check for Leave Attachments..
                if ( leavePolicy.RequireAttachments.HasValue && leavePolicy.RequireAttachments.Value )
                {
                    if ( leave.Attachments == null || leave.Attachments.Count == 0 )
                    {
                        TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreateLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                        var leaveTypes = Broker.DetailCodeHandler.GetDetailCodesByMasterCode( TamamConstants.MasterCodes.LeaveType );
                        var associatedType = leaveTypes.FirstOrDefault( x => x.Id == leave.LeaveTypeId );
                        var leaveTypeName = requestContext.CultureName.ToLower().Contains( "ar" ) ? associatedType.NameCultureVariant : associatedType.Name;
                        context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "LeaveTypeId", string.Format( ValidationResources.LeaveAttachmentRequired, leaveTypeName ) ) } );

                        return;
                    }
                }

                # endregion

                // validate leave credit
                // Set EffectiveDaysCount for leave according to leave policy value ( Including weekends and holidays)
                SetEffectiveDaysAccordingToLeavePolicy( leave, leavePolicy );
                if ( leave.EffectiveDaysCount == 0 )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.RequestLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                    context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "StartDate", string.Format( ValidationResources.LeaveEffectiveDaysEqualZero, person.GetLocalizedFullName() ) ) } );
                    return;
                }

                var maxDaysPerRequest = leavePolicy.MaxDaysPerRequest.HasValue ? leavePolicy.MaxDaysPerRequest.Value : 0;
                //0 value will disable the validation
                if ( maxDaysPerRequest > 0 && leave.EffectiveDaysCount > maxDaysPerRequest )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.RequestLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                    context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "StartDate", string.Format( ValidationResources.LeaveEffectiveDayNotVaild, maxDaysPerRequest ) ) } );
                    return;
                }
                var LeaveCredit = GetCreditForLeave( leave.PersonId, leave, requestContext );
                if ( LeaveCredit.Type == ResponseState.NotFound )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.RequestLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                    context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "PersonId", string.Format( ValidationResources.LeaveOutCredit, person.GetLocalizedFullName() ) ) } );
                    return;
                }
                var credit = LeaveCredit.Result;
                var unlimitedCredit = leavePolicy.UnlimitedCredit.HasValue ? leavePolicy.UnlimitedCredit.Value : false;
                if ( !unlimitedCredit )
                {
                    var personCreditResult = GetCreditAmount( credit, leave, leavePolicy, requestContext );
                    if ( personCreditResult.Type == ResponseState.NotFound )
                    {
                        TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.RequestLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                        context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "PersonId", string.Format( ValidationResources.LeaveOutCredit, person.GetLocalizedFullName() ) ) } );
                        return;
                    }

                    var personCredit = personCreditResult.Result;
                    if ( leave.EffectiveDaysCount > personCredit )
                    {
                        TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.RequestLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                        context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "PersonId", string.Format( ValidationResources.LeaveTypeHasNoCredit, person.GetLocalizedFullName() ) ) } );
                        return;
                    }
                }
                //Set Request Time
                leave.RequestTime = DateTime.Now;
                leave.EffectiveYearStart = credit.EffectiveYearStart;

                #region Data Layer

                var response = dataHandler.CreateLeaves( new List<Leave>() { leave } );
                if ( !response.Result )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.RequestLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                    context.Response.Set( response );
                    return;
                }

                #endregion

                var state = EventsBroker.Handle( new LeaveRequestEvent( leave ), EventMode.Sync );

                // audit ...
                TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.RequestLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                context.Response.Set( ResponseState.Success, true );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Leaves );

                #endregion

                #endregion
            }, requestContext );

            return context.Response;
        }

        public ExecutionResponse<bool> EditLeave( Leave leave, RequestContext requestContext )
        {
            return this.EditLeave( leave, false, requestContext );
        }
        public ExecutionResponse<Leave> GetLeave( Guid leaveId, RequestContext requestContext )
        {
            var context = new ExecutionContext<Leave>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "LeavesHandler_GetLeave" + leaveId + requestContext;
                if ( !requestContext.IgnoreCache )
                {
                    var cached = Broker.Cache.Get<Leave>( TamamCacheClusters.Leaves, cacheKey );
                    if ( cached != null )
                    {
                        context.Response.Set( ResponseState.Success, cached );
                        return;
                    }
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.LeaveAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.GetLeaveAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.GetLeaveActionKey,
                    messageForDenied: string.Format( TamamConstants.AuthorizationConstants.GetLeaveAuditMessageFailure, leaveId ),
                    messageForFailure: string.Format( TamamConstants.AuthorizationConstants.GetLeaveAuditMessageFailure, leaveId ),
                    messageForSuccess: string.Format( TamamConstants.AuthorizationConstants.GetLeaveAuditMessageSuccessful, leaveId )
                    );

                var response = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type, null );
                    return;
                }

                var securityContext = response.Result;

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.GetLeave( leaveId, securityContext );
                if ( dataHandlerResponse.Result == null )
                {
                    context.Response.Set( dataHandlerResponse );
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.GetLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.GetLeaveAuditMessageFailure, leaveId ), string.Empty );
                    return;
                }

                context.Response.Set( dataHandlerResponse );

                #region Cache

                if ( !requestContext.IgnoreCache )
                {
                    Broker.Cache.Add<Leave>( TamamCacheClusters.Leaves, cacheKey, dataHandlerResponse.Result );
                }

                #endregion

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<LeaveSearchResult> SearchLeaves( LeaveSearchCriteria criteria, bool activePersonsOnly, RequestContext requestContext )
        {
            var context = new ExecutionContext<LeaveSearchResult>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "LeavesHandler_SearchLeaves" + criteria + activePersonsOnly + requestContext;
                var cached = Broker.Cache.Get<LeaveSearchResult>( TamamCacheClusters.Leaves, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext(
                    moduleId: TamamConstants.AuthorizationConstants.LeaveAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.SearchLeavesAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.SearchLeavesActionKey,
                    messageForDenied: TamamConstants.AuthorizationConstants.SearchLeavesActionKey,
                    messageForFailure: TamamConstants.AuthorizationConstants.SearchLeavesAuditMessageFailure,
                    messageForSuccess: TamamConstants.AuthorizationConstants.SearchLeavesAuditMessageSuccessful
                    );

                var response = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type, null );
                    return;
                }

                var securityContext = response.Result;

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.SearchLeaves( criteria, activePersonsOnly, securityContext );
                if ( dataHandlerResponse.Result == null )
                {
                    context.Response.Set( dataHandlerResponse );
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.SearchLeavesAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, TamamConstants.AuthorizationConstants.SearchLeavesAuditMessageFailure, string.Empty );
                    return;
                }

                context.Response.Set( dataHandlerResponse );
                #region Cache

                Broker.Cache.Add<LeaveSearchResult>( TamamCacheClusters.Leaves, cacheKey, dataHandlerResponse.Result );

                #endregion
                #endregion
            }, requestContext );

            return context.Response;
        }


        public ExecutionResponse<List<LeaveDetails>> SearchDetailsLeaves( LeaveSearchCriteria criteria, bool activePersonsOnly, RequestContext requestContext )
        {
            var context = new ExecutionContext<List<LeaveDetails>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "LeavesHandler_SearchLeaves" + criteria + activePersonsOnly + requestContext;
                var cached = Broker.Cache.Get<List<LeaveDetails>>( TamamCacheClusters.Leaves, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext(
                    moduleId: TamamConstants.AuthorizationConstants.LeaveAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.SearchLeavesAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.SearchLeavesActionKey,
                    messageForDenied: TamamConstants.AuthorizationConstants.SearchLeavesActionKey,
                    messageForFailure: TamamConstants.AuthorizationConstants.SearchLeavesAuditMessageFailure,
                    messageForSuccess: TamamConstants.AuthorizationConstants.SearchLeavesAuditMessageSuccessful
                    );

                var response = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type, null );
                    return;
                }

                var securityContext = response.Result;

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.SearchLeaves( criteria, activePersonsOnly, securityContext );
                if ( dataHandlerResponse.Result == null )
                {
                    context.Response.Set( dataHandlerResponse.Type, null, dataHandlerResponse.MessageDetailed );
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.SearchLeavesAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, TamamConstants.AuthorizationConstants.SearchLeavesAuditMessageFailure, string.Empty );
                    return;
                }
                List<LeaveDetails> LeavesDetails = new List<LeaveDetails>();
                //1- Group Leaves by Person
                var leavesByPerson = dataHandlerResponse.Result.Leaves.GroupBy( x => x.PersonId );
                foreach ( var leaveGroupPerson in leavesByPerson )
                {
                    var personId = leaveGroupPerson.Key;
                    //2- Group Leaves by EffectiveYearStart
                    var leavesByCreditYear = leaveGroupPerson.GroupBy( x => x.EffectiveYearStart );
                    foreach ( var leaveGroupByCreditYear in leavesByCreditYear )
                    {
                        var effectiveYearStart = leaveGroupByCreditYear.Key != null ? leaveGroupByCreditYear.Key : new DateTime();
                        var personCredit = GetPersonLeaveCredit( personId, effectiveYearStart.Value, SystemSecurityContext.Instance );
                        var personPreCredit = dataHandler.GetLeavePreCredit( personId, effectiveYearStart.Value ).Result;
                        //3- Group Leaves by Leave Type
                        var leavesByType = leaveGroupByCreditYear.GroupBy( x => x.LeaveTypeId );
                        foreach ( var leaveGroupByType in leavesByType )
                        {
                            var leaveType = leaveGroupByType.Key;
                            var leaveCredit = personCredit.LeaveTypeCredits.FirstOrDefault( x => x.LeaveTypeId == ( int )leaveType );
                            var carryOverCredit = personCredit.LeaveTypeCarryOverCredits.FirstOrDefault( x => x.LeaveTypeId == ( int )leaveType );
                            double carryOverCreditAmount = 0;
                            if ( carryOverCredit != null )
                            {
                                carryOverCreditAmount = carryOverCredit.Credit;
                            }
                            //Get preCredit consumed value for this Leave type
                            var preCreditLeaveType = personPreCredit != null ? personPreCredit.LeaveTypePreCredits.FirstOrDefault( x => x.LeaveTypeId == ( int )leaveType ) : null;
                            var consumed = preCreditLeaveType != null ? preCreditLeaveType.Consumed : 0;
                            double CreditAmount = leaveCredit != null ? leaveCredit.Amount + carryOverCreditAmount : 0;
                            double balanceBefore, balanceAfter;
                            var leavesOrders = leaveGroupByType.OrderBy( x => x.StartDate );
                            if ( leavesOrders.Count() > 0 )
                            {
                                //Get all person leaves for this type before the first leave in search result that may consume from the credit balance 
                                var firstStartDate = leavesOrders.First().StartDate;
                                if ( effectiveYearStart != firstStartDate )
                                {
                                    var searchCriteria = new LeaveSearchCriteria( new List<Guid> { personId }, null, effectiveYearStart.Value, firstStartDate.AddDays( -1 ), null, new List<int> { leaveType },
                                        new List<int> { ( int )LeaveStatus.Approved, ( int )LeaveStatus.Pending, ( int )LeaveStatus.Planned, ( int )LeaveStatus.Taken }, false, 0, 0 );
                                    var LeavesBeforeStart = SearchLeaves( searchCriteria, true, requestContext ).Result;
                                    consumed += LeavesBeforeStart != null ? LeavesBeforeStart.Leaves.Sum( x => x.EffectiveDaysCount ) : 0;
                                }
                            }
                            CreditAmount -= consumed;
                            foreach ( var leave in leavesOrders )
                            {
                                balanceBefore = CreditAmount;
                                balanceAfter = ( leave.LeaveStatusId == ( int )LeaveStatus.Cancelled || leave.LeaveStatusId == ( int )LeaveStatus.Denied ) ?
                                               balanceBefore : CreditAmount - leave.EffectiveDaysCount;
                                CreditAmount = balanceAfter;
                                LeavesDetails.Add( new LeaveDetails() { Leave = leave, BalanceBefore = balanceBefore, BalanceAfter = balanceAfter } );

                            }
                        }
                    }
                }
                context.Response.Set( ResponseState.Success, LeavesDetails );
                #region Cache

                Broker.Cache.Add<List<LeaveDetails>>( TamamCacheClusters.Leaves, cacheKey, LeavesDetails );

                #endregion
                #endregion
            }, requestContext );

            return context.Response;
        }

        public ExecutionResponse<bool> CancelLeave( Guid leaveId, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.CancelLeaveActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CancelLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CancelLeaveAuditMessageFailure, leaveId ), leaveId.ToString() );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // models
                var leave = dataHandler.GetLeave( leaveId, SystemSecurityContext.Instance ).Result;

                #region Validation

                var validator = new LeaveValidator( leave, LeaveValidator.ValidationMode.Cancel );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CancelLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CancelLeaveAuditMessageFailure, leaveId ), leaveId.ToString() );
                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    return;
                }

                #endregion

                #region Advanced validation

                //if (leave.LeaveStatusId == (int)LeaveStatus.Taken
                //    && leave.PersonId == requestContext.PersonId)
                //{

                //    context.Response.Set(ResponseState.ValidationError, false, new List<ModelMetaPair>() { new ModelMetaPair(string.Empty, string.Format(ValidationResources.LeaveCanCancelled, leave.Person.GetLocalizedFullName())) });
                //    return;
                //}

                #endregion

                #region workflow ...

                // workflow : cancel ... ( + model )
                // Note: intentionally Cancel WF, not CancelAndDelete, because we still need this WF instance due to Checkpoints..
                var approvalData = new LeaveReviewWorkflowData( leaveId.ToString(), null, WorkflowLeaveReviewStatus.Cancelled.ToString(), string.Empty, WorkflowLeaveTargetType.Leave );
                if ( !Broker.WorkflowEngine.Cancel( leave, approvalWorkflowDefinition.Id, approvalData ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CancelLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CancelLeaveAuditMessageFailure, leaveId ), leaveId.ToString() );
                    context.Response.Set( ResponseState.SystemError, false );
                    return;
                }

                #endregion

                // audit ...
                TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CancelLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CancelLeaveAuditMessageSuccessful, leaveId ), leaveId.ToString() );

                context.Response.Set( ResponseState.Success, true );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Leaves );

                #endregion

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> ReviewLeave( LeaveReview review, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.ReviewLeaveActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.ReviewLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.ReviewLeaveAuditMessageFailure, review == null ? string.Empty : review.LeaveId.ToString() ), review == null ? string.Empty : review.LeaveId.ToString() );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // model
                var leave = dataHandler.GetLeave( review.LeaveId, SystemSecurityContext.Instance ).Result;

                #region Validation

                var validator = new ReviewLeaveValidator( leave );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.ReviewLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.ReviewLeaveAuditMessageFailure, review.LeaveId ), review.LeaveId.ToString() );
                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    return;
                }

                #endregion
                #region workflow ...

                //var result = TamamServiceBroker.OrganizationHandler.UpdateRequestStatus(review.LeaveId , review.Status == ReviewLeaveStatus.Approved ? WorkFlowStepStatus.Approved : WorkFlowStepStatus.Denied, review.Comment , requestContext);

                var command = review.Status == ReviewLeaveStatus.Approved ? WorkflowLeaveReviewStatus.Approved : WorkflowLeaveReviewStatus.Denied;
                var approvalData = new LeaveReviewWorkflowData( review.LeaveId.ToString(), review.ReviewerId.ToString(), command.ToString(), review.Comment, WorkflowLeaveTargetType.Leave );
                if ( !Broker.WorkflowEngine.Invoke( leave, approvalWorkflowDefinition.Id, approvalData ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.ReviewLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.ReviewLeaveAuditMessageFailure, review.LeaveId ), review.LeaveId.ToString() );
                    context.Response.Set( ResponseState.SystemError, false );
                    return;
                }

                #endregion

                // audit trail ...
                TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.ReviewLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.ReviewLeaveAuditMessageSuccessful, review.LeaveId ), review.LeaveId.ToString() );

                context.Response.Set( ResponseState.Success, true );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Leaves );

                #endregion

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<LeaveAttachment> GetLeaveAttachment( Guid attachmentId, RequestContext requestContext )
        {
            var context = new ExecutionContext<LeaveAttachment>();
            context.Execute( () =>
            {
                #region logic ...
                #region Cache

                var cacheKey = "LeavesHandler_GetLeaveAttachment" + attachmentId + requestContext;
                var cached = Broker.Cache.Get<LeaveAttachment>( TamamCacheClusters.Leaves, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.LeaveAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.GetLeaveAttachmentAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.GetLeaveAttachmentActionKey,
                    messageForDenied: string.Format( TamamConstants.AuthorizationConstants.GetLeaveAttachmentAuditMessageFailure, attachmentId ),
                    messageForFailure: string.Format( TamamConstants.AuthorizationConstants.GetLeaveAttachmentAuditMessageFailure, attachmentId ),
                    messageForSuccess: string.Format( TamamConstants.AuthorizationConstants.GetLeaveAttachmentAuditMessageSuccessful, attachmentId )
                    );

                var response = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type, null );
                    return;
                }

                var securityContext = response.Result;

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.GetLeaveAttachment( attachmentId, securityContext );
                if ( dataHandlerResponse.Result == null )
                {
                    context.Response.Set( dataHandlerResponse );
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.GetLeaveAttachmentAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.GetLeaveAttachmentAuditMessageFailure, attachmentId ), string.Empty );
                    return;
                }

                // audit trail ...
                //TamamServiceBroker.Audit(requestContext, TamamConstants.AuthorizationConstants.GetLeaveAttachmentAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format(TamamConstants.AuthorizationConstants.GetLeaveAttachmentAuditMessageSuccessful, attachmentId), string.Empty);

                context.Response.Set( dataHandlerResponse );

                #region Cache

                Broker.Cache.Add<LeaveAttachment>( TamamCacheClusters.Leaves, cacheKey, dataHandlerResponse.Result );

                #endregion
                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> ChangeLeaveStatus( Guid leaveId, LeaveStatus status )
        {
            var response = dataHandler.ChangeLeaveStatus( leaveId, status );

            #region Cache

            Broker.Cache.Invalidate( TamamCacheClusters.Leaves );

            #endregion

            return response;
        }

        # endregion
        # region Pre-Credits
        
        public ExecutionResponse<LeavePreCredit> GetPersonPreCredits( Guid personId, RequestContext requestContext )
        {
            var context = new ExecutionContext<LeavePreCredit>();
            context.Execute( () =>
            {
                # region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError, null, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.EditPersonPreCreditActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditPersonPreCreditAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.EditPersonPreCreditAuditMessageFailure, personId ), string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, null, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                #region Cache

                var cacheKey = "LeavesHandler_GetPersonPreCredits" + personId;
                var cached = Broker.Cache.Get<LeavePreCredit>( TamamCacheClusters.Leaves, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion

                LeavePreCredit PreCredits = null;

                // models ...
                var person = GetPerson( personId, SystemRequestContext.Instance );
                var policies = GetPolicies( person, SystemRequestContext.Instance );
                var accrualPolicy = GetPoliciesAccrual( policies );
                var accrualPolicyStartDate = accrualPolicy.GetAccrualPolicyStartDate( person.AccountInfo.JoinDate.Value );

                var response = dataHandler.GetLeavePreCredit( personId, accrualPolicyStartDate );

                if ( response.Type == ResponseState.Success )
                {
                    PreCredits = response.Result;
                    context.Response.Set( response );
                }
                else
                {
                    context.Response.Set( response );
                    return;
                }

                #region Cache

                Broker.Cache.Add<LeavePreCredit>( TamamCacheClusters.Leaves, cacheKey, PreCredits );

                #endregion

                return;

                # endregion

            }, requestContext );
            return context.Response;
        }
        public ExecutionResponse<bool> BuildPreCredits( List<LeavePreCredit> preCredits, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                # region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError, false, new List<ModelMetaPair>() );
                    return false;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.TransferLeaveCreditActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.TransferLeaveCreditAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, TamamConstants.AuthorizationConstants.TransferLeaveCreditAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return false;
                }

                #endregion

                foreach ( var preCredit in preCredits )
                {
                    var effectiveDate = GetPersonAccrualPolicyEffectiveDate( preCredit.PersonId );
                    preCredit.EffectiveYearStart = effectiveDate;
                    var response = dataHandler.GetLeavePreCredit( preCredit.PersonId, effectiveDate );

                    var isOK = false;

                    switch ( response.Type )
                    {
                        case ResponseState.Success:

                        // Mark As Deleted (old one)
                        var success = MarkLeavePreCreditAsDeleted( response.Result );
                        if ( success == false )
                        {
                            context.Response.Set( ResponseState.Failure, false );
                            break;
                        }

                        // Add new one
                        success = AddLeavePreCredit( preCredit );
                        if ( success == false )
                        {
                            context.Response.Set( ResponseState.Failure, false );
                            break;
                        }

                        isOK = true;
                        context.Response.Set( ResponseState.Success, true );

                        break;
                        case ResponseState.NotFound:

                        // Add new one
                        success = AddLeavePreCredit( preCredit );
                        if ( success == false )
                        {
                            context.Response.Set( ResponseState.Failure, false );
                            break;
                        }

                        isOK = true;
                        context.Response.Set( ResponseState.Success, true );
                        break;
                        default:
                        context.Response.Set( ResponseState.Failure, false );
                        break;
                    }

                    if ( isOK == false ) break;
                }

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Leaves );

                #endregion

                return true;
                # endregion

            }, requestContext );
            return context.Response;
        }
        public ExecutionResponse<bool> ResetPreCredits( RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                # region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError, false, new List<ModelMetaPair>() );
                    return false;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.TransferLeaveCreditActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.TransferLeaveCreditAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, TamamConstants.AuthorizationConstants.TransferLeaveCreditAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return false;
                }

                #endregion

                // Mark All Pre-Credits as Deleted..
                var preCreditsResponse = dataHandler.DeletePreCredits();
                if ( preCreditsResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( preCreditsResponse.Type, false, new List<ModelMetaPair>() );
                    return false;
                }

                context.Response.Set( ResponseState.Success, true, new List<ModelMetaPair>() );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Leaves );

                #endregion

                return true;

                #endregion

            }, requestContext );
            return context.Response;
        }

        # region Helpers ...

        private DateTime GetPersonAccrualPolicyEffectiveDate( Guid personId )
        {
            var person = GetPerson( personId, SystemRequestContext.Instance );
            var policies = GetPolicies( person, SystemRequestContext.Instance );
            var accrualPolicy = GetPoliciesAccrual( policies );
            return accrualPolicy.GetAccrualPolicyStartDate( person.AccountInfo.JoinDate.Value );
        }
        private bool MarkLeavePreCreditAsDeleted( LeavePreCredit preCredit )
        {
            preCredit.IsDeleted = true;
            var response = dataHandler.UpdatePreCredit( preCredit );

            return response.Type == ResponseState.Success;
        }
        private bool AddLeavePreCredit( LeavePreCredit preCredit )
        {
            var response = dataHandler.AddLeavePreCredit( preCredit );
            return response.Type == ResponseState.Success;
        }

        # endregion

        # endregion
        # region Leave Credits
        
        public ExecutionResponse<List<DetailedLeaveCreditDTO>> GetDetailedLeaveCredit( List<Guid> departments, List<Guid> personnel, List<int> leaveTypes, RequestContext requestContext )
        {
            var context = new ExecutionContext<List<DetailedLeaveCreditDTO>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "LeavesHandler_GetDetailedLeaveCredit" + ListToString( departments ) + ListToString( personnel ) + ListToString( leaveTypes ) + requestContext;
                var cached = Broker.Cache.Get<List<DetailedLeaveCreditDTO>>( TamamCacheClusters.Leaves, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                                            (
                                            moduleId: TamamConstants.AuthorizationConstants.LeaveAuditModuleId,
                                            actionId: TamamConstants.AuthorizationConstants.GetLeaveAuditActionId,
                                            actionKey: TamamConstants.AuthorizationConstants.GetLeaveActionKey,
                                            messageForDenied: TamamConstants.AuthorizationConstants.GetLeaveAuditMessageFailure,
                                            messageForFailure: TamamConstants.AuthorizationConstants.GetLeaveAuditMessageFailure,
                                            messageForSuccess: TamamConstants.AuthorizationConstants.GetLeaveAuditMessageSuccessful
                                            );

                var response = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type, null );
                    return;
                }

                var securityContext = response.Result;

                #endregion

                #region Filter Personnel

                var P = new List<Person>();

                if ( personnel == null || personnel.Count == 0 )
                {
                    var personnelResponse = TamamServiceBroker.PersonnelHandler.GetPersonnel( new PersonSearchCriteria(), requestContext );
                    if ( personnelResponse.Type == ResponseState.Success ) P = personnelResponse.Result.Persons;
                    else
                    {
                        context.Response.Set( personnelResponse.Type, null );
                        return;
                    }
                }
                else
                {
                    var personnelResponse = TamamServiceBroker.PersonnelHandler.GetPersonnel( new PersonSearchCriteria() { Ids = personnel }, requestContext );
                    if ( personnelResponse.Type == ResponseState.Success ) P = personnelResponse.Result.Persons;
                    else
                    {
                        context.Response.Set( personnelResponse.Type, null );
                        return;
                    }
                }

                // Apply Departments filters..
                if ( departments != null && departments.Count > 0 ) 
                {
                    P = P.Where( x => departments.Contains( x.AccountInfo.DepartmentId ) ).ToList();
                }

                var personnelIds = P.Select( x => x.Id ).ToList();

                #endregion


                List<DetailedLeaveCreditDTO> DTOs = new List<DetailedLeaveCreditDTO>();

                var LeaveCredits = dataHandler.GetLeaveCredits( personnelIds, securityContext ).Result;
                var PreCredits = dataHandler.GetLeavePreCredits( personnelIds ).Result;

                foreach ( LeaveCredit Credit in LeaveCredits )
                {
                    var LeavesPreCredit = PreCredits.FirstOrDefault( x => x.PersonId == Credit.PersonId );

                    var DTO = new DetailedLeaveCreditDTO( Credit, LeavesPreCredit, leaveTypes );
                    DTOs.Add( DTO );
                }

                context.Response.Set( ResponseState.Success, DTOs );

                #region Cache

                Broker.Cache.Add<List<DetailedLeaveCreditDTO>>( TamamCacheClusters.Leaves, cacheKey, DTOs );

                #endregion

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<DetailedLeaveCreditDTO> GetDetailedLeaveCredit( Guid personId, RequestContext requestContext )
        {
            var context = new ExecutionContext<DetailedLeaveCreditDTO>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "LeavesHandler_GetDetailedLeaveCredit" + personId + requestContext;
                var cached = Broker.Cache.Get<DetailedLeaveCreditDTO>( TamamCacheClusters.Leaves, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                                            (
                                            moduleId: TamamConstants.AuthorizationConstants.LeaveAuditModuleId,
                                            actionId: TamamConstants.AuthorizationConstants.GetLeaveAuditActionId,
                                            actionKey: TamamConstants.AuthorizationConstants.GetLeaveActionKey,
                                            messageForDenied: TamamConstants.AuthorizationConstants.GetLeaveAuditMessageFailure,
                                            messageForFailure: TamamConstants.AuthorizationConstants.GetLeaveAuditMessageFailure,
                                            messageForSuccess: TamamConstants.AuthorizationConstants.GetLeaveAuditMessageSuccessful
                                            );

                var response = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type, null );
                    return;
                }

                var securityContext = response.Result;

                #endregion

                var nativeLeavePolicies = TamamServiceBroker.OrganizationHandler.GetPolicies( personId, new PolicyFilters( Guid.Parse( PolicyTypes.LeavePolicyType ) ), SystemRequestContext.Instance ).Result;
                var LeavePolicies = nativeLeavePolicies != null ? nativeLeavePolicies.Select( p => new LeavePolicy( p ) ).ToList() : new List<LeavePolicy>();

                var LeavesCredit = GetLeaveCredit( personId, requestContext ).Result;
                var LeavesPreCredit = GetPersonPreCredits( personId, SystemRequestContext.Instance ).Result;

                var DTO = new DetailedLeaveCreditDTO( LeavePolicies, LeavesCredit, LeavesPreCredit );

                context.Response.Set( ResponseState.Success, DTO );

                #region Cache

                Broker.Cache.Add<DetailedLeaveCreditDTO>( TamamCacheClusters.Leaves, cacheKey, DTO );

                #endregion

                #endregion
            }, requestContext );

            return context.Response;
        }

        public ExecutionResponse<LeaveCredit> GetLeaveCredit( Guid personId, RequestContext requestContext )
        {
            var context = new ExecutionContext<LeaveCredit>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "LeavesHandler_GetLeaveCredit" + personId + requestContext;
                var cached = Broker.Cache.Get<LeaveCredit>( TamamCacheClusters.Leaves, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                                            (
                                            moduleId: TamamConstants.AuthorizationConstants.LeaveAuditModuleId,
                                            actionId: TamamConstants.AuthorizationConstants.GetLeaveAuditActionId,
                                            actionKey: TamamConstants.AuthorizationConstants.GetLeaveActionKey,
                                            messageForDenied: TamamConstants.AuthorizationConstants.GetLeaveAuditMessageFailure,
                                            messageForFailure: TamamConstants.AuthorizationConstants.GetLeaveAuditMessageFailure,
                                            messageForSuccess: TamamConstants.AuthorizationConstants.GetLeaveAuditMessageSuccessful
                                            );

                var response = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type, null );
                    return;
                }

                var securityContext = response.Result;

                #endregion

                // models ...
                var person = GetPerson( personId, requestContext );
                var policies = GetPolicies( person, requestContext );
                var accrualPolicy = GetPoliciesAccrual( policies );

                var credit = GetPersonLeaveCredit( person.Id, accrualPolicy.GetAccrualPolicyStartDate( person.AccountInfo.JoinDate.Value ), securityContext );

                context.Response.Set( ResponseState.Success, credit );

                #region Cache

                Broker.Cache.Add<LeaveCredit>( TamamCacheClusters.Leaves, cacheKey, credit );

                #endregion

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<LeaveCredit> GetLeaveCreditForLeaveDates( Guid personId, DateTime leaveStart, DateTime leaveEnd, RequestContext requestContext )
        {
            var context = new ExecutionContext<LeaveCredit>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "LeavesHandler_GetLeaveCreditForLeaveDates" + personId + leaveStart.ToShortDateString() + leaveEnd.ToShortDateString() + requestContext;
                var cached = Broker.Cache.Get<LeaveCredit>( TamamCacheClusters.Leaves, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                                            (
                                            moduleId: TamamConstants.AuthorizationConstants.LeaveAuditModuleId,
                                            actionId: TamamConstants.AuthorizationConstants.GetLeaveAuditActionId,
                                            actionKey: TamamConstants.AuthorizationConstants.GetLeaveActionKey,
                                            messageForDenied: TamamConstants.AuthorizationConstants.GetLeaveAuditMessageFailure,
                                            messageForFailure: TamamConstants.AuthorizationConstants.GetLeaveAuditMessageFailure,
                                            messageForSuccess: TamamConstants.AuthorizationConstants.GetLeaveAuditMessageSuccessful
                                            );

                var response = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type, null );
                    return;
                }

                var securityContext = response.Result;

                #endregion

                // models ...
                var person = GetPerson( personId, requestContext );
                var policies = GetPolicies( person, requestContext );
                var accrualPolicy = GetPoliciesAccrual( policies );
                var effectiveDate = accrualPolicy.GetAccrualPolicyStartDate( person.AccountInfo.JoinDate.Value );
                var effectiveDateNextCredit = effectiveDate.AddYears( 1 );
                var effectiveDatePreviousCredit = effectiveDate.AddYears( -1 );
                //Search for the suitable credit for this leave date range
                LeaveCredit credit = null;
                //Current year Credit
                if ( leaveStart >= effectiveDate && leaveEnd <= effectiveDate.AddYears( 1 ).AddDays( -1 ) )
                {
                    credit = GetPersonLeaveCredit( person.Id, effectiveDate, SystemSecurityContext.Instance );
                }
                //Next year Credit
                else if ( leaveStart >= effectiveDateNextCredit && leaveEnd <= effectiveDateNextCredit.AddYears( 1 ).AddDays( -1 ) )
                {
                    credit = GetPersonLeaveCredit( person.Id, effectiveDateNextCredit, SystemSecurityContext.Instance );
                }
                //Previous year Credit
                else if ( leaveStart >= effectiveDatePreviousCredit && leaveEnd <= effectiveDatePreviousCredit.AddYears( 1 ).AddDays( -1 ) )
                {
                    credit = GetPersonLeaveCredit( person.Id, effectiveDatePreviousCredit, SystemSecurityContext.Instance );
                }
                else
                {
                    context.Response.Set( ResponseState.NotFound, null );
                    return;
                }

                context.Response.Set( ResponseState.Success, credit );
                #region Cache

                Broker.Cache.Add<LeaveCredit>( TamamCacheClusters.Leaves, cacheKey, credit );

                #endregion
                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<LeaveCredit> GetCurrentLeaveCredit( Guid personId, RequestContext requestContext )
        {
            var context = new ExecutionContext<LeaveCredit>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "LeavesHandler_GetCurrentLeaveCredit" + personId + requestContext;
                var cached = Broker.Cache.Get<LeaveCredit>( TamamCacheClusters.Leaves, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                                            (
                                            moduleId: TamamConstants.AuthorizationConstants.LeaveAuditModuleId,
                                            actionId: TamamConstants.AuthorizationConstants.GetLeaveAuditActionId,
                                            actionKey: TamamConstants.AuthorizationConstants.GetLeaveActionKey,
                                            messageForDenied: TamamConstants.AuthorizationConstants.GetLeaveAuditMessageFailure,
                                            messageForFailure: TamamConstants.AuthorizationConstants.GetLeaveAuditMessageFailure,
                                            messageForSuccess: TamamConstants.AuthorizationConstants.GetLeaveAuditMessageSuccessful
                                            );

                var response = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type, null );
                    return;
                }

                var securityContext = response.Result;

                #endregion

                // models ...
                var person = GetPerson( personId, requestContext );
                var policies = GetPolicies( person, requestContext );
                var accrualPolicy = GetPoliciesAccrual( policies );

                var credit = GetPersonLeaveCredit( person.Id, accrualPolicy.GetAccrualPolicyStartDate( person.AccountInfo.JoinDate.Value ), ( int )LeaveCreditStatus.Current, securityContext );

                context.Response.Set( ResponseState.Success, credit );
                #region Cache

                Broker.Cache.Add<LeaveCredit>( TamamCacheClusters.Leaves, cacheKey, credit );

                #endregion
                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<LeaveCredit> GetNextLeaveCredit( Guid personId, RequestContext requestContext )
        {
            var context = new ExecutionContext<LeaveCredit>();
            context.Execute( () =>
            {
                #region logic ...
                #region Cache

                var cacheKey = "LeavesHandler_GetNextLeaveCredit" + personId + requestContext;
                var cached = Broker.Cache.Get<LeaveCredit>( TamamCacheClusters.Leaves, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                                            (
                                            moduleId: TamamConstants.AuthorizationConstants.LeaveAuditModuleId,
                                            actionId: TamamConstants.AuthorizationConstants.GetLeaveAuditActionId,
                                            actionKey: TamamConstants.AuthorizationConstants.GetLeaveActionKey,
                                            messageForDenied: TamamConstants.AuthorizationConstants.GetLeaveAuditMessageFailure,
                                            messageForFailure: TamamConstants.AuthorizationConstants.GetLeaveAuditMessageFailure,
                                            messageForSuccess: TamamConstants.AuthorizationConstants.GetLeaveAuditMessageSuccessful
                                            );

                var response = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type, null );
                    return;
                }

                var securityContext = response.Result;

                #endregion

                // models ...
                var person = GetPerson( personId, requestContext );
                var policies = GetPolicies( person, requestContext );
                var accrualPolicy = GetPoliciesAccrual( policies );
                var effectiveDate = accrualPolicy.GetAccrualPolicyStartDate( person.AccountInfo.JoinDate.Value ).AddYears( 1 );
                var credit = GetPersonLeaveCredit( person.Id, effectiveDate, ( int )LeaveCreditStatus.Next, securityContext );

                context.Response.Set( ResponseState.Success, credit );
                #region Cache

                Broker.Cache.Add<LeaveCredit>( TamamCacheClusters.Leaves, cacheKey, credit );

                #endregion
                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<double> CheckLeaveTypeCredit( Guid personId, LeaveTypes leaveType, Leave leave, RequestContext requestContext )
        {
            var context = new ExecutionContext<double>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError, 0, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.CheckLeaveCreditActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CheckLeaveCreditAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CheckLeaveCreditAuditMessageFailure, personId ), string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, 0, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // models ...
                var LeaveCreditResult = GetLeaveCreditForLeaveDates( personId, leave.StartDate, leave.EndDate, requestContext );
                if ( LeaveCreditResult.Type == ResponseState.Success && LeaveCreditResult.Result == null )
                {
                    var response = RecalculateLeaveCredit( personId, requestContext );
                    if ( response.Type == ResponseState.Success )
                    {
                        LeaveCreditResult = GetLeaveCreditForLeaveDates( personId, leave.StartDate, leave.EndDate, requestContext );
                        if ( LeaveCreditResult.Type == ResponseState.NotFound )
                        {
                            context.Response.Set( ResponseState.NotFound, 0 );
                            return;
                        }
                    }
                    else
                    {
                        context.Response.Set( ResponseState.SystemError, 0 );
                        return;
                    }
                }
                else if ( LeaveCreditResult.Type == ResponseState.NotFound )
                {
                    context.Response.Set( ResponseState.NotFound, 0 );
                    return;
                }
                var credit = LeaveCreditResult.Result;
                //MG,18-2-2015
                var creditStart = credit.EffectiveYearStart;
                var creditEnd = creditStart.AddYears( 1 ).AddDays( -1 );

                if ( leave.StartDate >= creditStart && leave.EndDate <= creditEnd )
                {
                    // credit ...
                    var leaveCredit = credit.LeaveTypeCredits.FirstOrDefault( x => x.LeaveTypeId == ( int )leaveType );
                    var carryOverCredit = credit.LeaveTypeCarryOverCredits.FirstOrDefault( x => x.LeaveTypeId == ( int )leaveType );
                    double carryOverCreditAmount = 0;
                    if ( carryOverCredit != null )
                    {
                        carryOverCreditAmount = carryOverCredit.Credit;
                    }
                    double amount = leaveCredit != null ? leaveCredit.Amount + carryOverCreditAmount - leaveCredit.Consumed : 0;

                    context.Response.Set( ResponseState.Success, amount );
                }
                else
                {
                    context.Response.Set( ResponseState.NotFound, 0 );
                }
                #endregion
            }, requestContext );

            return context.Response;
        }

        public ExecutionResponse<bool> UpdateLeaveCredit( Leave updatedLeave, LeaveStatus originalLeaveStatus, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.UpdateLeaveCreditActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.UpdateLeaveCreditAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.UpdateLeaveCreditAuditMessageFailure, updatedLeave.Id ), string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                double effectiveDaysCount = updatedLeave.EffectiveDaysCount;

                if ( ( originalLeaveStatus == LeaveStatus.Planned || originalLeaveStatus == LeaveStatus.Pending || originalLeaveStatus == LeaveStatus.Denied ) && ( updatedLeave.LeaveStatusId == ( int )LeaveStatus.Approved ) )
                {
                    // + (Incremented in Consumed)
                    //effectiveDaysCount *= 1;
                    effectiveDaysCount *= 0;
                }
                else if ( ( originalLeaveStatus == LeaveStatus.Planned || originalLeaveStatus == LeaveStatus.Pending || originalLeaveStatus == LeaveStatus.Approved || originalLeaveStatus == LeaveStatus.Taken ) && ( updatedLeave.LeaveStatusId == ( int )LeaveStatus.Denied || updatedLeave.LeaveStatusId == ( int )LeaveStatus.Cancelled ) )
                {
                    // - (Decremented from Consumed)
                    effectiveDaysCount *= -1;
                }
                else
                {
                    context.Response.Set( ResponseState.Success, true, new List<ModelMetaPair>() );
                    return;
                }

                // ...
                var response = UpdateLeaveCredit( updatedLeave.PersonId, updatedLeave, effectiveDaysCount, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response );
                    return;
                }

                // audit ...
                TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.UpdateLeaveCreditAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.UpdateLeaveCreditAuditMessageSuccessful, updatedLeave.Id ), string.Empty );

                context.Response.Set( ResponseState.Success, true );
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Leaves );

                #endregion
                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> RecalculateLeaveCredit( Guid personId, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError, false, new List<ModelMetaPair>() );
                    return false;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.RecalculateLeaveCreditActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.RecalculateLeaveCreditAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.UpdateLeaveCreditAuditMessageFailure, personId ), string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return false;
                }

                #endregion
                // models ...
                var person = GetPerson( personId, requestContext );
                var policies = GetPolicies( person, requestContext );
                var accrualPolicy = GetPoliciesAccrual( policies );

                var yearStart = accrualPolicy.GetAccrualPolicyStartDate( person.AccountInfo.JoinDate.Value );
                /*the end date for the credit is the last day in the same year 
                               *if the credit started at 1-1-2015 so the end must be 31-12-2015 not 1-1-2016 
                               *as 1-1-2016 will be calculated in the next credit*/
                var yearEnd = yearStart.AddYears( 1 ).AddDays( -1 );

                var leaves = GetLeaves( personId, yearStart, yearEnd, requestContext );

                var personCredits = new List<LeaveCredit>();
                var status = false;
                // mark the Credit with effectiveCreditYearStart = yearStart - 1  and has status current as Previous Credit
                //Needed when transfer credit
                var previousCreditYearStart = yearStart.AddYears( -1 );
                var previousCredit = GetPersonLeaveCredit( person.Id, previousCreditYearStart, ( int )LeaveCreditStatus.Current, SystemSecurityContext.Instance );
                if ( previousCredit != null )
                {
                    previousCredit.LeaveCreditStatusId = ( int )LeaveCreditStatus.Previous;
                    var responseupdate = dataHandler.UpdateLeaveCredit( previousCredit );
                    if ( responseupdate.Type != ResponseState.Success )
                    {
                        context.Response.Set( responseupdate );
                        return false;
                    }
                }
                else
                {
                    previousCredit = GetPersonLeaveCredit( person.Id, previousCreditYearStart, ( int )LeaveCreditStatus.Previous, SystemSecurityContext.Instance );
                    bool forceBuildPreviousCredit;
                    if ( !bool.TryParse( Broker.ConfigurationHandler.GetValue( Constants.TamamLeaveCreditConfig.SectionTamam_LeaveCredits,
                        Constants.TamamLeaveCreditConfig.ForceBuildPreviousCredit ), out forceBuildPreviousCredit ) ) forceBuildPreviousCredit = false;

                    if ( previousCredit != null || ( previousCredit == null && forceBuildPreviousCredit ) )
                    {
                        status = ResetPersonLeaveCredit( person, previousCreditYearStart, SystemSecurityContext.Instance );
                        if ( !status )
                        {
                            context.Response.Set( ResponseState.Failure, false );
                            return false;
                        }
                        if ( !CalculatePreviousCredit( personId, requestContext, context, person, policies, yearStart ) )
                        {
                            context.Response.Set( ResponseState.Failure, false );
                            return false;
                        }
                    }

                }
                ///////////////////////////////////////////////////////////////
                // reset credit
                status = ResetPersonLeaveCredit( person, yearStart, SystemSecurityContext.Instance );
                if ( !status )
                {
                    context.Response.Set( ResponseState.Failure, false, new List<ModelMetaPair>() );
                    return false;
                }
                var credit = RebuildLeaveCredit( person, policies );

                // update leaves
                leaves = UpdateLeavesEffectiveDays( leaves, credit.EffectiveYearStart, requestContext );

                // filter leaves (Planned && Pending && Approved & Taken)
                leaves = leaves.Where( x => x.LeaveStatusId == ( int )LeaveStatus.Planned || x.LeaveStatusId == ( int )LeaveStatus.Pending || x.LeaveStatusId == ( int )LeaveStatus.Approved || x.LeaveStatusId == ( int )LeaveStatus.Taken ).ToList();

                // update credit
                foreach ( var typeCredit in credit.LeaveTypeCredits )
                {
                    //typeCredit.Consumed = leaves.Where( x => x.LeaveTypeId == typeCredit.LeaveTypeId ).Sum( y => y.EffectiveDaysCount );
                    typeCredit.Consumed += leaves.Where( x => x.LeaveTypeId == typeCredit.LeaveTypeId ).Sum( y => y.EffectiveDaysCount );
                }
                personCredits.Add( credit );

                #region Handle Next year credit

                var nextYearStart = yearStart.AddYears( 1 );
                var nextYearEnd = nextYearStart.AddYears( 1 ).AddDays( -1 );

                var leavesInNextYear = GetLeaves( personId, nextYearStart, nextYearEnd, requestContext );

                // reset next year credit
                status = ResetPersonLeaveCredit( person, nextYearStart, SystemSecurityContext.Instance );
                if ( !status )
                {
                    context.Response.Set( ResponseState.Failure, false, new List<ModelMetaPair>() );
                    return false;
                }

                var nextCredit = RebuildNextYearLeaveCredit( person, policies );

                // update leaves
                leavesInNextYear = UpdateLeavesEffectiveDays( leavesInNextYear, nextCredit.EffectiveYearStart, requestContext );

                // filter leaves (Planned && Pending && Approved & Taken)
                leavesInNextYear = leavesInNextYear.Where( x => x.LeaveStatusId == ( int )LeaveStatus.Planned || x.LeaveStatusId == ( int )LeaveStatus.Pending
                    || x.LeaveStatusId == ( int )LeaveStatus.Approved || x.LeaveStatusId == ( int )LeaveStatus.Taken ).ToList();

                // update credit
                foreach ( var typeCredit in nextCredit.LeaveTypeCredits )
                {
                    typeCredit.Consumed += leavesInNextYear.Where( x => x.LeaveTypeId == typeCredit.LeaveTypeId ).Sum( y => y.EffectiveDaysCount );
                }

                personCredits.Add( nextCredit );
                #endregion

                var response_update = dataHandler.UpdateLeaveCredit( personCredits );
                if ( response_update.Type != ResponseState.Success )
                {
                    context.Response.Set( response_update );
                    return false;
                }

                // audit ...
                TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.RecalculateLeaveCreditAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.RecalculateLeaveCreditAuditMessageSuccessful, personId ), string.Empty );

                context.Response.Set( ResponseState.Success, true );
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Leaves );

                #endregion
                return true;

                #endregion
            }, requestContext );

            return context.Response;
        }

        public ExecutionResponse<bool> TransferCredits( RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError, false, new List<ModelMetaPair>() );
                    return false;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.TransferLeaveCreditActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.TransferLeaveCreditAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, TamamConstants.AuthorizationConstants.TransferLeaveCreditAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return false;
                }

                #endregion

                // models ...
                var peopleResponse = TamamServiceBroker.PersonnelHandler.GetPersonnelWithUnTransferredCredits();
                if ( peopleResponse.Type != ResponseState.Success )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.TransferLeaveCreditAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, TamamConstants.AuthorizationConstants.TransferLeaveCreditAuditMessageFailure, string.Empty );
                    context.Response.Set( ResponseState.Failure, false,
                        new List<ModelMetaPair> { new ModelMetaPair( string.Empty, LeavesConstants.GetPeopleFaild ) } );
                    return false;
                }

                var unTransferredCreditsPeople = peopleResponse.Result;

                foreach ( var id in unTransferredCreditsPeople )
                {
                    //We need to check if there is any pending leaves in the credit currently running -the one that will be mark as pervious-
                    //the transfer credit will automatic approve this leaves

                    SystemApprovePendingLeave( requestContext, id );
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    // Recalculate & Rebuild Credit
                    var response = RecalculateLeaveCredit( id, requestContext );

                    if ( !response.Result )
                    {
                        TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.TransferLeaveCreditAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, TamamConstants.AuthorizationConstants.TransferLeaveCreditAuditMessageFailure, string.Empty );
                        context.Response.Set( ResponseState.Failure, false, new List<ModelMetaPair> { new ModelMetaPair( string.Empty, LeavesConstants.TransferFaild ) } );
                        return false;
                    }
                }

                // audit ...
                TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.TransferLeaveCreditAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, TamamConstants.AuthorizationConstants.RecalculateLeaveCreditAuditMessageSuccessful, string.Empty );

                context.Response.Set( ResponseState.Success, true );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Leaves );

                #endregion
                return true;

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> UpdateLeaveCreditsMetaData( Guid personId, DateTime originalEffectiveDate, DateTime updatedEffectiveDate, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                var currentCredit = dataHandler.GetLeaveCredit( personId, originalEffectiveDate, SystemSecurityContext.Instance ).Result;
                var nextCredit = dataHandler.GetLeaveCredit( personId, originalEffectiveDate.AddYears( 1 ), SystemSecurityContext.Instance ).Result;
                var previousCredit = dataHandler.GetLeaveCredit( personId, originalEffectiveDate.AddYears( -1 ), SystemSecurityContext.Instance ).Result;
                var preCredit = dataHandler.GetLeavePreCredit( personId, originalEffectiveDate ).Result;

                var personCredit = new List<LeaveCredit>();
                if ( currentCredit != null )
                {
                    currentCredit.EffectiveYearStart = updatedEffectiveDate;
                    personCredit.Add( currentCredit );
                }
                if ( nextCredit != null )
                {
                    nextCredit.EffectiveYearStart = updatedEffectiveDate.AddYears( 1 );
                    personCredit.Add( nextCredit );
                }
                if ( previousCredit != null )
                {
                    previousCredit.EffectiveYearStart = updatedEffectiveDate.AddYears( -1 );
                    personCredit.Add( previousCredit );
                }

                var response_save = dataHandler.UpdateLeaveCredit( personCredit );
                if ( response_save.Type != ResponseState.Success )
                {
                    context.Response.Set( response_save );
                    return;
                }
                preCredit.EffectiveYearStart = updatedEffectiveDate;
                response_save = dataHandler.UpdatePreCredit( preCredit );
                if ( response_save.Type != ResponseState.Success )
                {
                    context.Response.Set( response_save );
                    return;
                }

                context.Response.Set( ResponseState.Success, true );
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Leaves );

                #endregion
                #endregion
            }, requestContext );

            return context.Response;
        }

        #endregion

        # region Excuses

        public ExecutionResponse<bool> CreateExcuses( List<Excuse> excuses, RequestContext requestContext )
        {
            return this.CreateExcuses( excuses, false, requestContext );
        }
        public ExecutionResponse<bool> RequestExcuse( Excuse excuse, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                var target = excuse.ExcuseTypeId == ( int )ExcuseTypes.AwayExcuse ? TamamConstants.AuthorizationConstants.RequestAwayActionKey : TamamConstants.AuthorizationConstants.RequestExcuseActionKey;
                if ( !TamamServiceBroker.Authorize( requestContext, target ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.RequestExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.RequestExcuseAuditMessageFailure, excuse == null ? string.Empty : excuse.Id.ToString() ), excuse == null ? string.Empty : excuse.Id.ToString() );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region UpdateModel
                excuse.IsNative = true;
                excuse.Code = excuse.Id.ToString();
                #endregion
                #region Validation

                var validator = new ExcuseValidator( excuse, ExcuseValidator.ExcuseValidationMode.Request );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.RequestExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.RequestExcuseAuditMessageFailure, excuse.Id ), excuse.Id.ToString() );
                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    return;
                }

                #endregion

                //  Validate Excuse Duration
                var duration = SystemBroker.SchedulesHandler.GetScheduledHoursCount( excuse.PersonId, excuse.ExcuseDate, excuse.StartTime.TimeOfDay, excuse.EndTime.TimeOfDay ).Result;
                //excuse.Duration = Math.Round( duration, 2 );
                //var isValidHours = excuse.Duration != 0;
                var isValidHours = duration != 0 && GetDuration(excuse) == duration;
                if ( !isValidHours )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.RequestExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.RequestExcuseAuditMessageFailure, excuse.Id ), excuse.Id.ToString() );
                    context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "PersonId", string.Format( ValidationResources.ExcuseDurationEqualZero ) ) } );
                    return;
                }
                excuse.Duration = Math.Round(duration, 2);
                // validate excuses credit
                var response = CheckExcusesCredit( excuse.PersonId, ( ExcuseTypes )excuse.ExcuseTypeId, excuse.ExcuseDate, excuse.Duration, requestContext );

                if ( !response.Result )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.RequestExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.RequestExcuseAuditMessageFailure, excuse.Id ), excuse.Id.ToString() );
                    context.Response.Set( ResponseState.ValidationError, false, response.MessageDetailed );
                    return;
                }

                //Set Request Time
                excuse.RequestTime = DateTime.Now;

                // data layer ...
                var result = dataHandler.CreateExcuses( new List<Excuse>() { excuse } );
                if ( !result.Result )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.RequestExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.RequestExcuseAuditMessageFailure, excuse.Id ), excuse.Id.ToString() );
                    context.Response.Set( result );
                    return;
                }

                // Create Request if excuse status is Pending
                if ( excuse.ExcuseStatusId == ( int )ExcuseStatus.Pending )
                {
                    //TamamServiceBroker.OrganizationHandler.CreateRequest(excuse, requestContext);

                    if ( !Broker.WorkflowEngine.Initialize( excuse, approvalWorkflowDefinition ) )
                    {
                        context.Response.Set( ResponseState.SystemError, false );
                        return;
                    }
                }

                // audit ...
                TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.RequestExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.RequestExcuseAuditMessageSuccessful, excuse.Id ), excuse.Id.ToString() );

                context.Response.Set( ResponseState.Success, true );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Excuses );

                #endregion

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> EditExcuse( Excuse excuse, RequestContext requestContext )
        {
            return this.EditExcuse( excuse, false, requestContext );
        }
        public ExecutionResponse<Excuse> GetExcuse( Guid excuseId, RequestContext requestContext )
        {
            var context = new ExecutionContext<Excuse>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "LeavesHandler_GetExcuse" + excuseId + requestContext;
                if ( !requestContext.IgnoreCache )
                {
                    var cached = Broker.Cache.Get<Excuse>( TamamCacheClusters.Excuses, cacheKey );
                    if ( cached != null )
                    {
                        context.Response.Set( ResponseState.Success, cached );
                        return;
                    }
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.ExcuseAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.GetExcuseAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.GetExcuseActionKey,
                    messageForDenied: string.Format( TamamConstants.AuthorizationConstants.GetExcuseAuditMessageFailure, excuseId ),
                    messageForFailure: string.Format( TamamConstants.AuthorizationConstants.GetExcuseAuditMessageFailure, excuseId ),
                    messageForSuccess: string.Format( TamamConstants.AuthorizationConstants.GetExcuseAuditMessageSuccessful, excuseId )
                    );

                var responseExcuse = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                context.ActionContext.ActionKey = TamamConstants.AuthorizationConstants.GetAwayActionKey;
                var responseAway = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( responseExcuse.Type != ResponseState.Success && responseAway.Type != ResponseState.Success )
                {
                    context.Response.Set( responseExcuse.Type, null );
                    return;
                }

                var securityContext = responseExcuse.Result;

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.GetExcuse( excuseId, securityContext );
                if ( dataHandlerResponse.Result == null )
                {
                    context.Response.Set( dataHandlerResponse );
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.GetExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.GetExcuseAuditMessageFailure, excuseId ), string.Empty );
                    return;
                }

                if ( dataHandlerResponse.Result.ExcuseTypeId == ( int )ExcuseTypes.AwayExcuse )
                {
                    if ( responseAway.Type != ResponseState.Success )
                    {
                        context.Response.Set( responseAway.Type, null );
                        return;
                    }
                }
                else
                {
                    if ( responseExcuse.Type != ResponseState.Success )
                    {
                        context.Response.Set( responseExcuse.Type, null );
                        return;
                    }
                }
                context.Response.Set( dataHandlerResponse );

                #region Cache

                if ( !requestContext.IgnoreCache )
                {
                    Broker.Cache.Add<Excuse>( TamamCacheClusters.Excuses, cacheKey, dataHandlerResponse.Result );
                }

                #endregion

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<ExcuseSearchResult> SearchExcuses( ExcuseSearchCriteria criteria, RequestContext requestContext )
        {
            var context = new ExecutionContext<ExcuseSearchResult>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "LeavesHandler_SearchExcuses" + criteria + requestContext;
                var cached = Broker.Cache.Get<ExcuseSearchResult>( TamamCacheClusters.Excuses, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.ExcuseAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.SearchExcusesAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.SearchExcusesActionKey,
                    messageForDenied: TamamConstants.AuthorizationConstants.SearchExcusesAuditMessageAccessDenied,
                    messageForFailure: TamamConstants.AuthorizationConstants.SearchExcusesAuditMessageFailure,
                    messageForSuccess: TamamConstants.AuthorizationConstants.SearchExcusesAuditMessageSuccessful
                    );
                if ( criteria.ExcuseTypeId.HasValue )
                {
                    context.ActionContext.ActionKey = criteria.ExcuseTypeId.Value == ( int )ExcuseTypes.AwayExcuse ?
                        TamamConstants.AuthorizationConstants.SearchAwayActionKey :
                        TamamConstants.AuthorizationConstants.SearchExcusesActionKey;
                }
                else
                {
                    if ( !( requestContext is SystemRequestContext ) )
                    {
                        context.Response.Set( ResponseState.AccessDenied, null );
                        return;
                    }
                }
                var response = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type, null );
                    return;
                }

                var securityContext = response.Result;

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.SearchExcuses( criteria, securityContext );
                if ( dataHandlerResponse.Result == null )
                {
                    context.Response.Set( dataHandlerResponse );
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.SearchExcusesAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, TamamConstants.AuthorizationConstants.SearchExcusesAuditMessageAccessDenied, string.Empty );
                    return;
                }

                context.Response.Set( dataHandlerResponse );
                #region Cache

                Broker.Cache.Add<ExcuseSearchResult>( TamamCacheClusters.Excuses, cacheKey, dataHandlerResponse.Result );

                #endregion

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> CancelExcuse( Guid excuseId, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError, false, new List<ModelMetaPair>() );
                    return;
                }



                #endregion
                #region Validation

                var excuse = dataHandler.GetExcuse( excuseId, SystemSecurityContext.Instance ).Result;
                var target = excuse.ExcuseTypeId == ( int )ExcuseTypes.AwayExcuse ? TamamConstants.AuthorizationConstants.CancelAwayActionKey : TamamConstants.AuthorizationConstants.CancelExcuseActionKey;
                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, target ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CancelExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CancelExcuseAuditMessageFailure, excuseId ), excuseId.ToString() );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }
                var validator = new ExcuseValidator( excuse, ExcuseValidator.ExcuseValidationMode.Cancel );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CancelExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CancelExcuseAuditMessageFailure, excuseId ), excuseId.ToString() );
                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    return;
                }

                #endregion
                #region Advanced validation

                //if (excuse.ExcuseStatusId == (int)ExcuseStatus.Taken
                //    && excuse.PersonId == requestContext.PersonId)
                //{
                //    context.Response.Set(ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair(string.Empty, string.Format(ValidationResources.ExcuseCanCancelled, excuse.Person.GetLocalizedFullName())) });
                //    return;
                //}

                #endregion
                #region workflow ...

                // workflow : cancel ... ( + model )
                // Note: intentionally Cancel WF, not CancelAndDelete, because we still need this WF instance due to Checkpoints..
                var approvalData = new LeaveReviewWorkflowData( excuseId.ToString(), null, WorkflowLeaveReviewStatus.Cancelled.ToString(), string.Empty, WorkflowLeaveTargetType.Excuse );
                if ( !Broker.WorkflowEngine.Cancel( excuse, approvalWorkflowDefinition.Id, approvalData ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CancelExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CancelExcuseAuditMessageFailure, excuseId ), excuseId.ToString() );
                    context.Response.Set( ResponseState.SystemError, false );
                    return;
                }

                #endregion

                TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CancelExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CancelExcuseAuditMessageSuccessful, excuseId ), excuseId.ToString() );

                context.Response.Set( ResponseState.Success, true );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Excuses );

                #endregion

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> ReviewExcuse( ExcuseReview review, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region Validation

                var excuse = dataHandler.GetExcuse( review.ExcuseId, SystemSecurityContext.Instance ).Result;
                var target = excuse.ExcuseTypeId == ( int )ExcuseTypes.AwayExcuse ? TamamConstants.AuthorizationConstants.ReviewAwayActionKey : TamamConstants.AuthorizationConstants.ReviewExcuseActionKey;
                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, target ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.ReviewExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.ReviewExcuseAuditMessageFailure, review == null ? string.Empty : review.ExcuseId.ToString() ), review == null ? string.Empty : review.ExcuseId.ToString() );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }
                var validator = new ExcuseValidator( excuse, ExcuseValidator.ExcuseValidationMode.Review );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.ReviewExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.ReviewExcuseAuditMessageFailure, review.ExcuseId ), review.ExcuseId.ToString() );
                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    return;
                }

                #endregion
                #region workflow ...

                //var result = TamamServiceBroker.OrganizationHandler.UpdateRequestStatus(review.ExcuseId, review.Status == ReviewExcuseStatus.Approved ? WorkFlowStepStatus.Approved : WorkFlowStepStatus.Denied, review.Comment, requestContext);

                var command = review.Status == ReviewExcuseStatus.Approved ? WorkflowLeaveReviewStatus.Approved : WorkflowLeaveReviewStatus.Denied;
                var approvalData = new LeaveReviewWorkflowData( review.ExcuseId.ToString(), review.ReviewerId.ToString(), command.ToString(), review.Comment, WorkflowLeaveTargetType.Excuse );
                if ( !Broker.WorkflowEngine.Invoke( excuse, approvalWorkflowDefinition.Id, approvalData ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.ReviewExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.ReviewExcuseAuditMessageFailure, review.ExcuseId ), review.ExcuseId.ToString() );
                    context.Response.Set( ResponseState.SystemError, false );
                    return;
                }

                #endregion

                // audit ...
                TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.ReviewExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.ReviewExcuseAuditMessageSuccessful, review.ExcuseId ), review.ExcuseId.ToString() );

                context.Response.Set( ResponseState.Success, true );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Excuses );

                #endregion
                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<ExcuseAttachment> GetExcuseAttachment( Guid attachmentId, RequestContext requestContext )
        {
            var context = new ExecutionContext<ExcuseAttachment>();
            context.Execute( () =>
            {
                #region logic ...
                #region Cache

                var cacheKey = "LeavesHandler_GetExcuseAttachment" + attachmentId + requestContext;
                var cached = Broker.Cache.Get<ExcuseAttachment>( TamamCacheClusters.Excuses, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.ExcuseAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.GetExcuseAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.GetExcuseActionKey,
                    messageForDenied: TamamConstants.AuthorizationConstants.GetExcuseAuditMessageFailure,
                    messageForFailure: TamamConstants.AuthorizationConstants.GetExcuseAuditMessageFailure,
                    messageForSuccess: TamamConstants.AuthorizationConstants.GetExcuseAuditMessageSuccessful
                    );

                var response = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                var responseExcuse = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                context.ActionContext.ActionKey = TamamConstants.AuthorizationConstants.GetAwayActionKey;
                var responseAway = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( responseExcuse.Type != ResponseState.Success && responseAway.Type != ResponseState.Success )
                {
                    context.Response.Set( responseExcuse.Type, null );
                    return;
                }

                var securityContext = response.Result;

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.GetExcuseAttachment( attachmentId, securityContext );
                if ( dataHandlerResponse.Result == null )
                {
                    context.Response.Set( dataHandlerResponse );
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.GetExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, TamamConstants.AuthorizationConstants.GetExcuseAuditMessageFailure, string.Empty );
                    return;
                }

                // audit ...
                //TamamServiceBroker.Audit(requestContext, TamamConstants.AuthorizationConstants.GetExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format(TamamConstants.AuthorizationConstants.GetExcuseAttachmentAuditMessageSuccessful, attachmentId), string.Empty);

                context.Response.Set( dataHandlerResponse );
                #region Cache

                Broker.Cache.Add<ExcuseAttachment>( TamamCacheClusters.Excuses, cacheKey, dataHandlerResponse.Result );

                #endregion
                #endregion
            }, requestContext );

            return context.Response;
        }

        #endregion
        # region Excuse Credits

        public ExecutionResponse<bool> RecalculateExcuseDuration( Guid personId, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if (
                    !TamamServiceBroker.Authorize( requestContext,
                        TamamConstants.AuthorizationConstants.RecalculateLeaveCreditActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext,
                        TamamConstants.AuthorizationConstants.RecalculateExcuseDurationAuditActionId,
                        TamamConstants.AuthorizationConstants.ExcuseAuditModuleId,
                        string.Format(
                            TamamConstants.AuthorizationConstants.RecalculateExcuseDurationAuditMessageFailure, personId ),
                        string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // specify Start & End Date
                var startDate = new DateTime( DateTime.Today.Year, DateTime.Today.Month, 1 );
                var endDate = startDate.AddMonths( 2 ).AddDays( -1 );

                // get excuses..
                var excuses = GetExcuses( personId, startDate, endDate, requestContext );
                foreach ( var excuse in excuses )
                {
                    excuse.Duration = SystemBroker.SchedulesHandler.GetScheduledHoursCount( excuse.PersonId, excuse.ExcuseDate, excuse.StartTime.TimeOfDay, excuse.EndTime.TimeOfDay ).Result;
                }

                // update ...
                var result = dataHandler.UpdateExcuses( excuses );
                context.Response.Set( result );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Excuses );

                #endregion

            }, requestContext );

            return context.Response;
        }

        // for Create / Request
        public ExecutionResponse<bool> CheckExcusesCredit( Guid personId, ExcuseTypes excuseType, DateTime date, double duration, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.CheckLeaveCreditActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CheckLeaveCreditAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CheckLeaveCreditAuditMessageFailure, personId ), string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // models ...
                var person = GetPerson( personId, requestContext );
                var policies = GetPolicies( person, requestContext );
                var excusePolicy = GetPoliciesExcuse( policies, excuseType );

                if ( excusePolicy == null )
                {
                    // this person dosent has this excuse type policy
                    var message = new ModelMetaPair( string.Empty, string.Format( ValidationResources.PersonHasNoPolicyForExcuseType, person.GetLocalizedFullName() ) );
                    context.Response.Set( ResponseState.Failure, false, new List<ModelMetaPair>() { message } );
                    return;
                }

                if ( !excusePolicy.HasCredit )
                {
                    // No more validations required this excuse has no credits to calc
                    context.Response.Set( ResponseState.Success, true, new List<ModelMetaPair>() );
                    return;
                }

                var response = dataHandler.GetPersonExcusesByMonth( personId, date.Year, date.Month, excuseType );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( ResponseState.Failure, false, new List<ModelMetaPair>() );
                    return;
                }

                var excuses = response.Result.Where( x => !x.Duration.Equals( 0.0 ) ).ToList();

                // check max permissions per month (as Permissions Count)
                var maxCountResponse = CheckMaximumExcusesCountPerMonth( excuses.Count, excusePolicy, person.GetLocalizedFullName() );
                if ( maxCountResponse.Result == false )
                {
                    context.Response.Set( maxCountResponse.Type, false, maxCountResponse.MessageDetailed );
                    return;
                }

                // check max permissions per month (as Permissions Duration)
                var maxDurationResponse = CheckMaximumExcusesDurationPerMonth( excuses.Sum( x => x.Duration ) + duration, excusePolicy, person.GetLocalizedFullName() );
                if ( maxDurationResponse.Result == false )
                {
                    context.Response.Set( maxDurationResponse.Type, false, maxDurationResponse.MessageDetailed );
                    return;
                }

                // check max permissions per day (as Permissions Count)
                var maxCountPerDayResponse = CheckMaximumExcusesCountPerDay( excuses.Count( x => x.ExcuseDate.Day == date.Day ), excusePolicy, person.GetLocalizedFullName() );
                if ( maxCountPerDayResponse.Result == false )
                {
                    context.Response.Set( maxCountPerDayResponse.Type, false, maxCountPerDayResponse.MessageDetailed );
                    return;
                }

                // check min duration per permission
                var minExcuseDurationResponse = CheckMinimumExcuseDuration( duration, excusePolicy, person.GetLocalizedFullName() );
                if ( minExcuseDurationResponse.Result == false )
                {
                    context.Response.Set( minExcuseDurationResponse.Type, false, minExcuseDurationResponse.MessageDetailed );
                    return;
                }

                // check max permissions per day (as Permissions Duration)
                var maxDurationPerDayResponse =
                    CheckMaximumExcusesDurationPerDay(
                        ( excuses.Where( x => x.ExcuseDate.Day == date.Day ).Sum( x => x.Duration ) + duration ), excusePolicy, person.GetLocalizedFullName() );
                if ( maxDurationPerDayResponse.Result == false )
                {
                    context.Response.Set( maxDurationPerDayResponse.Type, false, maxDurationPerDayResponse.MessageDetailed );
                    return;
                }

                # region Deleted

                // check max permissions per month (as Permissions Count)
                //if (excuses.Count == excusePolicy.MaxPermissionsMonth)
                //{
                //    context.Response.Set(ResponseState.Success, false,
                //        new List<ModelMetaPair>
                //        {
                //            new ModelMetaPair(string.Empty, Leaves.NoExcusesCredit)
                //        });
                //    return;
                //}

                // check max permissions per month (as Permissions Duration)
                //if ((excuses.Sum(x => x.Duration) + duration) > excusePolicy.AllowedHoursMonth)
                //{
                //    context.Response.Set(ResponseState.Success, false,
                //        new List<ModelMetaPair> { new ModelMetaPair(string.Empty, Leaves.ExceedExcusesCredit) });
                //    return;
                //}

                // check max permissions per day (as Permissions Count)
                //if (excuses.Count(x => x.Date.Day == date.Day) == excusePolicy.MaxPermissionsDay)
                //{
                //    context.Response.Set(ResponseState.Success, false,
                //        new List<ModelMetaPair>
                //        {
                //            new ModelMetaPair(string.Empty, Leaves.ExceedExcusesCreditDay)
                //        });
                //    return;
                //}

                // check min duration per permission
                //if (duration < excusePolicy.MinExcuseDuration)
                //{
                //    context.Response.Set(ResponseState.Success, false,
                //        new List<ModelMetaPair>
                //        {
                //            new ModelMetaPair(string.Empty,
                //                string.Format(Leaves.NoExcusesCreditHours, excusePolicy.MinExcuseDuration))
                //        });
                //    return;
                //}

                // check max permissions per day (as Permissions Duration)
                //if ((excuses.Where(x => x.Date.Day == date.Day).Sum(x => x.Duration) + duration) > excusePolicy.AllowedHoursDay)
                //{
                //    context.Response.Set(ResponseState.Success, false,
                //        new List<ModelMetaPair>
                //        {
                //            new ModelMetaPair(string.Empty, Leaves.ExceedExcusesCreditDay2)
                //        });
                //    return;
                //}


                #endregion

                // 1- check max permissions per month        (as count)
                // if reach to max  --> return false;

                //else
                // check max permissions per month          (as period)
                // if reach to max  --> return false;
                //else

                // 2- check max permissions per today
                // if reach to max --> return false;
                //else
                // 

                // audit ...
                //TamamServiceBroker.Audit(requestContext, TamamConstants.AuthorizationConstants.CheckLeaveCreditAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format(TamamConstants.AuthorizationConstants.CheckLeaveCreditAuditMessageSuccessful, personId), string.Empty);

                context.Response.Set( ResponseState.Success, true, new List<ModelMetaPair>() );

                #endregion
            }, requestContext );

            return context.Response;
        }

        // for Edit
        public ExecutionResponse<bool> CheckExcusesCredit( Excuse oldExcuse, DateTime date, double duration, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.CheckLeaveCreditActionKey ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CheckLeaveCreditAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CheckLeaveCreditAuditMessageFailure, oldExcuse.PersonId ), string.Empty );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                // models ...
                var person = GetPerson( oldExcuse.PersonId, requestContext );
                var policies = GetPolicies( person, requestContext );
                var excusePolicy = GetPoliciesExcuse( policies, ( ExcuseTypes )oldExcuse.ExcuseTypeId );

                if ( excusePolicy == null )
                {
                    // this person dosent has this excuse type policy
                    var message = new ModelMetaPair( string.Empty, string.Format( ValidationResources.PersonHasNoPolicyForExcuseType, person.GetLocalizedFullName() ) );
                    context.Response.Set( ResponseState.Failure, false, new List<ModelMetaPair>() { message } );
                    return;
                }

                if ( !excusePolicy.HasCredit )
                {
                    // No more validations required this excuse has no credits to calc
                    context.Response.Set( ResponseState.Success, true, new List<ModelMetaPair>() );
                    return;
                }

                var response = dataHandler.GetPersonExcusesByMonth( oldExcuse.PersonId, date.Year, date.Month, ( ExcuseTypes )oldExcuse.ExcuseTypeId );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( ResponseState.Failure, false, new List<ModelMetaPair>() );
                    return;
                }

                var excuses = response.Result.Where( x => x.Id != oldExcuse.Id && !x.Duration.Equals( 0.0 ) ).ToList();

                // check max permissions per month (as Permissions Count)
                var maxCountResponse = CheckMaximumExcusesCountPerMonth( excuses.Count, excusePolicy, person.GetLocalizedFullName() );
                if ( maxCountResponse.Result == false )
                {
                    context.Response.Set( maxCountResponse.Type, false, maxCountResponse.MessageDetailed );
                    return;
                }

                // check max permissions per month (as Permissions Duration)
                var maxDurationResponse = CheckMaximumExcusesDurationPerMonth( excuses.Sum( x => x.Duration ) + duration, excusePolicy, person.GetLocalizedFullName() );
                if ( maxDurationResponse.Result == false )
                {
                    context.Response.Set( maxDurationResponse.Type, false, maxDurationResponse.MessageDetailed );
                    return;
                }

                // check max permissions per day (as Permissions Count)
                var maxCountPerDayResponse = CheckMaximumExcusesCountPerDay( excuses.Count( x => x.ExcuseDate.Day == date.Day ), excusePolicy, person.GetLocalizedFullName() );
                if ( maxCountPerDayResponse.Result == false )
                {
                    context.Response.Set( maxCountPerDayResponse.Type, false, maxCountPerDayResponse.MessageDetailed );
                    return;
                }

                // check min duration per permission
                var minExcuseDurationResponse = CheckMinimumExcuseDuration( duration, excusePolicy, person.GetLocalizedFullName() );
                if ( minExcuseDurationResponse.Result == false )
                {
                    context.Response.Set( minExcuseDurationResponse.Type, false, minExcuseDurationResponse.MessageDetailed );
                    return;
                }

                // check max permissions per day (as Permissions Duration)
                var maxDurationPerDayResponse =
                    CheckMaximumExcusesDurationPerDay(
                        ( excuses.Where( x => x.ExcuseDate.Day == date.Day ).Sum( x => x.Duration ) + duration ), excusePolicy, person.GetLocalizedFullName() );
                if ( maxDurationPerDayResponse.Result == false )
                {
                    context.Response.Set( maxDurationPerDayResponse.Type, false,
                        maxDurationPerDayResponse.MessageDetailed );
                    return;
                }

                // audit ...
                //TamamServiceBroker.Audit(requestContext, TamamConstants.AuthorizationConstants.CheckLeaveCreditAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format(TamamConstants.AuthorizationConstants.CheckLeaveCreditAuditMessageSuccessful, oldExcuse.PersonId), string.Empty);

                context.Response.Set( ResponseState.Success, true );

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<ExcuseCredit> GetExcusesCredit( Guid personId, int year, int monthNo, RequestContext requestContext )
        {
            var context = new ExecutionContext<ExcuseCredit>();
            context.Execute( () =>
            {
                #region logic ...
                #region Cache

                var cacheKey = "LeavesHandler_GetExcusesCredit" + personId + year + monthNo + requestContext;
                var cached = Broker.Cache.Get<ExcuseCredit>( TamamCacheClusters.Excuses, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.ExcuseAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.GetExcuseAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.GetExcuseActionKey,
                    messageForDenied: string.Empty,
                    messageForFailure: string.Empty,
                    messageForSuccess: string.Empty
                    );

                var securityResponse = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( securityResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( securityResponse.Type, null );
                    return;
                }

                var securityContext = securityResponse.Result;

                #endregion

                // models ...
                var policies = GetPolicies( personId, requestContext );
                var excusePolicy = GetPoliciesExcuse( policies, ExcuseTypes.NormalExcuse );
                var excuseCredit = new ExcuseCredit { PersonId = personId, MonthNo = monthNo };

                if ( excusePolicy == null )
                {
                    excuseCredit.MaxExcusesPerMonth = 0;
                    excuseCredit.MaxHoursPerMonth = 0;
                }
                else
                {
                    excuseCredit.MaxExcusesPerMonth = excusePolicy.MaxExcusesPerMonth.GetValueOrDefault();
                    excuseCredit.MaxHoursPerMonth = excusePolicy.AllowedHoursPerMonth.GetValueOrDefault();
                }

                var dataHandlerResponse = dataHandler.GetPersonExcusesByMonth( personId, year, monthNo, ExcuseTypes.NormalExcuse );
                if ( dataHandlerResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( ResponseState.Failure, null );
                    return;
                }
                else
                {
                    excuseCredit.ConsumedExcusesPerMonth = dataHandlerResponse.Result.Count;
                    var totalDuration = dataHandlerResponse.Result.Sum( e => e.Duration );
                    excuseCredit.ConsumedHoursPerMonth = Math.Round( totalDuration, 2 );
                }

                context.Response.Set( ResponseState.Success, excuseCredit );
                #region Cache

                Broker.Cache.Add<ExcuseCredit>( TamamCacheClusters.Excuses, cacheKey, excuseCredit );

                #endregion

                #endregion
            }, requestContext );

            return context.Response;
        }


        #endregion

        #region Approval Workflow Support

        public ExecutionResponse<List<WorkflowCheckPoint>> ApprovalStepsGet( Guid targetId, RequestContext requestContext )
        {
            var context = new ExecutionContext<List<WorkflowCheckPoint>>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "LeavesHandler_ApprovalStepsGet" + targetId + requestContext;
                var cached = Broker.Cache.Get<List<WorkflowCheckPoint>>( TamamCacheClusters.Leaves, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region logic ...

                #region Security

                context.ActionContext = new ActionContext
                    (
                    moduleId: TamamConstants.AuthorizationConstants.LeaveAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.GetLeaveAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.GetLeaveActionKey,
                    messageForDenied: string.Format( TamamConstants.AuthorizationConstants.GetLeaveAuditMessageFailure, targetId ),
                    messageForFailure: string.Format( TamamConstants.AuthorizationConstants.GetLeaveAuditMessageFailure, targetId ),
                    messageForSuccess: string.Format( TamamConstants.AuthorizationConstants.GetLeaveAuditMessageSuccessful, targetId )
                    );

                var response = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type, null );
                    return;
                }

                var securityContext = response.Result;

                #endregion

                var instance = Broker.WorkflowEngine.GetInstance( targetId, approvalWorkflowDefinition.Id );
                if ( instance == null )
                {
                    context.Response.Set( ResponseState.NotFound, null );
                    return;
                }

                var checkPoints = instance.CheckPoints;
                var checkPointsStack = instance.CPS;
                if ( checkPointsStack != null && checkPointsStack.Count > 0 )
                {
                    checkPoints.AddRange( checkPointsStack.ToList() );
                }

                context.Response.Set( ResponseState.Success, checkPoints );

                #endregion
                #region Cache

                Broker.Cache.Add<List<WorkflowCheckPoint>>( TamamCacheClusters.Leaves, cacheKey, checkPoints );

                #endregion

            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> ApprovalIntegrityMaintainByOwner( Guid ownerId, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                if ( !( requestContext is SystemRequestContext ) )
                {
                    context.Response.Set( ResponseState.AccessDenied, false );
                    return;
                }

                #endregion
                #region Leaves ...

                var criteriaLeaves = new LeaveSearchCriteria { Personnel = new List<Guid> { ownerId }, LeaveStatuses = new List<int> { ( int )LeaveStatus.Pending, ( int )LeaveStatus.Planned } };
                var responseLeaves = dataHandler.SearchLeaves( criteriaLeaves, true, SystemSecurityContext.Instance );
                if ( responseLeaves.Type != ResponseState.Success )
                {
                    context.Response.Set( ResponseState.SystemError, false );
                    return;
                }

                foreach ( var leave in responseLeaves.Result.Leaves )
                {
                    if ( !Broker.WorkflowEngine.Maintain( leave, approvalWorkflowDefinition ) )
                    {
                        context.Response.Set( ResponseState.SystemError, false );
                        return;
                    }
                }

                #endregion
                #region Excuses ...

                var criteriaExcuses = new ExcuseSearchCriteria { Personnel = new List<Guid> { ownerId }, ExcuseStatuses = new List<int> { ( int )ExcuseStatus.Pending }, ActivePersonnelStatus = true, };
                var responseExcuses = dataHandler.SearchExcuses( criteriaExcuses, SystemSecurityContext.Instance );
                if ( responseExcuses.Type != ResponseState.Success )
                {
                    context.Response.Set( ResponseState.SystemError, false );
                    return;
                }

                foreach ( var excuse in responseExcuses.Result.Excuses )
                {
                    if ( !Broker.WorkflowEngine.Maintain( excuse, approvalWorkflowDefinition ) )
                    {
                        context.Response.Set( ResponseState.SystemError, false );
                        return;
                    }
                }

                #endregion

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Leaves );
                Broker.Cache.Invalidate( TamamCacheClusters.Excuses );

                #endregion

                context.Response.Set( ResponseState.Success, true );

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> ApprovalIntegrityMaintainByReviewer( Guid reviewerId, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                if ( !( requestContext is SystemRequestContext ) )
                {
                    return context.Response.Set( ResponseState.AccessDenied, false );
                }

                #endregion
                #region instances to review in ...

                var instancesToReview = Broker.WorkflowEngine.GetInstancesByAffinity( reviewerId.ToString(), WorkflowInstanceStatus.InProgress );

                if ( instancesToReview == null ) return context.Response.Set( ResponseState.Success, true );

                var owners = instancesToReview.Select( x => x.PersonId ).Distinct().ToList();

                foreach ( var owner in owners )
                {
                    var response = ApprovalIntegrityMaintainByOwner( owner, requestContext );
                    if ( response.Type != ResponseState.Success || response.Result == false ) return context.Response.Set( response );
                }

                #endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Leaves );
                Broker.Cache.Invalidate( TamamCacheClusters.Excuses );

                #endregion

                return context.Response.Set( ResponseState.Success, true );

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> ApprovalInstancesCancel( Guid personId, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Security

                if ( !( requestContext is SystemRequestContext ) ) return context.Response.Set( ResponseState.AccessDenied, false );

                #endregion
                #region to review in ...

                // TODO : NOTE : check if we still need this section ,, most likely we won't ...

                var instancesToReview = Broker.WorkflowEngine.GetInstancesByAffinity( personId.ToString(), WorkflowInstanceStatus.InProgress );
                if ( instancesToReview != null )
                {
                    foreach ( var instance in instancesToReview )
                    {
                        if ( instance.Status != WorkflowInstanceStatus.InProgress ) continue;
                        if ( !Broker.WorkflowEngine.CancelAndDelete( instance.Id, null ) ) return context.Response.Set( ResponseState.SystemError, false );
                    }
                }

                #endregion
                #region owned ...

                var instancesOwned = Broker.WorkflowEngine.GetInstancesByOwner( personId );
                if ( instancesOwned != null )
                {
                    foreach ( var instance in instancesOwned )
                    {
                        if ( instance.Status != WorkflowInstanceStatus.InProgress ) continue;
                        if ( !Broker.WorkflowEngine.CancelAndDelete( instance.Id, null ) ) return context.Response.Set( ResponseState.SystemError, false );
                    }
                }

                #endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Leaves );
                Broker.Cache.Invalidate( TamamCacheClusters.Excuses );

                #endregion

                return context.Response.Set( ResponseState.Success, true );

                #endregion
            }, requestContext );

            return context.Response;
        }

        public ExecutionResponse<bool> CanPersonCancelLeave( Guid targetId, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region Logic

                #region Cache

                var cacheKey = "LeavesHandler_CanPersonCancelLeave" + targetId + requestContext;
                var cached = Broker.Cache.Get<bool?>( TamamCacheClusters.Leaves, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached.Value );
                    return;
                }

                #endregion

                bool canCancelLeave = false;

                #region Leave

                var responseLeave = GetLeave( targetId, SystemRequestContext.Instance );
                if ( responseLeave.Type != ResponseState.Success )
                {
                    context.Response.Set( responseLeave.Type, false );
                    return;
                }

                var leave = responseLeave.Result;

                #endregion
                #region Owner ?

                // owner or can act ?
                var canActResponse = TamamServiceBroker.PersonnelHandler.CanActFor( requestContext.PersonId.Value, leave.PersonId, SystemRequestContext.Instance );
                if ( canActResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( ResponseState.Failure, false );
                    return;
                }

                canCancelLeave = canActResponse.Result;
                if ( canCancelLeave )
                {
                    context.Response.Set( ResponseState.Success, true );

                    #region Cache

                    Broker.Cache.Add<bool?>( TamamCacheClusters.Leaves, cacheKey, canCancelLeave );

                    #endregion
                    return;
                }

                #endregion
                #region HR ?

                // HR ?
                var hr = GetPersonHR( leave.PersonId );
                if ( hr == null )
                {
                    context.Response.Set( ResponseState.Success, false );
                    return;
                }

                var canActForHrResponse = TamamServiceBroker.PersonnelHandler.CanActFor( requestContext.PersonId.Value, hr.Id, SystemRequestContext.Instance );
                if ( canActForHrResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( ResponseState.SystemError, false );
                    return;
                }

                canCancelLeave = canActForHrResponse.Result;
                if ( canCancelLeave )
                {
                    context.Response.Set( ResponseState.Success, true );

                    #region Cache

                    Broker.Cache.Add<bool?>( TamamCacheClusters.Leaves, cacheKey, canCancelLeave );

                    #endregion
                    return;
                }

                #endregion
                #region Cache

                Broker.Cache.Add<bool?>( TamamCacheClusters.Leaves, cacheKey, canCancelLeave );

                #endregion

                // ...
                context.Response.Set( ResponseState.Success, canCancelLeave );

                #endregion

            }, requestContext );
            return context.Response;
        }
        public ExecutionResponse<bool> CanPersonCancelExcuse( Guid targetId, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region Logic ...

                #region Cache

                var cacheKey = "LeavesHandler_CanPersonCancelExcuse" + targetId + requestContext;
                var cached = Broker.Cache.Get<bool?>( TamamCacheClusters.Excuses, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached.Value );
                    return;
                }

                #endregion

                bool canCancelExcuse = false;

                #region Excuse

                var responseExcuse = GetExcuse( targetId, SystemRequestContext.Instance );
                if ( responseExcuse.Type != ResponseState.Success )
                {
                    context.Response.Set( responseExcuse.Type, false );
                    return;
                }

                var excuse = responseExcuse.Result;

                #endregion
                #region Owner ?

                var canActResponse = TamamServiceBroker.PersonnelHandler.CanActFor( requestContext.PersonId.Value, excuse.PersonId, SystemRequestContext.Instance );
                if ( canActResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( ResponseState.Failure, false );
                    return;
                }

                canCancelExcuse = canActResponse.Result;
                if ( canCancelExcuse )
                {
                    context.Response.Set( ResponseState.Success, true );

                    #region Cache

                    Broker.Cache.Add<bool?>( TamamCacheClusters.Excuses, cacheKey, canCancelExcuse );

                    #endregion
                    return;
                }

                #endregion
                #region HR ?

                var hr = GetPersonHR( excuse.PersonId );
                if ( hr == null )
                {
                    context.Response.Set( ResponseState.Success, false );
                    return;
                }

                var canActForHrResponse = TamamServiceBroker.PersonnelHandler.CanActFor( requestContext.PersonId.Value, hr.Id, SystemRequestContext.Instance );
                if ( canActForHrResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( ResponseState.Success, true );
                    return;
                }

                canCancelExcuse = canActForHrResponse.Result;
                if ( canCancelExcuse )
                {
                    context.Response.Set( ResponseState.Success, true );

                    #region Cache

                    Broker.Cache.Add<bool?>( TamamCacheClusters.Excuses, cacheKey, canCancelExcuse );

                    #endregion
                    return;
                }

                #endregion
                #region Cache

                Broker.Cache.Add<bool?>( TamamCacheClusters.Excuses, cacheKey, canCancelExcuse );

                #endregion

                // ...
                context.Response.Set( ResponseState.Success, canCancelExcuse );

                #endregion

            }, requestContext );
            return context.Response;
        }
        public ExecutionResponse<bool> CanPersonReviewTarget( Guid targetId, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                // security ...
                if ( requestContext is SystemRequestContext ) return context.Response.Set( ResponseState.Success, true );
                if ( !requestContext.PersonId.HasValue ) return context.Response.Set( ResponseState.Success, false );
                if ( Broker.AuthorizationHandler.Authorize( requestContext.PersonId.Value, TamamConstants.AuthorizationConstants.ReviewLeaveActionKey ) == false ) return context.Response.Set( ResponseState.Success, false );

                // approval instance ...
                var approvalInstance = Broker.WorkflowEngine.GetInstance( targetId, approvalWorkflowDefinition.Id );
                if ( approvalInstance == null || approvalInstance.Status != WorkflowInstanceStatus.InProgress ) return context.Response.Set( ResponseState.NotFound, false );

                // pending step ...
                var currentStep = approvalInstance.CurrentStep;
                var currentAction = currentStep != null ? currentStep.Action : null;
                var currentData = currentAction != null ? currentAction.Data as LeaveReviewWorkflowData : null;
                var expectedReviewer = currentData != null ? currentData.ApproverIdExpected : null;

                // error ...
                if ( expectedReviewer == null ) return context.Response.Set( ResponseState.Success, false );

                // check if logged in person is the expected reviewer
                if ( requestContext.PersonId == new Guid( expectedReviewer ) ) return context.Response.Set( ResponseState.Success, true );

                //  check if logged in person is a delegate for expected reviewer
                var canActResponse = TamamServiceBroker.PersonnelHandler.CanActFor( requestContext.PersonId.Value, new Guid( expectedReviewer ), SystemRequestContext.Instance );
                if ( canActResponse.Type != ResponseState.Success ) return context.Response.Set( ResponseState.Failure, false );

                // ...
                return context.Response.Set( ResponseState.Success, canActResponse.Result );

            }, requestContext );
            return context.Response;
        }

        #endregion

        #region Helpers ...

        private static double GetDuration(Excuse excuse)
        {
            if (excuse == null) return 0;

            var d = excuse.StartTime <= excuse.EndTime ? (excuse.EndTime - excuse.StartTime).TotalHours : 
                (excuse.EndTime.TimeOfDay.Add(new TimeSpan(24,0,0)) - excuse.StartTime.TimeOfDay).TotalHours;

            return d;
        }

        private void SystemApprovePendingLeave( RequestContext requestContext, Guid personId )
        {
            //1- Select any pending leave for this person id dated in the boundary of the credit of the pervious annual year
            var person = GetPerson( personId, requestContext );
            var policies = GetPolicies( person, requestContext );
            var accrualPolicy = GetPoliciesAccrual( policies );
            var effectiveDate = accrualPolicy.GetAccrualPolicyStartDate( person.AccountInfo.JoinDate.Value );
            //that will return today date
            var yearStart = effectiveDate.AddYears( -1 );
            var yearEnd = effectiveDate.AddDays( -1 );
            var pendingLeaves = GetLeaves( personId, yearStart, yearEnd, ( int )LeaveStatus.Pending, requestContext );
            //Approve all leaves
            foreach ( var pendingLeave in pendingLeaves )
            {
                var finishData = new LeaveReviewWorkflowData( pendingLeave.Id.ToString(), string.Empty,
                    WorkflowLeaveReviewStatus.SystemApproved.ToString(),
                    WorkflowLeaveReviewStatus.SystemApproved.ToString(), WorkflowLeaveTargetType.Leave );
                var state = Broker.WorkflowEngine.Invoke( pendingLeave, approvalWorkflowDefinition.Id, finishData );
            }
        }
        private LeaveCredit GetPersonLeaveCredit( Guid personId, DateTime effectiveDate, SecurityContext securityContext )
        {
            var response = dataHandler.GetLeaveCredit( personId, effectiveDate, securityContext );
            if ( response.Type != ResponseState.Success ) return null;

            return response.Result;
        }
        private LeaveCredit GetPersonLeaveCredit( Guid personId, DateTime effectiveDate, int leaveCreditStatusId, SecurityContext securityContext )
        {
            var response = dataHandler.GetLeaveCredit( personId, effectiveDate, leaveCreditStatusId, securityContext );
            if ( response.Type != ResponseState.Success ) return null;

            return response.Result;
        }
        private bool ResetPersonLeaveCredit( Person person, DateTime effectiveDate, SecurityContext securityContext )
        {
            var response = dataHandler.GetLeaveCredit( person.Id, effectiveDate, securityContext );
            if ( response.Type == ResponseState.NotFound ) return true;
            if ( response.Type != ResponseState.Success ) return false;

            var credit = response.Result;
            if ( credit == null ) return true;

            credit.IsDeleted = true;
            var response_update = dataHandler.UpdateLeaveCredit( credit );
            if ( response_update.Type != ResponseState.Success ) return false;

            return true;
        }
        private LeaveCredit GetPersonLeaveCreditPrevious( Person person, SecurityContext securityContext )
        {
            //Edit to get all credit with status Previous and is delete = false order by EffectiveDateStart Descending
            //So the first element in the returned list will be the last credit for the previous year
            var response = dataHandler.GetLeaveCreditHistory( person.Id, securityContext );
            if ( response.Type != ResponseState.Success ) return null;

            var credits = response.Result;
            if ( credits == null ) return null;

            #region commented code
            /*no need for filter the list by EffectiveYearStart not equal current as all list element has status previous*/
            //var currentEffectiveDate = accrualPolicy.GetAccrualPolicyStartDate(person.AccountInfo.JoinDate.Value);
            //credits = credits.Where(x => x.EffectiveYearStart.Date.Equals(currentEffectiveDate.Date) == false).ToList();
            #endregion

            if ( credits.Count == 0 ) return null;

            return credits[0];
        }

        private bool CalculatePreviousCredit( Guid personId, RequestContext requestContext, ExecutionContext<bool> context, Person person, List<Policy> policies, DateTime yearStart )
        {
            var previousYearStart = yearStart.AddYears( -1 );
            var previousYearEnd = previousYearStart.AddYears( 1 ).AddDays( -1 );
            var leavesInPreviousYear = GetLeaves( personId, previousYearStart, previousYearEnd, requestContext );

            //the person hire date is large than the previous annual year end -No credit for pervious year as the person is hired this year-
            if ( previousYearEnd < person.AccountInfo.JoinDate.Value )
            {
                return true;
            }
            var previousCredit = RebuildPreviousLeaveCredit( person, policies );
            // update leaves
            leavesInPreviousYear = UpdateLeavesEffectiveDays( leavesInPreviousYear, previousCredit.EffectiveYearStart, requestContext );
            // filter leaves (Planned && Pending && Approved & Taken)
            leavesInPreviousYear = leavesInPreviousYear.Where( x => x.LeaveStatusId == ( int )LeaveStatus.Planned || x.LeaveStatusId == ( int )LeaveStatus.Pending
                || x.LeaveStatusId == ( int )LeaveStatus.Approved || x.LeaveStatusId == ( int )LeaveStatus.Taken ).ToList();
            // update credit
            foreach ( var typeCredit in previousCredit.LeaveTypeCredits )
            {
                typeCredit.Consumed += leavesInPreviousYear.Where( x => x.LeaveTypeId == typeCredit.LeaveTypeId ).Sum( y => y.EffectiveDaysCount );
            }
            var responseupdate = dataHandler.UpdateLeaveCredit( previousCredit );
            if ( responseupdate.Type != ResponseState.Success )
            {
                context.Response.Set( responseupdate );
                return false;
            }
            return true;
        }

        private LeaveCredit RebuildLeaveCredit( Person person, List<Policy> policies )
        {
            // policies ...
            var accrualPolicy = GetPoliciesAccrual( policies );
            var leavePolicies = GetPoliciesLeaves( policies );

            // dates
            var hireDate = person.AccountInfo.JoinDate.Value;
            var effectiveCreditYearStart = accrualPolicy.GetAccrualPolicyStartDate( hireDate );
            var creditUtilization = GetLeaveCreditUtilization( effectiveCreditYearStart, hireDate );

            // Pre-Credit ...
            var preCredit = dataHandler.GetLeavePreCredit( person.Id, effectiveCreditYearStart ).Result;

            // model ...
            var credit = new LeaveCredit( Guid.NewGuid(), person.Id, effectiveCreditYearStart, ( int )LeaveCreditStatus.Current )
            {
                LeaveTypeCredits = new List<LeaveTypeCredit>(),
                LeaveTypeCarryOverCredits = new List<LeaveTypeCarryOverCredit>()
            };

            foreach ( var leavePolicy in leavePolicies )
            {
                var preCreditLeaveType = preCredit != null ? preCredit.LeaveTypePreCredits.FirstOrDefault( x => x.LeaveTypeId == ( int )leavePolicy.LeaveType.Value ) : null;
                var preCreditBalance = preCreditLeaveType != null ? preCreditLeaveType.Balance : 0;
                var preCreditConsumed = preCreditLeaveType != null ? preCreditLeaveType.Consumed : 0;
                var unlimitedCredit = leavePolicy.UnlimitedCredit.HasValue ? leavePolicy.UnlimitedCredit.Value : false;
                var policyBalance = unlimitedCredit ? effectiveCreditYearStart.AddYears( 1 ).AddDays( -1 ).DayOfYear : Math.Round( leavePolicy.AllowedAmount.Value * creditUtilization, Broker.Settings.RoundPolicy.RoundTo );

                var totalBalance = policyBalance + preCreditBalance;

                credit.LeaveTypeCredits.Add( new LeaveTypeCredit( Guid.NewGuid(), credit.Id, leavePolicy.Policy.Id, ( int )leavePolicy.LeaveType.Value, totalBalance, preCreditConsumed ) );
            }

            var previousCredit = GetPersonLeaveCreditPrevious( person, SystemSecurityContext.Instance );
            if ( previousCredit != null )
            {
                foreach ( var previousLeaveTypeCredit in previousCredit.LeaveTypeCredits )
                {
                    // check policies ...
                    var effectivePolicy = leavePolicies.FirstOrDefault( x => x.LeaveType == ( LeaveTypes? )previousLeaveTypeCredit.LeaveTypeId );

                    double availableCarryoverCredit = 0;

                    // carryover amount ...
                    var associatedCarryoverCredit = previousCredit.LeaveTypeCarryOverCredits.FirstOrDefault( x => x.LeaveTypeId == previousLeaveTypeCredit.LeaveTypeId );
                    var associatedCarryoverCreditAmount = associatedCarryoverCredit == null ? 0 : associatedCarryoverCredit.Credit;
                    if ( associatedCarryoverCredit != null && associatedCarryoverCredit.OverridePolicies )
                    {
                        availableCarryoverCredit = associatedCarryoverCreditAmount;
                    }
                    else
                    {
                        if ( effectivePolicy != null && effectivePolicy.SupportCarryOver.HasValue && effectivePolicy.SupportCarryOver.Value )
                        {
                            // presumed carryover amount ...
                            availableCarryoverCredit = previousLeaveTypeCredit.Amount - previousLeaveTypeCredit.Consumed + associatedCarryoverCreditAmount;

                            // adjusted carryover amount ...
                            availableCarryoverCredit = availableCarryoverCredit <= effectivePolicy.MaxCarryOverDays.Value ? availableCarryoverCredit : effectivePolicy.MaxCarryOverDays.Value;
                        }
                    }

                    // add to current credit as carry over ...
                    credit.LeaveTypeCarryOverCredits.Add( new LeaveTypeCarryOverCredit( Guid.NewGuid(), credit.Id, previousLeaveTypeCredit.LeaveTypeId, availableCarryoverCredit, false ) );
                }
            }
            else
            {
                foreach ( var leavePolicy in leavePolicies )
                {
                    credit.LeaveTypeCarryOverCredits.Add( new LeaveTypeCarryOverCredit( Guid.NewGuid(), credit.Id, ( int )leavePolicy.LeaveType.Value, 0, false ) );
                }
            }

            // data layer ( save ) ...
            var response = dataHandler.AddLeaveCredit( credit );
            if ( response.Type != ResponseState.Success ) return null;

            return response.Result;
        }
        private LeaveCredit RebuildNextYearLeaveCredit( Person person, List<Policy> policies )
        {
            // policies ...
            var accrualPolicy = GetPoliciesAccrual( policies );
            var leavePolicies = GetPoliciesLeaves( policies );

            // dates
            var hireDate = person.AccountInfo.JoinDate.Value;
            var effectiveCreditYearStart = accrualPolicy.GetAccrualPolicyStartDate( hireDate ).AddYears( 1 );
            var creditUtilization = GetLeaveCreditUtilization( effectiveCreditYearStart, hireDate );

            // model ...
            var credit = new LeaveCredit( Guid.NewGuid(), person.Id, effectiveCreditYearStart, ( int )LeaveCreditStatus.Next );

            credit.LeaveTypeCredits = new List<LeaveTypeCredit>();

            foreach ( var leavePolicy in leavePolicies )
            {
                var unlimitedCredit = leavePolicy.UnlimitedCredit.HasValue ? leavePolicy.UnlimitedCredit.Value : false;
                var policyBalance = unlimitedCredit ? effectiveCreditYearStart.AddYears( 1 ).AddDays( -1 ).DayOfYear : Math.Round( leavePolicy.AllowedAmount.Value * creditUtilization, Broker.Settings.RoundPolicy.RoundTo );

                credit.LeaveTypeCredits.Add( new LeaveTypeCredit( Guid.NewGuid(), credit.Id, leavePolicy.Policy.Id, ( int )leavePolicy.LeaveType.Value, policyBalance, 0 ) );
            }

            // data layer ( save ) ...
            var response = dataHandler.AddLeaveCredit( credit );
            if ( response.Type != ResponseState.Success ) return null;

            return response.Result;
        }
        private LeaveCredit RebuildPreviousLeaveCredit( Person person, List<Policy> policies )
        {
            // policies ...
            var accrualPolicy = GetPoliciesAccrual( policies );
            var leavePolicies = GetPoliciesLeaves( policies );

            // dates
            var hireDate = person.AccountInfo.JoinDate.Value;
            var effectiveCreditYearStart = accrualPolicy.GetAccrualPolicyStartDate( hireDate ).AddYears( -1 );
            var creditUtilization = GetLeaveCreditUtilization( effectiveCreditYearStart, hireDate );

            // Pre-Credit ...
            var preCredit = dataHandler.GetLeavePreCredit( person.Id, effectiveCreditYearStart ).Result;

            // model ...
            var credit = new LeaveCredit( Guid.NewGuid(), person.Id, effectiveCreditYearStart, ( int )LeaveCreditStatus.Previous )
            {
                LeaveTypeCredits = new List<LeaveTypeCredit>(),
                LeaveTypeCarryOverCredits = new List<LeaveTypeCarryOverCredit>()
            };

            foreach ( var leavePolicy in leavePolicies )
            {
                var preCreditLeaveType = preCredit != null ? preCredit.LeaveTypePreCredits.FirstOrDefault( x => x.LeaveTypeId == ( int )leavePolicy.LeaveType.Value ) : null;
                var preCreditBalance = preCreditLeaveType != null ? preCreditLeaveType.Balance : 0;
                var preCreditConsumed = preCreditLeaveType != null ? preCreditLeaveType.Consumed : 0;
                var unlimitedCredit = leavePolicy.UnlimitedCredit.HasValue ? leavePolicy.UnlimitedCredit.Value : false;
                var policyBalance = unlimitedCredit ? effectiveCreditYearStart.AddYears( 1 ).AddDays( -1 ).DayOfYear : Math.Round( leavePolicy.AllowedAmount.Value * creditUtilization, Broker.Settings.RoundPolicy.RoundTo );

                var totalBalance = policyBalance + preCreditBalance;

                credit.LeaveTypeCredits.Add( new LeaveTypeCredit( Guid.NewGuid(), credit.Id, leavePolicy.Policy.Id, ( int )leavePolicy.LeaveType.Value, totalBalance, preCreditConsumed ) );
                credit.LeaveTypeCarryOverCredits.Add( new LeaveTypeCarryOverCredit( Guid.NewGuid(), credit.Id, ( int )leavePolicy.LeaveType.Value, 0, false ) );
            }

            var response = dataHandler.AddLeaveCredit( credit );
            if ( response.Type != ResponseState.Success ) return null;

            return response.Result;
        }

        private List<Leave> UpdateLeavesEffectiveDays( List<Leave> leaves, DateTime effectiveYearStart, RequestContext requestContext )
        {
            foreach ( var leave in leaves )
            {
                var person = GetPerson( leave.PersonId, requestContext );
                var policies = GetPolicies( person, requestContext );
                var leavePolicy = GetPoliciesLeaves( policies, ( LeaveTypes )leave.LeaveTypeId );

                SetEffectiveDaysAccordingToLeavePolicy( leave, leavePolicy );
                leave.EffectiveYearStart = effectiveYearStart;

                // var daysCount = GetLeaveEffectiveDaysCount( leave , workingDays );
                //  leave.EffectiveDaysCount = daysCount;
            }

            // update
            var response = dataHandler.UpdateLeaves( leaves );
            if ( response.Type != ResponseState.Success ) return null;

            return leaves;
        }
        private double GetLeaveEffectiveDaysCount( Leave leave, List<DateTime> workingDays )
        {
            var daysCount = workingDays.Count( x => x >= leave.StartDate && x <= leave.EndDate );
            return leave.LeaveMode == LeaveMode.HalfDay && daysCount > 0 ? 0.5 : daysCount;
        }
        private double GetLeaveCreditUtilization( DateTime effectiveYearStart, DateTime hireDate )
        {
            var yearEnd = effectiveYearStart.AddYears( 1 ).AddDays( -1 );
            var totalDays = ( yearEnd - effectiveYearStart ).TotalDays;
            var actualDays = ( yearEnd - hireDate ).TotalDays;
            return Math.Min( 1, ( actualDays / totalDays ) );
        }
        private void SetEffectiveDaysAccordingToLeavePolicy( Leave leave, LeavePolicy leavePolicy )
        {
            bool includeOffDays = leavePolicy.IncludeWeekEndsAndHolidays != null && leavePolicy.IncludeWeekEndsAndHolidays.Value;
            double count = SystemBroker.SchedulesHandler.GetScheduledDaysCount( leave.PersonId, leave.StartDate, leave.EndDate, includeOffDays, true ).Result;

            count = leave.LeaveMode == LeaveMode.HalfDay && count == 1 ? 0.5 : count;
            leave.EffectiveDaysCount = count;
        }

        private Person GetPerson( Guid id, RequestContext requestContext )
        {
            var response = TamamServiceBroker.PersonnelHandler.GetPerson( id, requestContext );
            if ( response.Type != ResponseState.Success ) return null;

            return response.Result;
        }
        private Person GetPersonHR( Guid id )
        {
            var dataHandlerPersonnel = new PersonnelDataHandler();
            var dataHandlerOrganization = new OrganizationDataHandler();

            var policies = GetPolicies( id );
            policies = GetPolicies( policies, PolicyTypes.HRPolicyType );
            if ( policies == null || policies.Count == 0 ) return null;
            var HRPolicy = new HRPolicy( policies[0] );
            var HRDepartment = HRPolicy.HRDepartment;
            if ( HRDepartment == null ) return null;
            var HRDepartments = dataHandlerOrganization.GetDepartmentsByRoot( HRDepartment.Id ).Result;

            var criteria = new PersonSearchCriteria { ActivationStatus = true, Departments = HRDepartments.Select( d => d.Id ).ToList(), Titles = new List<int> { HRPolicy.HRRole.Id }, };
            var HRPerson = dataHandlerPersonnel.GetPersonnel( criteria, SystemSecurityContext.Instance ).Result.Persons.FirstOrDefault();

            return HRPerson;
        }

        private List<Policy> GetPolicies( Guid personId )
        {
            var response = TamamServiceBroker.OrganizationHandler.GetPolicies( personId, SystemRequestContext.Instance );
            if ( response.Type != ResponseState.Success || response.Result == null || response.Result.Count == 0 ) return null;

            return response.Result;
        }
        private List<Policy> GetPolicies( Guid personId, RequestContext requestContext )
        {
            var response = TamamServiceBroker.OrganizationHandler.GetPolicies( personId, requestContext );
            if ( response.Type != ResponseState.Success ) return null;

            return response.Result;
        }
        private List<Policy> GetPolicies( Person person, RequestContext requestContext )
        {
            return GetPolicies( person.Id, requestContext );
        }
        private List<Policy> GetPolicies( List<Policy> policies, string PolicyTypeId )
        {
            return policies == null ? null : policies.Where( p => p.PolicyTypeId == new Guid( PolicyTypeId ) ).ToList();
        }
        private AccrualPolicy GetPoliciesAccrual( List<Policy> policies )
        {
            var accrualPolicies = GetPolicies( policies, PolicyTypes.AccrualPolicyType );

            return accrualPolicies == null || accrualPolicies.Count == 0 ? null : new AccrualPolicy( accrualPolicies[0] );
        }
        private LeavePolicy GetPoliciesLeaves( List<Policy> policies, LeaveTypes leaveType )
        {
            var leavePoliciesNative = GetPolicies( policies, PolicyTypes.LeavePolicyType );
            var leavePolicies = leavePoliciesNative == null ? null : LeavePolicy.GetInstances( leavePoliciesNative, ( int )leaveType );

            return leavePolicies != null && leavePolicies.Count > 0 ? leavePolicies[0] : null;
        }
        private List<LeavePolicy> GetPoliciesLeaves( List<Policy> policies )
        {
            var leavePolicies = GetPolicies( policies, PolicyTypes.LeavePolicyType );
            return leavePolicies == null ? null : LeavePolicy.GetInstances( leavePolicies );
        }
        private ExcusePolicy GetPoliciesExcuse( List<Policy> policies, ExcuseTypes excuseType )
        {
            var excusePolicies = GetPolicies( policies, PolicyTypes.ExcusesPolicyType );
            var isPolicesExist = excusePolicies != null && excusePolicies.Any();
            if ( !isPolicesExist ) return null;

            var excusePolicy = ExcusePolicy.GetExcusePolicy( policies, excuseType );
            return excusePolicy;
        }

        private List<Leave> GetLeaves( Guid personId, DateTime from, DateTime to, RequestContext requestContext )
        {
            // get leaves
            var searchCriteria = new LeaveSearchCriteria( personId != Guid.Empty ? new List<Guid> { personId } : null, null, from, to, null, null, null, false, 0, 0 );

            var response = SearchLeaves( searchCriteria, true, requestContext );
            if ( response.Type != ResponseState.Success ) return null;

            return response.Result.Leaves;
        }
        private List<Leave> GetLeaves( Guid personId, DateTime from, DateTime to, int leaveStatus, RequestContext requestContext )
        {
            // get leaves
            var searchCriteria = new LeaveSearchCriteria( new List<Guid> { personId }, null, from, to, null, null, new List<int> { leaveStatus }, false, 0, 0 );

            var response = SearchLeaves( searchCriteria, true, requestContext );
            if ( response.Type != ResponseState.Success ) return null;

            return response.Result.Leaves;
        }
        private List<Excuse> GetExcuses( Guid personId, DateTime from, DateTime to, RequestContext requestContext )
        {
            var searchCriteria = new ExcuseSearchCriteria( new List<Guid> { personId }, null, from, to, null, null, true, false, 0, 0 );
            var response = SearchExcuses( searchCriteria, requestContext );
            if ( response.Type != ResponseState.Success ) return null;

            return response.Result.Excuses;
        }

        private ExecutionResponse<bool> CheckMaximumExcusesCountPerMonth( int count, ExcusePolicy policy, string personName )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                if ( count == policy.MaxExcusesPerMonth )
                {
                    context.Response.Set( ResponseState.Success, false, new List<ModelMetaPair> { new ModelMetaPair( string.Empty, string.Format( Leaves.NoExcusesCredit, personName ) ) } );
                    return;
                }

                context.Response.Set( ResponseState.Success, true );
            } );
            return context.Response;
        }
        private ExecutionResponse<bool> CheckMaximumExcusesDurationPerMonth( double duration, ExcusePolicy policy, string personName )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                if ( duration > policy.AllowedHoursPerMonth )
                {
                    context.Response.Set( ResponseState.Success, false, new List<ModelMetaPair> { new ModelMetaPair( string.Empty, string.Format( Leaves.ExceedExcusesCredit, personName ) ) } );
                    return;
                }

                context.Response.Set( ResponseState.Success, true );
            } );
            return context.Response;
        }
        private ExecutionResponse<bool> CheckMaximumExcusesCountPerDay( int count, ExcusePolicy policy, string personName )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                if ( count == policy.MaxExcusesPerDay )
                {
                    context.Response.Set( ResponseState.Success, false, new List<ModelMetaPair> { new ModelMetaPair( string.Empty, string.Format( Leaves.ExceedExcusesCreditDay, personName ) ) } );
                    return;
                }

                context.Response.Set( ResponseState.Success, true );
            } );
            return context.Response;
        }
        private ExecutionResponse<bool> CheckMinimumExcuseDuration( double duration, ExcusePolicy policy, string personName )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                if ( duration < policy.MinExcuseDuration )
                {
                    context.Response.Set( ResponseState.Success, false, new List<ModelMetaPair> { new ModelMetaPair( string.Empty, string.Format( Leaves.NoExcusesCreditHours, personName, policy.MinExcuseDuration ) ) } );
                    return;
                }

                context.Response.Set( ResponseState.Success, true );
            } );
            return context.Response;
        }
        private ExecutionResponse<bool> CheckMaximumExcusesDurationPerDay( double duration, ExcusePolicy policy, string personName )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                if ( duration > policy.AllowedHoursPerDay )
                {
                    context.Response.Set( ResponseState.Success, false, new List<ModelMetaPair> { new ModelMetaPair( string.Empty, string.Format( Leaves.ExceedExcusesCreditDay2, personName ) ) } );
                    return;
                }

                context.Response.Set( ResponseState.Success, true );
            } );
            return context.Response;
        }

        private ExecutionResponse<LeaveCredit> GetCreditForLeave( Guid personId, Leave leave, RequestContext requestContext )
        {
            var context = new ExecutionContext<LeaveCredit>();
            context.Execute( () =>
            {
                #region logic ...

                // models ...
                var LeaveCreditResult = GetLeaveCreditForLeaveDates( personId, leave.StartDate, leave.EndDate, requestContext );
                if ( LeaveCreditResult.Type == ResponseState.Success && LeaveCreditResult.Result == null )
                {
                    var response = RecalculateLeaveCredit( personId, requestContext );
                    if ( response.Type == ResponseState.Success )
                    {
                        LeaveCreditResult = GetLeaveCreditForLeaveDates( personId, leave.StartDate, leave.EndDate, requestContext );
                        if ( LeaveCreditResult.Type == ResponseState.NotFound )
                        {
                            context.Response.Set( ResponseState.NotFound, null );
                            return;
                        }
                    }
                    else
                    {
                        context.Response.Set( ResponseState.SystemError, null );
                        return;
                    }
                }
                else if ( LeaveCreditResult.Type == ResponseState.NotFound )
                {
                    context.Response.Set( ResponseState.NotFound, null );
                    return;
                }
                var credit = LeaveCreditResult.Result;
                context.Response.Set( ResponseState.Success, credit );
                #endregion
            }, requestContext );

            return context.Response;
        }

        private ExecutionResponse<double> GetCreditAmount( LeaveCredit credit, Leave leave, LeavePolicy policy, RequestContext requestContext )
        {
            var context = new ExecutionContext<double>();
            context.Execute( () =>
            {
                #region logic ...

                var creditStart = credit.EffectiveYearStart;
                var creditEnd = creditStart.AddYears( 1 ).AddDays( -1 );

                if ( leave.StartDate >= creditStart && leave.EndDate <= creditEnd )
                {
                    // credit ...
                    var leaveTypeCredit = credit.LeaveTypeCredits.FirstOrDefault( x => x.LeaveTypeId == leave.LeaveTypeId );
                    var carryOverCredit = credit.LeaveTypeCarryOverCredits.FirstOrDefault( x => x.LeaveTypeId == leave.LeaveTypeId );

                    double leaveTypeAmount = leaveTypeCredit == null ? 0 : leaveTypeCredit.Amount;
                    double leaveTypeConsumed = leaveTypeCredit == null ? 0 : leaveTypeCredit.Consumed;
                    double leaveCarryOver = carryOverCredit == null ? 0 : carryOverCredit.Credit;

                    double TotalAmount = 0;

                    if ( policy.ExceedsProgressiveCredit )
                    {
                        TotalAmount += leaveTypeAmount;
                    }
                    else
                    {
                        var percent = ( leave.EndDate.Date - credit.EffectiveYearStart.Date ).TotalDays / 365;
                        TotalAmount += leaveTypeAmount * percent;
                    }

                    TotalAmount += leaveCarryOver;
                    TotalAmount -= leaveTypeConsumed;

                    context.Response.Set( ResponseState.Success, Math.Round( TotalAmount, 2 ) );
                }
                else
                {
                    context.Response.Set( ResponseState.NotFound, 0 );
                }
                #endregion
            }, requestContext );

            return context.Response;
        }


        private string ListToString(List<Guid> ids)
        {
            if ( ids == null ) return "";
            return string.Join( ",", ids.ToArray() );
        }
        private string ListToString( List<int> ids )
        {
            if ( ids == null ) return "";
            return string.Join( ",", ids.ToArray() );
        }

        #endregion

        # endregion
        #region ISystemLeavesHandler

        public ExecutionResponse<bool> CreateLeaves( List<Leave> leaves, bool systemLevelAction, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Validation : input

                if ( leaves == null || !leaves.Any() )
                {
                    context.Response.Set( ResponseState.InvalidInput, false, new List<ModelMetaPair>() { new ModelMetaPair( "PersonId", ValidationResources.PersonIdEmpty ) } );
                    return;
                }

                if ( leaves.GroupBy( e => e.PersonId ).Any( g => g.Count() > 1 ) )
                {
                    context.Response.Set( ResponseState.InvalidInput, false, new List<ModelMetaPair>() { new ModelMetaPair( "PersonId", ValidationResources.DuplicatePersonId ) } );
                    return;
                }

                #endregion
                #region global variables setup

                var Ids = leaves.Select( l => l.Id );
                var IdsString = string.Join( ", ", Ids );

                #endregion
                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.CreateLeaveActionKey ) )
                {
                    var errorMessage = string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, IdsString );
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreateLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, errorMessage, IdsString );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion

                var now = DateTime.Now;
                var validaionErrors = new List<ModelMetaPair>();
                foreach ( var leave in leaves )
                {
                    leave.Code = systemLevelAction ? leave.Code : leave.Id.ToString();
                    leave.IsNative = !systemLevelAction;

                    #region Validation : Basic

                    var validationMode = systemLevelAction ? LeaveValidator.ValidationMode.SystemCreate : LeaveValidator.ValidationMode.Create;
                    var validator = new LeaveValidator( leave, validationMode );
                    if ( validator.IsValid != null && !validator.IsValid.Value )
                    {
                        TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreateLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                        validaionErrors.AddRange( validator.ErrorsDetailed );
                        continue;
                    }

                    #endregion

                    leave.Person = GetPerson( leave.PersonId, requestContext );
                    leave.RequestTime = now;   //Set Request Time
                    leave.RequestedBy = requestContext.Person;

                    //
                    var person = GetPerson( leave.PersonId, requestContext );
                    var policies = GetPolicies( person, requestContext );
                    var leavePolicy = GetPoliciesLeaves( policies, ( LeaveTypes )leave.LeaveTypeId );
                    if ( leavePolicy == null )
                    {
                        TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreateLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                        validaionErrors.AddRange( new List<ModelMetaPair> { new ModelMetaPair( "LeaveTypeId", string.Format( ValidationResources.LeaveInvalidLeaveTypePolicy, person.GetLocalizedFullName() ) ) } );
                        continue;
                    }

                    SetEffectiveDaysAccordingToLeavePolicy( leave, leavePolicy );

                    #region Validation : Advanced (credit / working days)

                    if ( !systemLevelAction )
                    {
                        int diff = leave.StartDate.Subtract( DateTime.Today ).Days;
                        // the requested leave is for today or future day
                        if ( diff >= 0 && leavePolicy.DisablePlannedLeaves.HasValue && leavePolicy.DisablePlannedLeaves.Value )
                        {
                            //if the leave policy disable the planned leave so the system will return error
                            TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreateLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                            context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "PersonId", ValidationResources.PlannedLeavesNotAllow ) } );
                            return;

                        }
                        // validate Days Limit for old dates..
                        if ( leavePolicy.DaysLimitForOldLeaves.HasValue && leavePolicy.DaysLimitForOldLeaves.Value > 0 )
                        {
                            var daysLimit = leavePolicy.DaysLimitForOldLeaves.Value;
                            var startDateLimit = DateTime.Today.AddDays( daysLimit * -1 );
                            if ( leave.StartDate < startDateLimit )
                            {
                                validaionErrors.Add( new ModelMetaPair( "StartDate", string.Format( ValidationResources.LeaveStartDateExceedsOldLeavesDaysLimit, leave.Person.GetLocalizedFullName() ) ) );
                                continue;
                            }
                        }

                        if ( leave.EffectiveDaysCount == 0 )
                        {
                            validaionErrors.Add( new ModelMetaPair( "StartDate", string.Format( ValidationResources.LeaveEffectiveDaysEqualZero, leave.Person.GetLocalizedFullName() ) ) );
                            continue;
                        }
                        var maxDaysPerRequest = leavePolicy.MaxDaysPerRequest.HasValue ? leavePolicy.MaxDaysPerRequest.Value : 0;
                        //0 value will disable the validation
                        if ( maxDaysPerRequest > 0 && leave.EffectiveDaysCount > maxDaysPerRequest )
                        {
                            validaionErrors.Add( new ModelMetaPair( "StartDate", string.Format( ValidationResources.LeaveEffectiveDayNotVaild, maxDaysPerRequest ) ) );
                            continue;
                        }

                        // validate leave credit
                        var LeaveCredit = GetCreditForLeave( leave.PersonId, leave, requestContext );
                        if ( LeaveCredit.Type == ResponseState.NotFound )
                        {
                            validaionErrors.Add( new ModelMetaPair( "PersonId", string.Format( ValidationResources.LeaveOutCredit, leave.Person.GetLocalizedFullName() ) ) );
                            continue;
                        }
                        var credit = LeaveCredit.Result;
                        var unlimitedCredit = leavePolicy.UnlimitedCredit.HasValue ? leavePolicy.UnlimitedCredit.Value : false;
                        if ( !unlimitedCredit )
                        {
                            var personCreditResult = GetCreditAmount( credit, leave, leavePolicy, requestContext );
                            if ( personCreditResult.Type == ResponseState.NotFound )
                            {
                                validaionErrors.Add( new ModelMetaPair( "PersonId", string.Format( ValidationResources.LeaveOutCredit, leave.Person.GetLocalizedFullName() ) ) );
                                continue;
                            }
                            var personCredit = personCreditResult.Result;
                            if ( leave.EffectiveDaysCount > personCredit )
                            {
                                TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreateLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                                validaionErrors.Add( new ModelMetaPair( "PersonId", string.Format( ValidationResources.LeaveTypeHasNoCredit, leave.Person.GetLocalizedFullName() ) ) );
                                continue;
                            }
                        }

                        leave.EffectiveYearStart = credit.EffectiveYearStart;
                    }

                    #endregion
                    # region Check Allow Request For half day leaves / Require Attachments..

                    if ( systemLevelAction && leave.LeaveMode == LeaveMode.HalfDay )
                    {
                        validaionErrors.Add( new ModelMetaPair( "LeaveTypeId", "Half Leaves are not supported in integration mode ..." ) );
                        continue;
                    }
                    else
                    {
                        // not allowed to request half days ?
                        if ( leave.LeaveMode == LeaveMode.HalfDay && ( !leavePolicy.AllowHalfDays.HasValue || !leavePolicy.AllowHalfDays.Value ) )
                        {
                            TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreateLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                            validaionErrors.Add( new ModelMetaPair( "LeaveTypeId", string.Format( ValidationResources.LeaveHalfDayNotAllowed, person.GetLocalizedFullName() ) ) );

                            continue;
                        }

                        // check for Leave Attachments..
                        if ( leavePolicy.RequireAttachments.HasValue && leavePolicy.RequireAttachments.Value )
                        {
                            if ( (leave.Attachments == null || leave.Attachments.Count == 0) && !systemLevelAction )
                            {
                                TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreateLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                                var leaveTypes = Broker.DetailCodeHandler.GetDetailCodesByMasterCode( TamamConstants.MasterCodes.LeaveType );
                                var associatedType = leaveTypes.FirstOrDefault( x => x.Id == leave.LeaveTypeId );
                                var leaveTypeName = requestContext.CultureName != null && requestContext.CultureName.ToLower().Contains("ar") ? associatedType.NameCultureVariant : associatedType.Name;
                                validaionErrors.Add( new ModelMetaPair( "LeaveTypeId", string.Format( ValidationResources.LeaveAttachmentRequired, leaveTypeName ) ) );

                                continue;
                            }
                        }
                    }

                    # endregion

                    leave.Person = null;   // need to clear the added person model, in order to avoid an issue with saving in the DataLayer (ORM sessions conflict)
                }

                // return if violating validations
                if ( validaionErrors.Any() )
                {
                    context.Response.Set( ResponseState.ValidationError, false, validaionErrors );
                    return;
                }

                #region data layer ..

                var result = dataHandler.CreateLeaves( leaves );
                if ( !result.Result )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreateLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, IdsString ), IdsString );
                    context.Response.Set( result );
                    return;
                }

                #endregion

                var state = EventsBroker.Handle( new LeaveCreateEvent( leaves, systemLevelAction ), EventMode.Sync );

                context.Response.Set( ResponseState.Success, true );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Leaves );

                #endregion

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<Leave> GetLeave( Guid personId, DateTime date )
        {
            var context = new ExecutionContext<Leave>();
            context.Execute( () =>
            {
                #region Cache

                var cacheKey = "LeavesHandler_GetLeave" + personId + date.ToShortDateString();
                var cached = Broker.Cache.Get<Leave>( TamamCacheClusters.Leaves, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion

                var criteria = new LeaveSearchCriteria( new List<Guid> { personId }, null, date, date, null, null, new List<int>() { ( int )LeaveStatus.Approved, ( int )LeaveStatus.Taken }, false, 0, 50 );
                var dataHandlerResponse = TamamServiceBroker.LeavesHandler.SearchLeaves( criteria, true, SystemRequestContext.Instance );
                if ( dataHandlerResponse.Type != ResponseState.Success )
                {
                    context.Response.Set( dataHandlerResponse.Type, null );
                    return;
                }

                var leave = dataHandlerResponse.Result.Leaves.FirstOrDefault();
                if ( leave == null )
                {
                    context.Response.Set( ResponseState.Success, null );
                    return;
                }

                context.Response.Set( ResponseState.Success, leave );

                #region Cache

                Broker.Cache.Add<Leave>( TamamCacheClusters.Leaves, cacheKey, leave );

                #endregion

            }, SystemRequestContext.Instance );

            return context.Response;
        }
        public ExecutionResponse<bool> EditLeave( Leave leave, bool systemLevelAction, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Validation : original version ...

                var originalLeaveVersion = dataHandler.GetLeave( leave.Id, SystemSecurityContext.Instance ).Result;
                if ( originalLeaveVersion == null )
                {
                    context.Response.Set( ResponseState.NotFound, false );
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditLeaveActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.EditLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                    return;
                }

                #endregion
                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError, false );
                    return;
                }

                // authorization ...
                bool isOwner = requestContext.PersonId.HasValue && originalLeaveVersion.PersonId == requestContext.PersonId;
                if ( !TamamServiceBroker.Authorize( requestContext, TamamConstants.AuthorizationConstants.EditLeaveActionKey ) && !isOwner )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditLeaveActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.EditLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                    context.Response.Set( ResponseState.AccessDenied, false );
                    return;
                }

                #endregion
                #region UpdateModel

                leave.IsNative = originalLeaveVersion.IsNative;

                #endregion
                #region Validation

                #region Basic

                var validationMode = systemLevelAction ? LeaveValidator.ValidationMode.SystemEdit : LeaveValidator.ValidationMode.Edit;
                var validator = new LeaveValidator( leave, validationMode );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditLeaveActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.EditLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                    return;
                }

                #endregion
                #region Leave Policies

                var person = GetPerson( leave.PersonId, requestContext );
                var policies = GetPolicies( person, requestContext );
                var leavePolicy = GetPoliciesLeaves( policies, ( LeaveTypes )leave.LeaveTypeId );
                if ( leavePolicy == null )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditLeaveActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.EditLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                    context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "LeaveTypeId", string.Format( ValidationResources.LeaveInvalidLeaveTypePolicy, person.GetLocalizedFullName() ) ) } );
                    return;
                }

                #endregion
                #region Credits

                SetEffectiveDaysAccordingToLeavePolicy( leave, leavePolicy );
                if ( !systemLevelAction )
                {


                    if ( leave.EffectiveDaysCount == 0 )
                    {
                        TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditLeaveActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.EditLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                        context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "StartDate", string.Format( ValidationResources.LeaveEffectiveDaysEqualZero, leave.Person.GetLocalizedFullName() ) ) } );
                        return;
                    }
                    int diff = leave.StartDate.Subtract( DateTime.Today ).Days;
                    // the requested leave is for today or future day
                    if ( diff >= 0 && leavePolicy.DisablePlannedLeaves.HasValue && leavePolicy.DisablePlannedLeaves.Value )
                    {
                        //if the leave policy disable the planned leave so the system will return error
                        TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditLeaveActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.EditLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                        context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "PersonId", ValidationResources.PlannedLeavesNotAllow ) } );
                        return;

                    }
                    // validate Days Limit for old dates..
                    if ( leavePolicy.DaysLimitForOldLeaves.HasValue && leavePolicy.DaysLimitForOldLeaves.Value > 0 )
                    {
                        var daysLimit = leavePolicy.DaysLimitForOldLeaves.Value;
                        var startDateLimit = leave.RequestTime.AddDays( daysLimit * -1 );
                        if ( leave.StartDate < startDateLimit )
                        {
                            var errorMessage = string.Format( ValidationResources.LeaveStartDateExceedsOldLeavesDaysLimit, person.GetLocalizedFullName() );
                            context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "StartDate", errorMessage ) } );
                            return;
                        }
                    }
                    var maxDaysPerRequest = leavePolicy.MaxDaysPerRequest.HasValue ? leavePolicy.MaxDaysPerRequest.Value : 0;
                    //0 value will disable the validation
                    if ( maxDaysPerRequest > 0 && leave.EffectiveDaysCount > maxDaysPerRequest )
                    {
                        context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "StartDate", string.Format( ValidationResources.LeaveEffectiveDayNotVaild, maxDaysPerRequest ) ) } );
                        return;
                    }

                    var LeaveCredit = GetCreditForLeave( leave.PersonId, leave, requestContext );
                    if ( LeaveCredit.Type == ResponseState.NotFound )
                    {
                        context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "PersonId", string.Format( ValidationResources.LeaveOutCredit, leave.Person.GetLocalizedFullName() ) ) } );
                        return;
                    }
                    var credit = LeaveCredit.Result;
                    var unlimitedCredit = leavePolicy.UnlimitedCredit.HasValue ? leavePolicy.UnlimitedCredit.Value : false;
                    if ( !unlimitedCredit )
                    {
                        var personCreditResult = GetCreditAmount( credit, leave, leavePolicy, requestContext );
                        if ( personCreditResult.Type == ResponseState.NotFound )
                        {
                            context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "PersonId", string.Format( ValidationResources.LeaveOutCredit, leave.Person.GetLocalizedFullName() ) ) } );
                            return;
                        }
                        var personCredit = personCreditResult.Result;
                        var EffectiveDaysCountDifference = leave.EffectiveDaysCount - originalLeaveVersion.EffectiveDaysCount;
                        if ( EffectiveDaysCountDifference > personCredit )
                        {
                            TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditLeaveActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.EditLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                            context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "PersonId", string.Format( ValidationResources.LeaveTypeHasNoCredit, leave.Person.GetLocalizedFullName() ) ) } );
                            return;
                        }
                    }
                    leave.EffectiveYearStart = credit.EffectiveYearStart;
                }

                #endregion

                #endregion
                # region Check Allow Request For half day leaves / Require Attachments..

                if ( systemLevelAction && leave.LeaveMode == LeaveMode.HalfDay )
                {
                    context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "LeaveTypeId", "Half Leaves are not supported in integration mode ..." ) } );
                    return;
                }
                else
                {
                    // not allowed to request half days ?
                    if ( leave.LeaveMode == LeaveMode.HalfDay && ( !leavePolicy.AllowHalfDays.HasValue || !leavePolicy.AllowHalfDays.Value ) )
                    {
                        TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreateLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                        context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "LeaveTypeId", string.Format( ValidationResources.LeaveHalfDayNotAllowed, person.GetLocalizedFullName() ) ) } );
                        return;
                    }

                    // check for Leave Attachments..
                    if ( leavePolicy.RequireAttachments.HasValue && leavePolicy.RequireAttachments.Value )
                    {
                        if ( leave.Attachments == null || leave.Attachments.Count == 0 )
                        {
                            TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreateLeaveAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                            var leaveTypes = Broker.DetailCodeHandler.GetDetailCodesByMasterCode( TamamConstants.MasterCodes.LeaveType );
                            var associatedType = leaveTypes.FirstOrDefault( x => x.Id == leave.LeaveTypeId );
                            var leaveTypeName = requestContext.CultureName.ToLower().Contains( "ar" ) ? associatedType.NameCultureVariant : associatedType.Name;

                            context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "LeaveTypeId", string.Format( ValidationResources.LeaveAttachmentRequired, leaveTypeName ) ) } );
                            return;
                        }
                    }
                }

                # endregion
                #region Data Layer

                var dataHandlerResponse = dataHandler.EditLeave( leave );
                if ( !dataHandlerResponse.Result )
                {
                    context.Response.Set( dataHandlerResponse );
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditLeaveActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.EditLeaveAuditMessageFailure, leave.Id ), leave.Id.ToString() );
                    return;
                }

                #endregion

                var state = EventsBroker.Handle( new LeaveEditEvent( leave, originalLeaveVersion, systemLevelAction ), EventMode.Sync );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Leaves );

                #endregion

                // audit ...
                TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditLeaveActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.EditLeaveAuditMessageSuccessful, leave.Id ), leave.Id.ToString() );

                context.Response.Set( ResponseState.Success, true );

                #endregion

            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> UpdateLeaveCredit( Guid personId, Leave leave, double daysAmount, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region validation ...

                //// check credit ...
                //var response = CheckLeaveTypeCredit(personId, leaveType, requestContext);
                //if (response.Type != ResponseState.Success)
                //{
                //    context.Response.Set(response.Type, false, response.Message, response.MessageDetailed);
                //    return;
                //}

                //var availableCredit = response.Result;

                //if ( daysAmount > availableCredit )
                //{
                //    context.Response.Set ( ResponseState.ValidationError , false , "Not Enough Credit" , null );
                //    return;
                //}

                //No Need to update DB in case the amount that need to be added in 0
                if ( daysAmount == 0 )
                {
                    context.Response.Set( ResponseState.Success, true, new List<ModelMetaPair>() );
                    return;
                }
                ////////////////////////////////////////////////////////////////////////////
                #endregion

                // models ...               
                var LeaveCreditResult = GetLeaveCreditForLeaveDates( personId, leave.StartDate, leave.EndDate, requestContext );
                if ( LeaveCreditResult.Type == ResponseState.NotFound )
                {
                    context.Response.Set( ResponseState.SystemError, false );
                    return;
                }
                var credit = LeaveCreditResult.Result;

                // check associated credit ...
                var leaveTypeCredit = credit.LeaveTypeCredits.FirstOrDefault( x => x.LeaveTypeId == leave.LeaveTypeId );
                if ( leaveTypeCredit == null )
                {
                    context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( string.Empty, Leaves.InvalidLeaveType ) } );
                    return;
                }

                // update value ...
                leaveTypeCredit.Consumed += daysAmount;


                // data layer ...
                var response_save = dataHandler.UpdateLeaveCredit( credit );
                if ( response_save.Type != ResponseState.Success )
                {
                    context.Response.Set( response_save );
                    return;
                }

                //Check if the leave taken from the Previous credit so i need to Recalculate Leave Credit to update the carry over for the current credit
                if ( credit.LeaveCreditStatusId == ( int )LeaveCreditStatus.Previous )
                    RecalculateLeaveCredit( personId, requestContext );
                context.Response.Set( ResponseState.Success, true );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Leaves );

                #endregion

                #endregion
            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> CreateExcuses( List<Excuse> excuses, bool systemLevelAction, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Input validation

                if ( excuses == null || !excuses.Any() )
                {
                    context.Response.Set( ResponseState.InvalidInput, false, new List<ModelMetaPair> { new ModelMetaPair( "PersonId", ValidationResources.PersonIdEmpty ) } );
                    return;
                }

                #endregion
                #region setup ...

                var Ids = excuses.Select( e => e.Id );
                var IdsString = string.Join( ", ", Ids );

                #endregion
                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError, false );
                    return;
                }

                // authorization ...
                var excuseType = excuses.Count > 0 ? excuses.First().ExcuseTypeId : 0;
                string target = TamamConstants.AuthorizationConstants.CreateExcuseActionKey;
                if ( excuseType != 0 )
                {
                    target = excuseType == ( int )ExcuseTypes.AwayExcuse ?
                        TamamConstants.AuthorizationConstants.CreateAwayActionKey :
                        TamamConstants.AuthorizationConstants.CreateExcuseActionKey;
                }
                else
                {
                    if ( !( requestContext is SystemRequestContext ) )
                    {
                        context.Response.Set( ResponseState.AccessDenied, false );
                        return;
                    }
                }
                if ( !TamamServiceBroker.Authorize( requestContext, target ) )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreateExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateExcuseAuditMessageFailure, IdsString ), IdsString );
                    context.Response.Set( ResponseState.AccessDenied, false );
                    return;
                }

                #endregion
                #region Validations

                #region Group Validation

                // validate : one excuse per person ...
                if ( excuses.GroupBy( e => e.PersonId ).Any( g => g.Count() > 1 ) )
                {
                    context.Response.Set( ResponseState.InvalidInput, false, new List<ModelMetaPair>() { new ModelMetaPair( "PersonId", ValidationResources.DuplicatePersonId ) } );
                    return;
                }

                #endregion

                var now = DateTime.Now;
                var validaionErrors = new List<ModelMetaPair>();

                foreach ( var excuse in excuses )
                {
                    #region Update Model

                    excuse.RequestTime = now;   // Set Request Time            
                    excuse.IsNative = !systemLevelAction;
                    excuse.Code = systemLevelAction ? excuse.Code : excuse.Id.ToString();
                    excuse.Person = this.GetPerson( excuse.PersonId, requestContext );

                    #endregion
                    #region Basic Validation

                    var validationMode = systemLevelAction ? ExcuseValidator.ExcuseValidationMode.SystemCreate : ExcuseValidator.ExcuseValidationMode.Create;
                    var validator = new ExcuseValidator( excuse, validationMode );
                    if ( validator.IsValid != null && !validator.IsValid.Value )
                    {
                        TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreateExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateExcuseAuditMessageFailure, excuse.Id.ToString() ), excuse.Id.ToString() );
                        validaionErrors.AddRange( validator.ErrorsDetailed );
                        continue;
                    }

                    #endregion
                    #region Advanced Validation

                    #region Duration check ...

                    var leaves = new SchedulesHandler().GetActiveLeaves( excuse.PersonId, excuse.ExcuseDate, excuse.ExcuseDate );
                    if ( leaves != null && leaves.Count > 0 )
                    {
                        TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreateExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateExcuseAuditMessageFailure, excuse.Id.ToString() ), excuse.Id.ToString() );
                        validaionErrors.Add( new ModelMetaPair( "PersonId", string.Format( ValidationResources.ExcuseForPersonConflictWithLeave, excuse.Person.GetLocalizedFullName() ) ) );
                        continue;
                    }

                    var duration = SystemBroker.SchedulesHandler.GetScheduledHoursCount( excuse.PersonId, excuse.ExcuseDate, excuse.StartTime.TimeOfDay, excuse.EndTime.TimeOfDay ).Result;
                    var isValidHours = duration != 0 && GetDuration(excuse) == duration;
                    if ( !isValidHours )
                    {
                        TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreateExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateExcuseAuditMessageFailure, excuse.Id.ToString() ), excuse.Id.ToString() );
                        validaionErrors.Add( new ModelMetaPair( "PersonId", string.Format( ValidationResources.ExcuseDurationForPersonNameEqualZero, excuse.Person.GetLocalizedFullName() ) ) );
                        continue;
                    }
                    excuse.Duration = Math.Round(duration, 2);
                    #endregion
                    #region Credit check ...

                    if ( !systemLevelAction )
                    {
                        // validate excuses credit
                        var response = CheckExcusesCredit( excuse.PersonId, ( ExcuseTypes )excuse.ExcuseTypeId, excuse.ExcuseDate, excuse.Duration, requestContext );
                        if ( !response.Result )
                        {
                            TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreateExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateExcuseAuditMessageFailure, excuse.Id.ToString() ), excuse.Id.ToString() );
                            validaionErrors.AddRange( response.MessageDetailed );
                            continue;
                        }
                    }

                    #endregion

                    #endregion
                    #region Update Model

                    excuse.Person = null; // need to clear the added person model, in order to avoid an issue with saving in the DataLayer (ORM sessions conflict)

                    #endregion
                }

                // return if having issues ...
                if ( validaionErrors.Any() )
                {
                    context.Response.Set( ResponseState.ValidationError, false, validaionErrors );
                    return;
                }

                #endregion
                #region Data Layer

                var dataHandlerResponse = dataHandler.CreateExcuses( excuses );
                if ( !dataHandlerResponse.Result )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreateExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateExcuseAuditMessageFailure, IdsString ), IdsString );
                    context.Response.Set( dataHandlerResponse );
                    return;
                }

                #endregion
                #region Audit

                TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.CreateExcuseAuditActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.CreateExcuseAuditMessageSuccessful, IdsString ), IdsString );

                #endregion

                var state = EventsBroker.Handle( new ExcuseCreateEvent( excuses, systemLevelAction ), EventMode.Sync );

                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Excuses );

                #endregion

                context.Response.Set( ResponseState.Success, true );

                #endregion

            }, requestContext );

            return context.Response;
        }
        public ExecutionResponse<bool> EditExcuse( Excuse excuse, bool systemLevelAction, RequestContext requestContext )
        {
            var context = new ExecutionContext<bool>();
            context.Execute( () =>
            {
                #region logic ...

                #region Validation : original version ...

                var originalExcuseVersion = dataHandler.GetExcuse( excuse.Id, SystemSecurityContext.Instance ).Result;

                if ( originalExcuseVersion == null )
                {
                    context.Response.Set( ResponseState.NotFound, false, new List<ModelMetaPair>() );
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditExcuseActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.EditExcuseAuditMessageFailure, excuse.Id ), excuse.Id.ToString() );
                    return;
                }

                #endregion
                #region Security

                // authentication ...
                if ( !TamamServiceBroker.Authenticate( requestContext ) )
                {
                    context.Response.Set( ResponseState.AuthenticationError, false, new List<ModelMetaPair>() );
                    return;
                }

                // authorization ...
                bool isOwner = requestContext.PersonId.HasValue && originalExcuseVersion.PersonId == requestContext.PersonId;
                var target = excuse.ExcuseTypeId == ( int )ExcuseTypes.AwayExcuse ? TamamConstants.AuthorizationConstants.EditAwayActionKey : TamamConstants.AuthorizationConstants.EditExcuseActionKey;
                if ( !TamamServiceBroker.Authorize( requestContext, target ) && !isOwner )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditExcuseActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.EditExcuseAuditMessageFailure, excuse.Id ), excuse.Id.ToString() );
                    context.Response.Set( ResponseState.AccessDenied, false, new List<ModelMetaPair>() );
                    return;
                }

                #endregion
                #region UpdateModel

                excuse.IsNative = originalExcuseVersion.IsNative;

                #endregion
                # region Validation

                // validation ...
                var validationMode = systemLevelAction ? ExcuseValidator.ExcuseValidationMode.SystemEdit : ExcuseValidator.ExcuseValidationMode.Edit;
                var validator = new ExcuseValidator( excuse, validationMode );
                if ( validator.IsValid != null && !validator.IsValid.Value )
                {
                    context.Response.Set( ResponseState.ValidationError, false, validator.ErrorsDetailed );
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditExcuseActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.EditExcuseAuditMessageFailure, excuse.Id ), excuse.Id.ToString() );
                    return;
                }

                # endregion
                #region Advanced Validation

                //  Validate Excuse Duration
                var duration = SystemBroker.SchedulesHandler.GetScheduledHoursCount( excuse.PersonId, excuse.ExcuseDate, excuse.StartTime.TimeOfDay, excuse.EndTime.TimeOfDay ).Result;
                var isValidHours = duration != 0 && GetDuration(excuse) == duration;
                if ( !isValidHours )
                {
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditExcuseActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.EditExcuseAuditMessageFailure, excuse.Id ), excuse.Id.ToString() );
                    context.Response.Set( ResponseState.ValidationError, false, new List<ModelMetaPair> { new ModelMetaPair( "PersonId", string.Format( ValidationResources.ExcuseDurationForPersonNameEqualZero, excuse.Person.GetLocalizedFullName() ) ) } );
                    return;
                }
                excuse.Duration = Math.Round(duration, 2);
                //excuse.Duration = (excuse.EndTime.TimeOfDay - excuse.StartTime.TimeOfDay).TotalHours;

                if ( !systemLevelAction )
                {
                    // validate excuses credit
                    var response = CheckExcusesCredit( originalExcuseVersion, excuse.ExcuseDate, excuse.Duration, requestContext );
                    if ( !response.Result )
                    {
                        TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditExcuseActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.EditExcuseAuditMessageFailure, excuse.Id ), excuse.Id.ToString() );
                        context.Response.Set( ResponseState.ValidationError, false, response.MessageDetailed );
                        return;
                    }
                }

                #endregion
                #region data layer

                var dataHandlerResponse = dataHandler.EditExcuse( excuse );
                if ( !dataHandlerResponse.Result )
                {
                    context.Response.Set( dataHandlerResponse );
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditExcuseActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.EditExcuseAuditMessageFailure, excuse.Id ), excuse.Id.ToString() );
                    return;
                }
                #endregion

                var state = EventsBroker.Handle( new ExcuseEditEvent( excuse, systemLevelAction ), EventMode.Sync );

                #region Audit

                // audit ...
                TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.EditExcuseActionId, TamamConstants.AuthorizationConstants.ExcuseAuditModuleId, string.Format( TamamConstants.AuthorizationConstants.EditExcuseAuditMessageSuccessful, excuse.Id ), excuse.Id.ToString() );

                #endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Excuses );

                #endregion

                context.Response.Set( ResponseState.Success, true );

                #endregion
            }, requestContext );

            return context.Response;
        }

        #endregion
        #region IReadOnlyLeavesHandler

        public ExecutionResponse<List<LeaveDTO>> GetCompositeLeave( LeaveSearchCriteria criteria, bool activePersonsOnly, RequestContext requestContext )
        {
            var context = new ExecutionContext<List<LeaveDTO>>();
            context.Execute( () =>
            {
                #region logic ...

                #region Cache

                var cacheKey = "LeavesHandler_GetCompositeLeave" + criteria + activePersonsOnly + requestContext;
                var cached = Broker.Cache.Get<List<LeaveDTO>>( TamamCacheClusters.Leaves, cacheKey );
                if ( cached != null )
                {
                    context.Response.Set( ResponseState.Success, cached );
                    return;
                }

                #endregion
                #region Security

                context.ActionContext = new ActionContext(
                    moduleId: TamamConstants.AuthorizationConstants.LeaveAuditModuleId,
                    actionId: TamamConstants.AuthorizationConstants.SearchLeavesAuditActionId,
                    actionKey: TamamConstants.AuthorizationConstants.SearchLeavesActionKey,
                    messageForDenied: TamamConstants.AuthorizationConstants.SearchLeavesActionKey,
                    messageForFailure: TamamConstants.AuthorizationConstants.SearchLeavesAuditMessageFailure,
                    messageForSuccess: TamamConstants.AuthorizationConstants.SearchLeavesAuditMessageSuccessful
                    );

                var response = TamamServiceBroker.Secure( context.ActionContext, requestContext );
                if ( response.Type != ResponseState.Success )
                {
                    context.Response.Set( response.Type, null );
                    return;
                }

                var securityContext = response.Result;

                #endregion

                // data layer ...
                var dataHandlerResponse = dataHandler.GetCompositeLeave( criteria, activePersonsOnly, securityContext );
                if ( dataHandlerResponse.Result == null )
                {
                    context.Response.Set( dataHandlerResponse );
                    TamamServiceBroker.Audit( requestContext, TamamConstants.AuthorizationConstants.SearchLeavesAuditActionId, TamamConstants.AuthorizationConstants.LeaveAuditModuleId, TamamConstants.AuthorizationConstants.SearchLeavesAuditMessageFailure, string.Empty );
                    return;
                }

                context.Response.Set( dataHandlerResponse );
                #region Cache

                Broker.Cache.Add<List<LeaveDTO>>( TamamCacheClusters.Leaves, cacheKey, dataHandlerResponse.Result );

                #endregion
                #endregion
            }, requestContext );

            return context.Response;
        }

        #endregion
    }
}