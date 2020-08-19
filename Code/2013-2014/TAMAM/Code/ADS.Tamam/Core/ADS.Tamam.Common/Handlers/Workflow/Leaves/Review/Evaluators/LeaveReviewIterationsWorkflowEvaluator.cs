using System.Runtime.Serialization;
using ADS.Common.Workflow.Contracts;
using ADS.Common.Workflow.Models;
using ADS.Tamam.Common.Handlers.Workflow.Leaves.Review.Data;

namespace ADS.Tamam.Common.Handlers.Workflow.Leaves.Review.Evaluators
{
    [DataContract( IsReference = true )]
    public class LeaveReviewIterationsWorkflowEvaluator : WorkflowDataEvaluatorBase
    {
        public override string Evaluate( WorkflowBaseData data )
        {
            var dataStored = this.Step.Instance.Data as NotificationsSettingsWorkflowData;

            dataStored.CurrentIterationNumber++;

            if ( dataStored == null || ( dataStored.CurrentIterationNumber >= dataStored.MaxIterations ) ) return "max";
            else return "";
        }
    }
}
