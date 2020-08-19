using System;
using System.Runtime.Serialization;
using ADS.Common.Workflow.Models;
using ADS.Common.Utilities;

namespace ADS.Tamam.Common.Handlers.Workflow.Attendance.ManualEdit.Actions
{
    [DataContract( IsReference = true )]
    public class CancellationWorkflowAction : WorkflowActionBase
    {
        public override bool Process( WorkflowBaseData data )
        {
            try
            {
                if ( Step == null ) return false;

                bool success = UpdateAttendanceRawData( this.Step.Instance.TargetId );

                return success;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        private bool UpdateAttendanceRawData( Guid id )
        {
            // 1. Remove the new edited value mark AttRawData as (NotSet or Approved)..     Need some discussion..
            // 2. Mark associated SE as Dirty..


            //var dataHandler = new AttendanceDataHandler();

            //var SE_Response = dataHandler.GetScheduleEvent( id );
            //if ( SE_Response.Type != ResponseState.Success ) return false;

            //var SE = SE_Response.Result;
            //SE.JustificationStatus = JustificationStatus.InProgress;

            //var Update_Response = dataHandler.UpdateScheduleEvent( SE );
            //return Update_Response.Type == ResponseState.Success;

            return true;
        }
    }
}