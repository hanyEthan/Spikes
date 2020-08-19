using System;
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
using System.Collections.Generic;

namespace ADS.Tamam.Modules.Attendance.Events
{
    [Serializable]
    public class ExcuseEditEvent : EventCell, IXIncludable
    {
        #region Properties

        public Excuse Excuse { get; set; }
        public bool SystemLevelAction { get; set; }
        private readonly LeaveApprovalWorkflowDefinition approvalWorkflowDefinition = new LeaveApprovalWorkflowDefinition();

        #endregion
        # region cst..

        public ExcuseEditEvent() : base() { }
        public ExcuseEditEvent( Excuse excuse , bool systemLevelAction ) : this()
        {
            this.Excuse = excuse;
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
            get { return typeof( Excuse ).ToString(); }
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
                
                if ( SystemLevelAction && Excuse.ExcuseStatusId == ( int )ExcuseStatus.Approved )
                {
                    var response = TamamServiceBroker.AttendanceHandler.HandleExcuse( Excuse, SystemRequestContext.Instance );
                    if ( response.Type != ResponseState.Success ) XLogger.Error( "error updating excuse related attendance times" );
                }

                if ( Excuse.ExcuseStatusId == ( int )ExcuseStatus.Pending )
                {
                    if ( !Broker.WorkflowEngine.Reset( Excuse, approvalWorkflowDefinition ) ) return false;
                }

                #endregion
                # region Cache

                Broker.Cache.Invalidate( TamamCacheClusters.Excuses );

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
            return Excuse != null;
        }

        #endregion
    }
}