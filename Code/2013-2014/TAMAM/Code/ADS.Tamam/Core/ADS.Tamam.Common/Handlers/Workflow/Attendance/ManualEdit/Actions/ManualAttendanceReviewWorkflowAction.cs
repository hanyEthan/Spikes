using System;
using System.Runtime.Serialization;
using ADS.Common.Utilities;
using ADS.Common.Workflow.Models;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.ManualEdit.Data;
using ADS.Tamam.Common.Data.Context;
using ADS.Common.Context;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.ManualEdit.Models;

namespace ADS.Tamam.Common.Handlers.Workflow.Attendance.ManualEdit.Actions
{
    [DataContract( IsReference = true )]
    public class ManualAttendanceReviewWorkflowAction : WorkflowActionBase
    {
        public override bool Process( WorkflowBaseData data )
        {
            try
            {
                if ( Step == null ) return false;

                var dataLocal = this.Data as AttendanceManualEditReviewWorkflowData;
                var dataPassed = data as AttendanceManualEditReviewWorkflowData;

                dataPassed = dataPassed ?? dataLocal;   // if nothing's passed, use locally stored data ...

                if ( dataLocal == null || dataPassed == null ) return false;

                // check if the reviewer is the expected one or can act for ...
                var canActForResponse = TamamServiceBroker.PersonnelHandler.CanActFor( new Guid( dataPassed.PersonId ) , new Guid( dataLocal.ApproverIdExpected ) , SystemRequestContext.Instance );
                if ( canActForResponse.Type != ResponseState.Success ) return false;
                if ( !canActForResponse.Result ) return false;

                WorkflowAttendanceManualEditReviewStatus command;
                if ( !Enum.TryParse<WorkflowAttendanceManualEditReviewStatus>( dataPassed.Command , true , out command ) ) return false;   // check if the command is valid ...

                dataLocal.Command = dataPassed.Command;
                dataLocal.TargetStatus = dataPassed.Command;
                dataLocal.PersonId = dataPassed.PersonId;
                dataLocal.Metadata = dataPassed.Metadata;

                Step.Data = dataLocal;   // data is valid ...
                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
    }
}