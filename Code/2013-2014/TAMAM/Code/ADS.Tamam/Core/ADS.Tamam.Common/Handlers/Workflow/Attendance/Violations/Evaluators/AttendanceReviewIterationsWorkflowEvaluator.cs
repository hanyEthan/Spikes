using System.Runtime.Serialization;
using ADS.Common.Workflow.Contracts;
using ADS.Common.Workflow.Models;
using ADS.Tamam.Common.Handlers.Workflow.Attendance.Violations.Data;

namespace ADS.Tamam.Common.Handlers.Workflow.Attendance.Violations.Evaluators
{
    [DataContract(IsReference = true)]
    public class AttendanceReviewIterationsWorkflowEvaluator : WorkflowDataEvaluatorBase
    {
        public override string Evaluate(WorkflowBaseData data)
        {
            var dataStored = this.Step.Instance.Data as NotificationsSettingsWorkflowData;
            
            dataStored.CurrentIterationNumber++;

            if ( dataStored == null || ( dataStored.CurrentIterationNumber >= dataStored.MaxIterations ) ) return "max";
            else return "";
        }
    }
}
