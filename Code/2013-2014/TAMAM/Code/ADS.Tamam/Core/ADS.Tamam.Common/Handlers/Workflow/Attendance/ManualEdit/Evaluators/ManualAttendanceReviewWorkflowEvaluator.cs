using System.Runtime.Serialization;
using ADS.Common.Workflow.Contracts;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.ManualEdit.Models;

namespace ADS.Tamam.Common.Handlers.Workflow.Attendance.ManualEdit.Evaluators
{
    [DataContract( IsReference = true )]
    public class ManualAttendanceReviewWorkflowEvaluator : WorkflowDataEvaluatorBase
    {
        public override string Evaluate( ADS.Common.Workflow.Models.WorkflowBaseData data )
        {
            if ( data == null ) return "";
            if ( data.Command.ToLower() == WorkflowAttendanceManualEditReviewStatus.Approved.ToString().ToLower() ) return WorkflowAttendanceManualEditReviewStatus.Approved.ToString().ToLower();
            if ( data.Command.ToLower() == WorkflowAttendanceManualEditReviewStatus.Denied.ToString().ToLower() ) return WorkflowAttendanceManualEditReviewStatus.Denied.ToString().ToLower();
            return "";
        }
    }
}