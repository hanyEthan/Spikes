using System;
using System.Collections.Generic;
using ADS.Common.Contracts;
using ADS.Common.Workflow.Enums;
using ADS.Common.Workflow.Models;

namespace ADS.Common.Workflow.Contracts
{
    public interface IWorkflowDataHandler : IBaseHandler
    {
        WorkflowInstance InstanceGet( Guid id );
        WorkflowInstance InstanceGetByTarget( Guid targetId , Guid definitionId );
        List<WorkflowInstance> InstancesGetByPerson( Guid personId );
        List<WorkflowInstance> InstancesGetByAffinity( string metadata );
        List<WorkflowInstance> InstancesGetByAffinity( string metadata, WorkflowInstanceStatus status );

        bool InstanceSave( WorkflowInstance instance );
        bool InstanceDelete( WorkflowInstance instance );
        bool InstanceDelete( Guid id );
        bool InstanceExists( Guid targetId , Guid definitionId );
    }
}