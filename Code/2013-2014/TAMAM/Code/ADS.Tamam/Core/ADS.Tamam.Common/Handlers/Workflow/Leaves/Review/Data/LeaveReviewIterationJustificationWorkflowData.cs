using System.Runtime.Serialization;
using ADS.Common.Workflow.Models;
using ADS.Tamam.Common.Handlers.Workflow.Leaves.Review.Models;

namespace ADS.Tamam.Common.Handlers.Workflow.Leaves.Review.Data
{
    [DataContract( IsReference = true )]
    public class LeaveReviewIterationJustificationWorkflowData : WorkflowBaseData
    {
        [DataMember] public string TargetId { get; set; }
        [DataMember] public WorkflowLeaveTargetType TargetType { get; set; }

        #region cst ...

        public LeaveReviewIterationJustificationWorkflowData() { }
        public LeaveReviewIterationJustificationWorkflowData( string personId , string targetId , WorkflowLeaveTargetType targetType )
        {
            this.PersonId = personId;
            this.TargetId = targetId;
            this.TargetType = targetType;
        }

        #endregion
    }
}