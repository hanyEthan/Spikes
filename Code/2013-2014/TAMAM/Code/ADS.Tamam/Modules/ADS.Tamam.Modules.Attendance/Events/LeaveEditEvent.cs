using System;
using ADS.Common.Bases.Events.Models;
using ADS.Common.Contracts;
using ADS.Common.Handlers;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers;
using ADS.Common.Context;
using ADS.Tamam.Common.Workflow.Leaves.Review.Definitions;
using System.Collections.Generic;

namespace ADS.Tamam.Modules.Attendance.Events
{
    [Serializable]
    public class LeaveEditEvent : EventCell, IXIncludable
    {
        # region Properties

        public Leave UpdatedLeave { get; set; }
        public Leave OriginalLeave { get; set; }
        public bool SystemLevelAction { get; set; }
        private readonly LeaveApprovalWorkflowDefinition approvalWorkflowDefinition = new LeaveApprovalWorkflowDefinition();

        # endregion
        # region cst..

        public LeaveEditEvent() : base() { }
        public LeaveEditEvent( Leave updatedLeave , Leave originalLeave , bool systemLevelAction )
            : this()
        {
            this.UpdatedLeave = updatedLeave;
            this.OriginalLeave = originalLeave;
            this.SystemLevelAction = systemLevelAction;
        }

        # endregion
        #region EventCell

        [XDontSerialize] public override string ContentType
        {
            get { return this.GetType().AssemblyQualifiedName; }
        }
        [XDontSerialize] public override string TargetId
        {
            get { return ""; }
        }
        [XDontSerialize] public override string TargetType
        {
            get { return typeof( Leave ).ToString(); }
        }

        #endregion

        public override bool Process()
        {
            try
            {
                # region Prep

                if ( !ValidateData() ) return false;

                # endregion
                #region Logic

                if ( UpdatedLeave.LeaveStatusId == ( int )LeaveStatus.Pending || UpdatedLeave.LeaveStatusId == ( int )LeaveStatus.Planned || SystemLevelAction )
                {
                    // check if leave type has changed ...
                    if ( UpdatedLeave.LeaveTypeId == OriginalLeave.LeaveTypeId )
                    {
                        var delta = UpdatedLeave.EffectiveDaysCount - OriginalLeave.EffectiveDaysCount;
                        SystemBroker.LeavesHandler.UpdateLeaveCredit( UpdatedLeave.PersonId, UpdatedLeave, delta, SystemRequestContext.Instance );
                    }
                    else
                    {
                        SystemBroker.LeavesHandler.UpdateLeaveCredit( UpdatedLeave.PersonId, OriginalLeave, -1 * OriginalLeave.EffectiveDaysCount, SystemRequestContext.Instance );
                        SystemBroker.LeavesHandler.UpdateLeaveCredit( UpdatedLeave.PersonId, UpdatedLeave, UpdatedLeave.EffectiveDaysCount, SystemRequestContext.Instance );
                    }

                    if ( SystemLevelAction )
                    {
                        var startDate = UpdatedLeave.StartDate <= OriginalLeave.StartDate ?UpdatedLeave.StartDate: OriginalLeave.StartDate;
                        var endDate = UpdatedLeave.EndDate >= OriginalLeave.EndDate ? UpdatedLeave.EndDate :OriginalLeave.EndDate ;
                        var response = TamamServiceBroker.AttendanceHandler.HandleAttendanceDuration( startDate,endDate,UpdatedLeave.PersonId, SystemRequestContext.Instance );
                       
                        if ( response.Type != ResponseState.Success ) XLogger.Error( "error updating leave related attendance times" );
                    }
                    else
                    {
                        // workflow : reset ...
                        if ( !Broker.WorkflowEngine.Reset( UpdatedLeave, approvalWorkflowDefinition ) ) return false;
                    }
                } 

                #endregion
                # region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Leaves );

                # endregion

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        #region Helpers

        private bool ValidateData()
        {
            return UpdatedLeave != null && OriginalLeave != null;
        }

        #endregion
    }
}