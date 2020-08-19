using System.Runtime.Serialization;
using ADS.Common.Contracts;
using ADS.Common.Workflow.Models;

namespace ADS.Common.Workflow.Contracts
{
    [DataContract( IsReference = true )]
    public abstract class WorkflowDataEvaluatorBase : IXSerializable
    {
        [DataMember] public WorkflowStep Step { get; set; }

        public abstract string Evaluate( WorkflowBaseData data );
    }
}
