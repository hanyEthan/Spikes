using System;
using System.Collections.Generic;
using ADS.Common.Workflow.Models;

namespace ADS.Common.Workflow.Contracts
{
    public interface IWorkflowDefinition
    {
        Guid Id { get; }
        List<string> WorkflowSupportingTypes { get; }

        WorkflowInstance Process(IWorkflowTarget target);
    }
}
