using System;
using ADS.Common.Bases.Events.Models;
using ADS.Common.Contracts;
using ADS.Common.Handlers;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Workflow.Leaves.Review.Definitions;
using ADS.Tamam.Common.Data.Model.Enums;
using System.Collections.Generic;

namespace ADS.Tamam.Modules.Attendance.Events
{
    [Serializable]
    public class LeaveRequestEvent : EventCell, IXIncludable
    {
        #region Properties

        public Leave Leave { get; set; }
        private readonly LeaveApprovalWorkflowDefinition approvalWorkflowDefinition = new LeaveApprovalWorkflowDefinition();

        #endregion
        # region cst..

        public LeaveRequestEvent() : base() { }
        public LeaveRequestEvent( Leave leave ) : this()
        {
            this.Leave = leave;
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

                if ( Leave.LeaveStatusId == ( int )LeaveStatus.Pending || Leave.LeaveStatusId == ( int )LeaveStatus.Planned )
                {
                    SystemBroker.LeavesHandler.UpdateLeaveCredit( Leave.PersonId, Leave, Leave.EffectiveDaysCount, SystemRequestContext.Instance );
                    if ( !Broker.WorkflowEngine.Initialize( Leave, approvalWorkflowDefinition ) ) return false;
                }
 
                #endregion
                #region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Leaves );

                #endregion

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
            return this.Leave != null;
        }

        #endregion
    }
}