using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ADS.Common.Context;
using ADS.Common.Utilities;
using ADS.Common.Workflow.Models;
using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.Violations.Data;

namespace ADS.Tamam.Common.Handlers.Workflow.Attendance.Violations.Actions
{
    [DataContract( IsReference = true )]
    public class AttendanceFinalizationWorkflowAction : WorkflowActionBase
    {
        public override bool Process( WorkflowBaseData data )
        {
            try
            {
                if ( Step == null ) return false;

                var dataLocal = this.Data as AttendanceFinalizationWorkflowData;
                var dataPassed = data as AttendanceFinalizationWorkflowData;
                var effectiveData = dataLocal ?? dataPassed;

                if ( effectiveData == null ) return false;

                bool success = UpdateScheduleEvent( this.Step.Instance.TargetId , effectiveData.Justified );

                return success;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        private bool UpdateScheduleEvent( Guid id , bool status )
        {
            var dataHandler = new AttendanceDataHandler();

            var SE_Response = dataHandler.GetScheduleEvent( id );
            if ( SE_Response.Type != ResponseState.Success ) return false;

            var SE = SE_Response.Result;
            SE.JustificationStatus = status ? JustificationStatus.Justified : JustificationStatus.Unjustified;

            var Update_Response = dataHandler.UpdateScheduleEvent( SE );
            return Update_Response.Type == ResponseState.Success;
        }
    }
}
