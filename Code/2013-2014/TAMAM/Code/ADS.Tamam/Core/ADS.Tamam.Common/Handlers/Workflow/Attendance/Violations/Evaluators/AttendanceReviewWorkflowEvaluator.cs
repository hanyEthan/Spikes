using System.Runtime.Serialization;
using ADS.Common.Workflow.Contracts;
using ADS.Common.Workflow.Models;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.Violations.Models;

namespace ADS.Tamam.Common.Handlers.Workflow.Attendance.Violations.Evaluators
{
    [DataContract(IsReference = true)]
    public class AttendanceReviewWorkflowEvaluator : WorkflowDataEvaluatorBase
    {
        public override string Evaluate(WorkflowBaseData data)
        {
            if (data == null) return "";
            if (data.Command.ToLower() == AttendanceReviewStatus.Approved.ToString().ToLower()) return AttendanceReviewStatus.Approved.ToString().ToLower();
            if (data.Command.ToLower() == AttendanceReviewStatus.Denied.ToString().ToLower()) return AttendanceReviewStatus.Denied.ToString().ToLower();
            return "";
        }
    }
}
