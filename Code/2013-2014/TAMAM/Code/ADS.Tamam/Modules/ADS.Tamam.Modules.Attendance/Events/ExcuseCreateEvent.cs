using System;
using System.Collections.Generic;
using ADS.Common.Bases.Events.Models;
using ADS.Common.Context;
using ADS.Common.Contracts;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Leaves;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers;
using ADS.Common.Handlers;
using ADS.Tamam.Common.Workflow.Leaves.Review.Definitions;

namespace ADS.Tamam.Modules.Attendance.Events
{
    [Serializable]
    public class ExcuseCreateEvent : EventCell, IXIncludable
    {
        #region Properties

        public List<Excuse> Excuses { get; set; }
        public bool SystemLevelAction { get; set; }
        private readonly LeaveApprovalWorkflowDefinition approvalWorkflowDefinition = new LeaveApprovalWorkflowDefinition();

        #endregion
        # region cst..

        public ExcuseCreateEvent() : base() { }
        public ExcuseCreateEvent( List<Excuse> excuses , bool systemLevelAction ) : this()
        {
            this.Excuses = excuses;
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
                
                foreach ( var excuse in Excuses )
                {
                    if ( SystemLevelAction && excuse.ExcuseStatusId == ( int )ExcuseStatus.Approved )
                    {
                        var request = TamamServiceBroker.AttendanceHandler.HandleExcuse( excuse, SystemRequestContext.Instance );
                        if ( request.Type != ResponseState.Success )
                        {
                            XLogger.Error( "error updating excuse related attendance times" );
                            continue;
                        }
                    }
                    else if ( excuse.ExcuseStatusId == ( int )ExcuseStatus.Pending )
                    {
                        if ( !Broker.WorkflowEngine.Initialize( excuse, approvalWorkflowDefinition ) ) return false;
                    }
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
            return Excuses != null;
        }

        #endregion
    }
}