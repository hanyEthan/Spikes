using System.Runtime.Serialization;
using ADS.Common.Workflow.Contracts;
using ADS.Common.Workflow.Models;
using ADS.Tamam.Common.Handlers.Workflow.Leaves.Review.Models;

namespace ADS.Tamam.Common.Workflow.Leaves.Review.Evaluators
{
    [DataContract(IsReference = true)]
    public class LeaveReviewWorkflowEvaluator : WorkflowDataEvaluatorBase
    {
        public override string Evaluate(WorkflowBaseData data)
        {
            if (data == null) return "";
            if (data.Command.ToLower() == WorkflowLeaveReviewStatus.Approved.ToString().ToLower()) return WorkflowLeaveReviewStatus.Approved.ToString().ToLower();
            if (data.Command.ToLower() == WorkflowLeaveReviewStatus.Denied.ToString().ToLower()) return WorkflowLeaveReviewStatus.Denied.ToString().ToLower();
            if (data.Command.ToLower() == WorkflowLeaveReviewStatus.SystemApproved.ToString().ToLower()) return WorkflowLeaveReviewStatus.SystemApproved.ToString().ToLower();
            return "";
        }
    }
}
