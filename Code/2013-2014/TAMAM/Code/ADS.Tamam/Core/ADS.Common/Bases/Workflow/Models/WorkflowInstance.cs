using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using ADS.Common.Contracts;
using ADS.Common.Workflow.Contracts;
using ADS.Common.Workflow.Enums;

namespace ADS.Common.Workflow.Models
{
    [DataContract( IsReference = true )]
    public class WorkflowInstance : IXSerializable
    {
        #region props ...

        [DataMember] public Guid Id { get; set; }
        [DataMember] public Guid TargetId { get; set; }
        [DataMember] public string PolicyId { get; set; }
        [DataMember] public Guid PersonId { get; set; }
        [DataMember] public Guid DefinitionId { get; set; }
        [DataMember] public IWorkflowDefinition Definition { get; set; }
        [DataMember] public DateTime TimeCreated { get; set; }
        [DataMember] public DateTime TimeEdited { get; set; }
        [DataMember] public WorkflowInstanceStatus Status { get; set; }
        [DataMember] public List<string> WorkflowSupportingTypes { get; set; }
        [DataMember] public string WorkflowSupportingTypesSerialized { get; set; }
        [DataMember] public WorkflowStep InitialStep { get; set; }
        [DataMember] public WorkflowStep CurrentStep { get; set; }
        [DataMember] public WorkflowStep CancelStep { get; set; }
        [DataMember] public WorkflowStep ResetStep { get; set; }
        [DataMember] public List<WorkflowStepStatus> StepStatuses { get; set; }
        [DataMember] public List<WorkflowCheckPoint> CheckPoints { get; set; }
        [DataMember] public List<WorkflowCheckPoint> CheckPointsTemplates { get; set; }

        [DataMember] public Stack<WorkflowCheckPoint> CPS { get; set; }

        [DataMember] public string Metadata { get; set; }
        [DataMember] public WorkflowBaseData Data { get; set; }

        #endregion

        #region cst ...

        public WorkflowInstance()
        {
            StepStatuses = new List<WorkflowStepStatus>();
            CheckPoints = new List<WorkflowCheckPoint>();
            CheckPointsTemplates = new List<WorkflowCheckPoint>();
            WorkflowSupportingTypes = new List<string>();

        }
        public WorkflowInstance( string policyId ) : this()
        {
            PolicyId = policyId;
        }
        
        #endregion

        #region publics ...

        public WorkflowStep Initial( WorkflowStep step )
        {
            return InitialStep = step;
        }
        public WorkflowStep Initial<T>() where T : WorkflowStep , new()
        {
            return Initial<T>( null );
        }
        public WorkflowStep Initial<T>( WorkflowBaseData data ) where T : WorkflowStep , new()
        {
            return InitialStep = Step<T>( data );
        }
        public WorkflowStep Initial<T , D>() where T : WorkflowStep , new() where D : WorkflowDataEvaluatorBase , new()
        {
            return Initial<T,D>( null );
        }
        public WorkflowStep Initial<T , D>( WorkflowBaseData data ) where T : WorkflowStep , new() where D : WorkflowDataEvaluatorBase , new()
        {
            return InitialStep = Step<T,D>( data );
        }

        public WorkflowStep Cancellation( WorkflowStep step )
        {
            return InitialStep = step;
        }
        public WorkflowStep Cancellation<T>() where T : WorkflowStep , new()
        {
            return Cancellation<T>( null );
        }
        public WorkflowStep Cancellation<T>( WorkflowBaseData data ) where T : WorkflowStep , new()
        {
            return CancelStep = Step<T>( data );
        }
        public WorkflowStep Cancellation<T , D>() where T : WorkflowStep , new() where D : WorkflowDataEvaluatorBase , new()
        {
            return Cancellation<T , D>( null );
        }
        public WorkflowStep Cancellation<T , D>( WorkflowBaseData data ) where T : WorkflowStep , new() where D : WorkflowDataEvaluatorBase , new()
        {
            return CancelStep = Step<T , D>( data );
        }

        public WorkflowStep Resetting<T>() where T : WorkflowStep , new()
        {
            return Resetting<T>( null );
        }
        public WorkflowStep Resetting<T>( WorkflowBaseData data ) where T : WorkflowStep , new()
        {
            return ResetStep = Step<T>( data );
        }

        public WorkflowStep Step<T>() where T : WorkflowStep , new()
        {
            return Step<T>( null );
        }
        public WorkflowStep Step<T>( WorkflowBaseData data ) where T : WorkflowStep , new()
        {
            return new T() { Id = Guid.NewGuid() , Instance = this , Data = data , Evaluator = null };
        }
        public WorkflowStep Step<T , D>() where T : WorkflowStep , new() where D : WorkflowDataEvaluatorBase , new()
        {
            return Step<T , D>( null );
        }
        public WorkflowStep Step<T , D>( WorkflowBaseData data ) where T : WorkflowStep , new() where D : WorkflowDataEvaluatorBase , new()
        {
            var step = new T() { Id = Guid.NewGuid() , Instance = this , Data = data , Evaluator = new D() };
            step.Evaluator.Step = step;

            return step;
        }

        #endregion
        #region IXSerializable

        [XmlIgnoreAttribute]
        public string SerializedInstance { get; set; }
        
        #endregion
    }
}
