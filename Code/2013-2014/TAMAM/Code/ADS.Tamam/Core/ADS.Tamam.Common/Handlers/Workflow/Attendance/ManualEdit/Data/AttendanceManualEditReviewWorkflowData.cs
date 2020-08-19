using System.Runtime.Serialization;
using ADS.Common.Workflow.Models;

namespace ADS.Tamam.Common.Handlers.Workflow.Attendance.ManualEdit.Data
{
    [DataContract( IsReference = true )]
    public class AttendanceManualEditReviewWorkflowData : WorkflowBaseData
    {
        [DataMember] public string TargetId { get; set; }
        [DataMember] public string TargetStatus { get; set; }
        [DataMember] public string ApproverIdExpected { get; set; }

        #region cst ...

        public AttendanceManualEditReviewWorkflowData() { }
        public AttendanceManualEditReviewWorkflowData( string targetId ) : this()
        {
            TargetId = targetId;
        }
        public AttendanceManualEditReviewWorkflowData( string targetId , string approverId , string command , string metadata ) : this( targetId )
        {
            PersonId = approverId;
            Command = command;
            Metadata = metadata;
        }

        #endregion
    }
}