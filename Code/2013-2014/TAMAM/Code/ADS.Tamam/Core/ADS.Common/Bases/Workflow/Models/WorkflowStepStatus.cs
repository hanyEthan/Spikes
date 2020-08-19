using System;
using System.Runtime.Serialization;
using ADS.Common.Contracts;

namespace ADS.Common.Workflow.Models
{
    [DataContract( IsReference = true )]
    public class WorkflowStepStatus : IXSerializable
    {
        [DataMember] public Guid Id { get; set; }
        [DataMember] public Guid StepId { get; set; }
        [DataMember] public WorkflowStep Step { get; set; }
        [DataMember] public string Status { get; set; }
        [DataMember] public string Command { get; set; }
        [DataMember] public string PersonId { get; set; }
        [DataMember] public string Metadata { get; set; }
        [DataMember] public DateTime TimeEdited { get; set; }

        #region cst ...

        public WorkflowStepStatus()
        {
            TimeEdited = DateTime.Now;
        }
        public WorkflowStepStatus( Guid id , Guid stepId ) : this()
        {
            Id = id;
            StepId = stepId;
        }
        public WorkflowStepStatus( Guid id , Guid stepId , string command , string status , string metadata , string personId ) : this( id , stepId )
        {
            Status = status;
            Metadata = metadata;
            PersonId = personId;
            Command = command;
        }
        
        #endregion
    }
}
