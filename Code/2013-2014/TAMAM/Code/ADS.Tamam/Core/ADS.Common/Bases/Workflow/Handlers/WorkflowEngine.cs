using System;
using System.Collections.Generic;
using ADS.Common.Utilities;
using ADS.Common.Workflow.Contracts;
using ADS.Common.Workflow.Enums;
using ADS.Common.Workflow.Models;

namespace ADS.Common.Workflow.Handlers
{
    public class WorkflowEngine
    {
        # region props ...

        public bool Initialized { get; private set; }
        private IWorkflowDataHandler _DataHandler;

        # endregion
        #region cst ...

        public WorkflowEngine( IWorkflowDataHandler dataHandler )
        {
            XLogger.Info( "" );

            try
            {
                _DataHandler = dataHandler;
                Initialized = _DataHandler != null && _DataHandler.Initialized;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
            }
        }

        #endregion
        #region publics ...

        public bool Initialize( IWorkflowTarget target , IWorkflowDefinition definition )
        {
            try
            {
                // validate ...
                if ( target == null || target.Id == Guid.Empty || definition == null || definition.Id == Guid.Empty ) return false;
                var instance = GetInstance( target.Id , definition.Id );   // check uniqueness ...

                if ( instance != null && instance.Status != WorkflowInstanceStatus.Cancelled ) return false;

                // build ...
                instance = BuildInstance( target , definition );
                if ( instance == null ) return false;

                // persist ...
                if ( !_DataHandler.InstanceSave( instance ) ) return false;

                // invoke ...
                return Invoke( target , definition.Id , null );
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        public bool Invoke( IWorkflowTarget target , Guid definitionId , WorkflowBaseData data )
        {
            try
            {
                var instance = GetInstance( target.Id , definitionId );            // instance ...
                return Invoke( instance , data );                                  // ...
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public bool Invoke( Guid instanceId , WorkflowBaseData data )
        {
            try
            {
                var instance = GetInstance( instanceId );                          // instance ...
                return Invoke( instance , data );                                  // ...
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        public bool Cancel( IWorkflowTarget target , Guid definitionId )
        {
            return Cancel( target , definitionId , null );
        }
        public bool Cancel( IWorkflowTarget target , Guid definitionId , WorkflowBaseData data )
        {
            try
            {
                var instance = GetInstance( target.Id , definitionId );            // instance ...
                return Cancel( instance , data );                                  // ...
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public bool Cancel( Guid instanceId )
        {
            return Cancel( instanceId , null );
        }
        public bool Cancel( Guid instanceId , WorkflowBaseData data )
        {
            try
            {
                var instance = GetInstance( instanceId );                          // instance ...
                return Cancel( instance , data );                                  // ...
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        public bool CancelAndDelete( Guid instanceId )
        {
            return CancelAndDelete( instanceId , null );
        }
        public bool CancelAndDelete( Guid instanceId , WorkflowBaseData data )
        {
            try
            {
                var instance = GetInstance( instanceId );
                var cancelState = Cancel( instance , data );
                var deleteState = DeleteInstance( instance );

                return cancelState && deleteState;
            }
            catch ( Exception x)
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public bool CancelAndDelete( IWorkflowTarget target , Guid definitionId )
        {
            return CancelAndDelete( target , definitionId , null );
        }
        public bool CancelAndDelete( IWorkflowTarget target , Guid definitionId , WorkflowBaseData data )
        {
            try
            {
                var instance = GetInstance( target.Id , definitionId );            // instance ...
                var cancelState = Cancel( instance , data );
                var deleteState = DeleteInstance( instance );

                return cancelState && deleteState;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }


        public bool Reset( IWorkflowTarget target , IWorkflowDefinition definition )
        {
            try
            {
                var instance = GetInstance( target.Id , definition.Id );           // instance (old) ...
                if ( instance != null )
                {
                    Invoke( instance.ResetStep , null );                           // run reset action (if any) ...
                    DeleteInstance( instance );                                    // delete ...
                }
                return CreateInstance( target , definition );                      // create ...
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }
        public bool Maintain( IWorkflowTarget target , IWorkflowDefinition definition )
        {
            try
            {
                var old = GetInstance( target.Id , definition.Id );                          // instance : old ...
                if ( old == null ) return CreateInstance( target , definition );

                var neo = BuildInstance( target , definition );                              // instance : new ...
                if ( neo == null ) return false;

                if ( !CompareInstances( old , neo ) ) return Reset( target , definition );   // compare / reset

                return true;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        public WorkflowInstance GetInstance( Guid instanceId )
        {
            return _DataHandler.InstanceGet( instanceId );
        }
        public WorkflowInstance GetInstance( Guid targetId , Guid definitionId )
        {
            return _DataHandler.InstanceGetByTarget( targetId , definitionId );
        }

        public List<WorkflowInstance> GetInstancesByOwner( Guid personId )
        {
            return _DataHandler.InstancesGetByPerson( personId );
        }

        public List<WorkflowInstance> GetInstancesByAffinity( string metadata )
        {
            return _DataHandler.InstancesGetByAffinity( metadata );
        }
        public List<WorkflowInstance> GetInstancesByAffinity( string metadata , WorkflowInstanceStatus status )
        {
            return _DataHandler.InstancesGetByAffinity( metadata , status );
        }

        public bool InstanceExists( Guid targetId , Guid definitionId )
        {
            return _DataHandler.InstanceExists( targetId , definitionId );
        }

        public bool DeleteInstance( IWorkflowTarget target , IWorkflowDefinition definition )
        {
            try
            {
                var instance = GetInstance( target.Id , definition.Id );            // instance (old) ...
                if ( instance == null ) return false;

                return DeleteInstance( instance );                                  // delete ...
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        #endregion
        #region Helpers

        private bool Invoke( WorkflowInstance instance , WorkflowBaseData data )
        {
            if ( !IsValidInstance( instance ) ) return false;                  // error ...
            var step = instance.CurrentStep ?? instance.InitialStep;           // step ...

            return Invoke( step , data );                                      // ...
        }
        private bool Invoke( WorkflowStep step , WorkflowBaseData data )
        {
            if ( step == null ) return false;                            // error ...
            var state = step.Process( data , false );                    // process ...
            if ( !state ) return false;                                  // error ...
            step.Instance.TimeEdited = DateTime.Now;                     // time ...
            var saved = _DataHandler.InstanceSave( step.Instance );      // persist ...
            return state && saved;
        }
        private bool Cancel( WorkflowInstance instance , WorkflowBaseData data )
        {
            if ( instance == null ) return false;                              // error ...
            var step = instance.CancelStep;                                    // step ...
            instance.Status = WorkflowInstanceStatus.Cancelled;                // cancel ...

            return Invoke( step , data );                                      // ...
        }

        private bool CreateInstance( IWorkflowTarget target , IWorkflowDefinition definition )
        {
            return Initialize( target , definition );
        }
        private bool DeleteInstance( WorkflowInstance instance )
        {
            if ( instance == null ) throw new Exception( "invalid instance for given target" );
            if ( !_DataHandler.InstanceDelete( instance ) ) throw new Exception( "couldn't delete old instance" );

            return true;
        }
        private WorkflowInstance BuildInstance( IWorkflowTarget target , IWorkflowDefinition definition )
        {
            // build ...
            var instance = definition.Process( target );
            if ( instance == null ) return null;

            // prepare ...
            instance = PrepareInstance( instance , target , definition );

            return instance;
        }
        private bool CompareInstances( WorkflowInstance old , WorkflowInstance neo )
        {
            if ( old == null ) return false;
            if ( old.PolicyId != neo.PolicyId ) return false;
            if ( old.CheckPointsTemplates.Count != neo.CheckPointsTemplates.Count ) return false;

            for ( int i = 0 ; i < old.CheckPointsTemplates.Count ; i++ )
            {
                if ( old.CheckPointsTemplates[i].PersonId != neo.CheckPointsTemplates[i].PersonId ) return false;
            }

            return true;
        }

        private bool IsValidInstance( WorkflowInstance instance )
        {
            return instance != null && instance.Status != WorkflowInstanceStatus.Finished;
        }
        private WorkflowInstance PrepareInstance( WorkflowInstance instance , IWorkflowTarget target , IWorkflowDefinition definition )
        {
            instance.Id = instance.Id == Guid.Empty ? Guid.NewGuid() : instance.Id;
            instance.Status = WorkflowInstanceStatus.InProgress;
            instance.TargetId = target.Id;
            instance.PersonId = target.PersonId;
            instance.Definition = definition;
            instance.DefinitionId = definition.Id;
            instance.TimeCreated = DateTime.Now;

            return PrepareInstanceSupportingTypes( instance , definition.WorkflowSupportingTypes );
        }
        private WorkflowInstance PrepareInstanceSupportingTypes( WorkflowInstance instance , List<string> supportingTypes )
        {
            instance.WorkflowSupportingTypes = new List<string>()
            {
                typeof( WorkflowStepCommand ).AssemblyQualifiedName ,
                typeof( WorkflowStepConditional ).AssemblyQualifiedName ,
            };

            instance.WorkflowSupportingTypes.AddRange( supportingTypes );

            return instance;
        }

        #endregion
    }
}
