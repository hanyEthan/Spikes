using System;
using System.Runtime.Serialization;
using ADS.Common.Context;
using ADS.Common.Utilities;
using ADS.Common.Workflow.Models;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Handlers;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.Violations.Data;

namespace ADS.Tamam.Common.Handlers.Workflow.Attendance.Violations.Actions
{
    [DataContract( IsReference = true )]
    public class AttendanceJustificationWorkflowAction : WorkflowActionBase
    {
        public override bool Process( WorkflowBaseData data )
        {
            try
            {
                if ( Step == null ) return false;

                var dataLocal = this.Data as AttendanceJustificationWorkflowData;
                var dataPassed = data as AttendanceJustificationWorkflowData;

                if ( dataLocal == null || dataPassed == null ) return false;

                // check if the justifier is the expected person ...
                var canActForResponse = TamamServiceBroker.PersonnelHandler.CanActFor( new Guid( dataPassed.PersonId ) , new Guid( dataLocal.PersonId ) , SystemRequestContext.Instance );
                if ( canActForResponse.Type != ResponseState.Success ) return false;
                if ( !canActForResponse.Result ) return false;

                dataLocal.Command = dataPassed.Command;
                dataLocal.PersonId = dataPassed.PersonId;
                dataLocal.Metadata = dataPassed.Metadata;

                Step.Data = dataLocal;   // data is valid ...

                bool state = false;

                state = Step.Instance.PersonId.ToString() == dataLocal.PersonId
                    ? UpdateScheduleEvent( this.Step.Instance.TargetId , dataPassed.Metadata , null )
                    : UpdateScheduleEvent( this.Step.Instance.TargetId , null , dataPassed.Metadata );

                return state;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        private bool UpdateScheduleEvent( Guid id , string staffComment , string managerComment )
        {
            var dataHandler = new AttendanceDataHandler();

            var SE_Response = dataHandler.GetScheduleEvent( id );
            if ( SE_Response.Type != ResponseState.Success ) return false;

            var SE = SE_Response.Result;

            if ( !string.IsNullOrWhiteSpace( staffComment ) ) SE.StaffComments = staffComment;
            if ( !string.IsNullOrWhiteSpace( managerComment ) ) SE.ManagerComments = managerComment;

            var Update_Response = dataHandler.UpdateScheduleEvent( SE );
            return Update_Response.Type == ResponseState.Success;
        }
    }
}
