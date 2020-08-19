using System.Runtime.Serialization;
using ADS.Common.Contracts;
using ADS.Common.Workflow.Contracts;

namespace ADS.Common.Workflow.Models
{
    [DataContract( IsReference = true )]
    public abstract class WorkflowActionBase : IXSerializable
    {
        [DataMember] public WorkflowStep Step { get; set; }
        [DataMember] public WorkflowBaseData Data { get; set; }

        public abstract bool Process( WorkflowBaseData data );
    }
}
