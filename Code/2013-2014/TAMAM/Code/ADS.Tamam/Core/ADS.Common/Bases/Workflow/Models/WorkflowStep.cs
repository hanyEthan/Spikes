using System;
using System.Linq;
using System.Collections.Generic;

using ADS.Common.Contracts;
using ADS.Common.Utilities;
using ADS.Common.Workflow.Enums;
using ADS.Common.Workflow.Contracts;
using System.Runtime.Serialization;

namespace ADS.Common.Workflow.Models
{
    [DataContract( IsReference = true )]
    public class WorkflowStep : IXSerializable
    {
        #region props ...

        [DataMember] public Guid Id { get; set; }

        private WorkflowStepType _Type = WorkflowStepType.Action;
        [DataMember] public virtual WorkflowStepType Type { get { return _Type; } set { _Type = value; } }
        [DataMember] public WorkflowInstance Instance { get; set; }
        [DataMember] public WorkflowStep NextStep { get; set; }
        [DataMember] public WorkflowActionBase Action { get; set; }
        [DataMember] public WorkflowBaseData Data { get; set; }
        [DataMember] public WorkflowDataEvaluatorBase Evaluator { get; set; }
        [DataMember] public WorkflowCheckPoint CheckPointTemplate { get; set; }
        [DataMember] public bool HasCheckPoint { get; set; }        

        #endregion
        #region cst ...

        public WorkflowStep()
        {
        }
        public WorkflowStep( Guid id , WorkflowInstance instance , WorkflowBaseData data , WorkflowDataEvaluatorBase evaluator )
        {
            Id = id;
            Instance = instance;
            Data = data;
            Evaluator = evaluator;
        }

        #endregion
        #region publics

        public WorkflowStep Next( WorkflowStep nextStep )
        {
            return NextStep = nextStep;
        }
        public WorkflowStep Next<T>() where T : WorkflowStep , new()
        {
            return Next<T>( null );
        }
        public WorkflowStep Next<T>( WorkflowBaseData data ) where T : WorkflowStep , new()
        {
            return NextStep = new T() { Id = Guid.NewGuid() , Instance = this.Instance , Data = data , Evaluator = null };
        }
        public WorkflowStep Next<T , D>() where T : WorkflowStep , new() where D : WorkflowDataEvaluatorBase , new()
        {
            return Next<T , D>( null );
        }
        public WorkflowStep Next<T , D>( WorkflowBaseData data ) where T : WorkflowStep , new() where D : WorkflowDataEvaluatorBase , new()
        {
            return NextStep = new T() { Id = Guid.NewGuid() , Instance = this.Instance , Data = data , Evaluator = new D() };
        }
        public WorkflowStep Run<T>( WorkflowBaseData data ) where T : WorkflowActionBase , new()
        {
            this.Action = new T();
            this.Action.Data = data;
            this.Action.Step = this;

            return this;
        }
        public WorkflowStep Checkpoint( WorkflowBaseData data )
        {
            this.CheckPointTemplate = new WorkflowCheckPoint( Guid.NewGuid() , this.Id , null , null , null , data.PersonId );
            this.Instance.CheckPointsTemplates.Add( this.CheckPointTemplate );

            this.HasCheckPoint = true;
            return this;
        }
        public WorkflowStep CheckpointPartial( WorkflowBaseData data )
        {
            //this.CheckPointTemplate = new WorkflowCheckPoint( Guid.NewGuid() , this.Id , null , null , null , data.PersonId );
            //this.Instance.CheckPointsTemplates.Add( this.CheckPointTemplate );

            this.HasCheckPoint = true;
            return this;
        }
        public WorkflowStepConditional.Branch When( string condition )
        {
            // validate ...
            if ( NextStep == null || !( NextStep is WorkflowStepConditional ) )
            {
                // if next step is not created yet or another type of next step exits, create a new conditional step and override the next node with it ...
                NextStep = new WorkflowStepConditional() { Id = Guid.NewGuid() , Instance = this.Instance };
            }
            else
            {
                //if ( string.IsNullOrWhiteSpace( condition ) ) throw new Exception( "condition is invalid" );
                if ( condition == null ) throw new Exception( "condition is invalid" );
                if ( ( ( WorkflowStepConditional ) NextStep ).ConditionExist( condition ) ) throw new Exception( "condition already exists within the same node" );
            }

            // branching ...
            var branch = new WorkflowStepConditional.Branch( this.Instance , new WorkflowStepConditional.Condition( condition ) );
            ( ( WorkflowStepConditional ) NextStep ).Branches.Add( branch );

            // evaluator ...
            NextStep.Data = this.Data;
            NextStep.Evaluator = this.Evaluator;

            return branch;
        }

        internal virtual bool Process( WorkflowBaseData data , bool cascaded )
        {
            try
            {
                var stepData = data ?? this.Data;

                this.Instance.CurrentStep = this;                         // current ...
                bool state = Action == null || Action.Process( data );    // action ...
                if ( !SaveState( state ? "done" : "error" , stepData != null ? stepData.Command : "" , stepData != null ? stepData.Metadata : "" , stepData != null ? stepData.PersonId : "" ) ) return false;   // status ...
                if ( !state ) return false;                               // error ...
                if ( this.NextStep == null ) return FinishWorkflow();     // finished ...
                return this.NextStep.Process( this.Data , true );         // next ...
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );

                SaveState( "error" , "" , "" , "" );
                return false;
            }
        }
        
        #endregion
        #region helpers

        protected bool SaveState( string status , string command , string metadata , string personId )
        {
            try
            {
                // step status ...
                var stepStatus = new WorkflowStepStatus( Guid.NewGuid() , this.Id , command , status , metadata , personId );
                this.Instance.StepStatuses.Add( stepStatus );

                MaintainCheckPoints( status , command , metadata , personId);

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        private void MaintainCheckPoints( string status , string command , string metadata , string personId )
        {
            // rebuild CPS from CPT ...
            if ( this == this.Instance.InitialStep )
            {
                this.Instance.CPS = new Stack<WorkflowCheckPoint>();
                for ( int i = this.Instance.CheckPointsTemplates.Count - 1 ; i >= 0 ; i-- )
                {
                    this.Instance.CPS.Push( this.Instance.CheckPointsTemplates[i] );
                }
            }
            else
            {
                if ( this.HasCheckPoint )
                {
                    var CPSItem = this.Instance.CPS.Count > 0 ? this.Instance.CPS.Peek() : null;
                    if ( this.CheckPointTemplate != null && CPSItem != null && this.CheckPointTemplate.StepId == CPSItem.StepId )
                    {
                        this.Instance.CPS.Pop();
                    }

                    // checkpoint ...
                    var checkpoint = new WorkflowCheckPoint( Guid.NewGuid() , this.Id , command , status , metadata , personId )
                    {
                        TimeEdited = DateTime.Now ,
                    };
                    this.Instance.CheckPoints.Add( checkpoint );
                }
            }

            #region OLD IMPLEMENTATION

            //// checkpoint ...
            //if ( this.HasCheckPoint )
            //{
            //    // checkpoint template ...
            //    this.CheckPointTemplate.Command = command;
            //    this.CheckPointTemplate.Status = status;
            //    this.CheckPointTemplate.Metadata = metadata;
            //    this.CheckPointTemplate.TimeEdited = DateTime.Now;

            //    //this.Instance.CheckPointsTemplates.Add( this.CheckPointTemplate );

            //    // checkpoint ...
            //    var checkpoint = new WorkflowCheckPoint( Guid.NewGuid() , this.Id , command , status , metadata , personId )
            //    {
            //        TimeEdited = DateTime.Now ,
            //    };

            //    this.Instance.CheckPoints.Add( checkpoint );
            //}
            
            #endregion
        }


        protected bool FinishWorkflow()
        {
            try
            {
                if ( this.Instance.Status == WorkflowInstanceStatus.InProgress )
                {
                    this.Instance.Status = WorkflowInstanceStatus.Finished;
                }

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        #endregion
    }

    [DataContract( IsReference = true )]
    public class WorkflowStepConditional : WorkflowStep
    {
        #region nested ...

        [DataContract( IsReference = true )]
        public class Branch : IXSerializable
        {
            #region props ...

            [DataMember] public WorkflowInstance Instance { get; set; }
            [DataMember] public Condition Condition { get; set; }
            [DataMember] public WorkflowStep NextStep { get; set; }
            
            #endregion
            #region cst ...

		    public Branch() {}
            public Branch( WorkflowInstance instance , Condition condition ) : this()
            {
                Condition = condition;
                Instance = instance;
            }
            public Branch( WorkflowInstance instance , Condition condition , WorkflowStep step ) : this( instance , condition )
	        {
                NextStep = step;
	        }
 
	        #endregion
            #region publics ...

            public WorkflowStep Next( WorkflowStep nextStep )
            {
                return NextStep = nextStep;
            }
            public WorkflowStep Next<T>() where T : WorkflowStep , new()
            {
                return Next<T>( null );
            }
            public WorkflowStep Next<T>( WorkflowBaseData data ) where T : WorkflowStep , new()
            {
                return NextStep = new T() { Id = Guid.NewGuid() , Instance = this.Instance , Data = data , Evaluator = null };
            }
            public WorkflowStep Next<T , D>() where T : WorkflowStep , new() where D : WorkflowDataEvaluatorBase , new()
            {
                return Next<T , D>( null );
            }
            public WorkflowStep Next<T , D>( WorkflowBaseData data ) where T : WorkflowStep , new() where D : WorkflowDataEvaluatorBase , new()
            {
                NextStep = new T() { Id = Guid.NewGuid() , Instance = this.Instance , Data = data , Evaluator = new D() };
                NextStep.Evaluator.Step = NextStep;

                return NextStep;
            }

            #endregion
        }
        [DataContract( IsReference = true )]
        public class Condition : IXSerializable
        {
            [DataMember] public string EvaluationCondition { get; set; }

            #region cst ...

            public Condition() { }
            public Condition( string condition )
            {
                this.EvaluationCondition = condition;
            }
            
            #endregion
        }

	    #endregion
        #region props ...

        private WorkflowStepType _Type = WorkflowStepType.Condition;
        [DataMember] public virtual WorkflowStepType Type { get { return _Type; } set { _Type = value; } }

        private List<Branch> _Branches = new List<Branch>();
        [DataMember] public List<Branch> Branches { get { return _Branches; } set { _Branches = value; } }

        #endregion

        #region publics ...

        internal override bool Process( WorkflowBaseData data , bool cascaded )
        {
            try
            {
                var condition = this.Evaluator.Evaluate( data );                                                          // evaluate ...
                var branch = this.Branches.Where( x => x.Condition.EvaluationCondition.ToLower() == condition.ToLower() ).FirstOrDefault();   // choose ...
                var branchFound = branch != null;                                                                         // not found ...
                if ( !SaveState( branchFound ? "" : "" , condition , "" , "" ) ) return false;                            // status ...
                if ( !branchFound ) return false;                                                                         // error ...
                if ( branch.NextStep == null ) return FinishWorkflow();                                                   // finished ...
                return branch.NextStep.Process( this.Data , true );                                                       // next ...
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );

                SaveState( "" , "" , "" , "" );
                return false;
            }
        }
        public bool ConditionExist( string condition )
        {
            return Branches.Any( x => x.Condition.EvaluationCondition.ToLower() == condition );
        }
        
        #endregion
    }

    [DataContract( IsReference = true )]
    public class WorkflowStepCommand : WorkflowStep
    {
        #region props ...

        private WorkflowStepType _Type = WorkflowStepType.Event;
        [DataMember] public virtual WorkflowStepType Type { get { return _Type; } set { _Type = value; } }

        #endregion
        #region publics ...

        internal override bool Process( WorkflowBaseData data , bool cascaded )
        {
            this.Instance.CurrentStep = this;         // current ...
            if ( cascaded ) return true;              // waiting ...
            return base.Process( data , cascaded );   // process ...
        }
        
        #endregion
    }
}
