using System.Runtime.Serialization;
using ADS.Common.Workflow.Models;
using ADS.Tamam.Common.Handlers.Workflow.Leaves.Review.Models;

namespace ADS.Tamam.Common.Workflow.Leaves.Review.Data
{
    [DataContract( IsReference = true )]
    public class LeaveReviewWorkflowData : WorkflowBaseData
    {
        [DataMember] public string TargetId { get; set; }
        [DataMember] public string TargetStatus { get; set; }
        [DataMember] public WorkflowLeaveTargetType TargetType { get; set; }
        [DataMember] public string ApproverIdExpected { get; set; }

        #region cst ...

        public LeaveReviewWorkflowData() { }
        public LeaveReviewWorkflowData( string targetId ) : this()
        {
            TargetId = targetId;
        }
        public LeaveReviewWorkflowData( string targetId , string approverId , string command , string metadata , WorkflowLeaveTargetType targetType ) : this( targetId )
        {
            PersonId = approverId;
            Command = command;
            Metadata = metadata;
            TargetType = targetType;
        }

        #endregion
    }
}
