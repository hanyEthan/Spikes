using System;
using System.Collections.Generic;
using ADS.Common.Bases.Events.Models;
using ADS.Common.Contracts;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers;
using ADS.Common.Context;
using ADS.Common.Handlers;
using ADS.Tamam.Common.Workflow.Leaves.Review.Definitions;

namespace ADS.Tamam.Modules.Attendance.Events
{
    [Serializable]
    public class LeaveCreateEvent : EventCell, IXIncludable
    {
        #region Properties

        public List<Leave> Leaves { get; set; }
        public bool SystemLevelAction { get; set; }
        private readonly LeaveApprovalWorkflowDefinition approvalWorkflowDefinition = new LeaveApprovalWorkflowDefinition();

        #endregion
        # region cst..

        public LeaveCreateEvent() : base() { }
        public LeaveCreateEvent( List<Leave> leaves, bool systemLevelAction ) : this()
        {
            this.Leaves = leaves;
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

                foreach ( var leave in Leaves )
                {
                    #region Workflow..

                    if ( SystemLevelAction && leave.LeaveStatusId == ( int )LeaveStatus.Approved )
                    {
                        SystemBroker.LeavesHandler.UpdateLeaveCredit( leave.PersonId, leave, leave.EffectiveDaysCount, SystemRequestContext.Instance );

                        // internal integration related events ...
                        var response = TamamServiceBroker.AttendanceHandler.HandleLeave( leave, SystemRequestContext.Instance );
                        if ( response.Type != ResponseState.Success )
                        {
                            XLogger.Error( "error updating leave related attendance times" );
                            continue;
                        }
                    }
                    else if ( leave.LeaveStatusId == ( int )LeaveStatus.Pending || leave.LeaveStatusId == ( int )LeaveStatus.Planned )
                    {
                        SystemBroker.LeavesHandler.UpdateLeaveCredit( leave.PersonId, leave, leave.EffectiveDaysCount, SystemRequestContext.Instance );

                        if ( !Broker.WorkflowEngine.Initialize( leave, approvalWorkflowDefinition ) ) return false;
                    }

                    #endregion
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
            return Leaves != null;
        }

        #endregion
    }
}