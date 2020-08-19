using System.Runtime.Serialization;
using ADS.Common.Contracts;

namespace ADS.Common.Workflow.Models
{
    [DataContract( IsReference = true )]
    public abstract class WorkflowBaseData : IXSerializable
    {
        [DataMember] public string PersonId { get; set; }
        [DataMember] public string Command { get; set; }
        [DataMember] public string Metadata { get; set; }
    }
}
