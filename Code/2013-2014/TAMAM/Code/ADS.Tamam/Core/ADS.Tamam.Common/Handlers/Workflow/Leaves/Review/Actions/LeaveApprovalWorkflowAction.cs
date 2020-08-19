using System;
using System.Runtime.Serialization;
using ADS.Common.Context;
using ADS.Common.Handlers;
using ADS.Common.Models.Domain;
using ADS.Common.Utilities;
using ADS.Common.Workflow.Models;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Handlers.Workflow.Leaves.Review.Models;
using ADS.Tamam.Common.Workflow.Leaves.Review.Data;

namespace ADS.Tamam.Common.Workflow.Leaves.Review.Actions
{
    [DataContract( IsReference = true )]
    public class LeaveApprovalWorkflowAction : WorkflowActionBase
    {
        public override bool Process( WorkflowBaseData data )
        {
            try
            {
                var dataLocal = PrepareData();

                switch ( dataLocal.TargetType )
                {
                    case WorkflowLeaveTargetType.Leave:
                        {
                            var leave = GetLeave( new Guid( dataLocal.TargetId ) );
                            var previousStatus = ( LeaveStatus ) leave.LeaveStatusId;
                            var newStatus = GetUpdatedStatusForLeaves( dataLocal );

                            if ( IsStatusValidForApproval( previousStatus , newStatus ) )
                            {
                                leave.LeaveStatusId = ( int ) newStatus;

                                UpdateTarget( leave );
                                UpdateLeaveCredit( leave , previousStatus );

                                #region Cache

                                Broker.Cache.Invalidate( TamamCacheClusters.Leaves );

                                #endregion

                                if ( newStatus == LeaveStatus.Approved || newStatus == LeaveStatus.Cancelled )
                                {
                                    UpdateAttendance( leave );
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        break;
                    case WorkflowLeaveTargetType.Away:
                    case WorkflowLeaveTargetType.Excuse:
                        {
                            var excuse = GetExcuse( new Guid( dataLocal.TargetId ) );
                            var previousStatus = ( ExcuseStatus ) excuse.ExcuseStatusId;
                            var newStatus = GetUpdatedStatusForExcuses( dataLocal );

                            if ( IsStatusValidForApproval( previousStatus , newStatus ) )
                            {
                                excuse.ExcuseStatusId = ( int ) newStatus;

                                UpdateTarget( excuse );

                                #region Cache

                                Broker.Cache.Invalidate( TamamCacheClusters.Excuses );
                                Broker.Cache.Invalidate( TamamCacheClusters.Leaves );   // because approval steps are cached in this cluster ...

                                #endregion

                                if ( newStatus == ExcuseStatus.Approved || newStatus == ExcuseStatus.Cancelled )
                                {
                                    UpdateAttendance( excuse );
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        break;
                    default: return false;
                }

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        #region Helpers

        private LeaveReviewWorkflowData PrepareData()
        {
            var data = this.Data as LeaveReviewWorkflowData;
            if ( data == null || string.IsNullOrWhiteSpace( data.TargetId ) || string.IsNullOrWhiteSpace( data.TargetStatus ) ) throw new Exception( "invalid action data" );

            return data;
        }

        private bool IsStatusValidForApproval( LeaveStatus old , LeaveStatus neo )
        {
            //if (old == LeaveStatus.Denied) return false;
            //if (old == LeaveStatus.Taken && neo != LeaveStatus.Cancelled) return false;
            //if (old == LeaveStatus.Approved && neo != LeaveStatus.Cancelled) return false;

            //return true;

            return ( old != LeaveStatus.Approved || neo == LeaveStatus.Cancelled )   // can't edit in an approved target, unless the new status is cancel.
                && ( old != LeaveStatus.Taken || neo == LeaveStatus.Cancelled )     // can't edit in a taken target, unless the new status is cancel.
                && old != LeaveStatus.Denied;
        }
        private bool IsStatusValidForApproval( ExcuseStatus old , ExcuseStatus neo )
        {
            return (old != ExcuseStatus.Approved || neo == ExcuseStatus.Cancelled)   // can't edit in an approved target, unless the new status is cancel.
                && (old != ExcuseStatus.Taken || neo == ExcuseStatus.Cancelled)     // can't edit in an approved target, unless the new status is cancel.
                && old != ExcuseStatus.Denied;
        }

        private LeaveStatus GetUpdatedStatusForLeaves( LeaveReviewWorkflowData data )
        {
            WorkflowLeaveReviewStatus command;
            if ( !Enum.TryParse<WorkflowLeaveReviewStatus>( data.TargetStatus , true , out command ) ) throw new Exception( "invalid command for target status" );

            switch ( command )
            {
                case WorkflowLeaveReviewStatus.Approved: return LeaveStatus.Approved;
                case WorkflowLeaveReviewStatus.Denied: return LeaveStatus.Denied;
                case WorkflowLeaveReviewStatus.Cancelled: return LeaveStatus.Cancelled;
                case WorkflowLeaveReviewStatus.Pending:
                default: return LeaveStatus.Pending;
            }
        }
        private ExcuseStatus GetUpdatedStatusForExcuses( LeaveReviewWorkflowData data )
        {
            WorkflowLeaveReviewStatus command;
            if ( !Enum.TryParse<WorkflowLeaveReviewStatus>( data.TargetStatus , true , out command ) ) throw new Exception( "invalid command for target status" );

            switch ( command )
            {
                case WorkflowLeaveReviewStatus.Approved: return ExcuseStatus.Approved;
                case WorkflowLeaveReviewStatus.Denied: return ExcuseStatus.Denied;
                case WorkflowLeaveReviewStatus.Cancelled: return ExcuseStatus.Cancelled;
                case WorkflowLeaveReviewStatus.Pending:
                default: return ExcuseStatus.Pending;
            }
        }

        private Leave GetLeave( Guid id )
        {
            var response = TamamServiceBroker.LeavesHandler.GetLeave( id , SystemRequestContext.Instance );
            if ( response.Type != ResponseState.Success ) throw new Exception( "error getting target model" );
            return response.Result;
        }
        private Excuse GetExcuse( Guid id )
        {
            var response = TamamServiceBroker.LeavesHandler.GetExcuse( id , SystemRequestContext.Instance );
            if ( response.Type != ResponseState.Success ) throw new Exception( "error getting target model" );
            return response.Result;
        }

        private void UpdateTarget( Leave leave )
        {
            #region TEMP

            // TEMP : this measure is to handle a conflict between Cache and ORM, because ORM is ignoring the passed id, and is considering the instance if existed ...
            var leaveStatusId = leave.LeaveStatusId;
            leave.LeaveStatus = XModel.Clone<DetailCode>( Broker.DetailCodeHandler.GetDetailCode( leave.LeaveStatusId ) );

            #endregion

            var dataHandler = new LeavesDataHandler();
            var request = dataHandler.EditLeave( leave );
            if ( request.Type != ResponseState.Success ) throw new Exception( "error updating target model" );

            // TEMP : Hack ...
            leave.LeaveStatusId = leaveStatusId;
        }
        private void UpdateTarget( Excuse excuse )
        {
            #region TEMP

            // TEMP : this measure is to handle a conflict between Cache and ORM, because ORM is ignoring the passed id, and is considering the instance if existed ...
            var excuseStatusId = excuse.ExcuseStatusId;
            excuse.ExcuseStatus = XModel.Clone<DetailCode>(Broker.DetailCodeHandler.GetDetailCode(excuse.ExcuseStatusId));
            
            #endregion

            var dataHandler = new LeavesDataHandler();
            var request = dataHandler.EditExcuse( excuse );
            if ( request.Type != ResponseState.Success ) throw new Exception( "error updating target model" );

            // TEMP : Hack ...
            excuse.ExcuseStatusId = excuseStatusId;
        }

        private void UpdateAttendance( Leave leave )
        {
            var request = TamamServiceBroker.AttendanceHandler.HandleLeave( leave , SystemRequestContext.Instance );
            if ( request.Type != ResponseState.Success ) throw new Exception( "error updating target attendance" );
        }
        private void UpdateAttendance( Excuse excuse )
        {
            var request = TamamServiceBroker.AttendanceHandler.HandleExcuse( excuse , SystemRequestContext.Instance );
            if ( request.Type != ResponseState.Success ) throw new Exception( "error updating target attendance" );
        }

        private void UpdateLeaveCredit( Leave leave , LeaveStatus previousStatus )
        {
            var request = TamamServiceBroker.LeavesHandler.UpdateLeaveCredit( leave , previousStatus , SystemRequestContext.Instance );
            if ( request.Type != ResponseState.Success ) throw new Exception( "error updating target credit" );
        }

        #endregion
    }
}
