using System;
using System.Runtime.Serialization;
using ADS.Common.Context;
using ADS.Common.Utilities;
using ADS.Common.Workflow.Models;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers.Workflow.Leaves.Review.Data;
using ADS.Tamam.Common.Handlers.Workflow.Leaves.Review.Models;
using ADS.Tamam.Common.Workflow.Leaves.Review.Data;

namespace ADS.Tamam.Common.Handlers.Workflow.Leaves.Review.Actions
{
    [DataContract( IsReference = true )]
    public class LeaveReviewIterationJustificationWorkflowAction : WorkflowActionBase
    {
        public override bool Process( WorkflowBaseData data )
        {
            try
            {
                if ( Step == null ) return false;

                var dataLocal = this.Data as LeaveReviewIterationJustificationWorkflowData;
                var dataPassed = data as LeaveReviewWorkflowData;

                if ( dataLocal == null || dataPassed == null ) return false;

                if ( !IsValidJustification( dataPassed ) ) return false;

                dataLocal.Command = dataPassed.Command;
                dataLocal.PersonId = dataPassed.PersonId;
                dataLocal.Metadata = dataPassed.Metadata;

                Step.Data = dataLocal;   // data is valid ...

                this.Step.CheckpointPartial( data );

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }


        # region internals

        private bool IsValidJustification( LeaveReviewWorkflowData data )
        {
            switch ( data.TargetType )
            {
                case WorkflowLeaveTargetType.Excuse:
                case WorkflowLeaveTargetType.Away:

                    var excuse = GetExcuse( new Guid( data.TargetId ) );
                    return excuse.ExcuseStatusId == ( int ) ExcuseStatus.Pending && excuse.PersonId.ToString() == data.PersonId;

                case WorkflowLeaveTargetType.Leave:
                default:

                    var leave = GetLeave( new Guid( data.TargetId ) );
                    return ( leave.LeaveStatusId == ( int ) LeaveStatus.Planned || leave.LeaveStatusId == ( int ) LeaveStatus.Pending ) && leave.PersonId.ToString() == data.PersonId;
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

        # endregion
    }
}