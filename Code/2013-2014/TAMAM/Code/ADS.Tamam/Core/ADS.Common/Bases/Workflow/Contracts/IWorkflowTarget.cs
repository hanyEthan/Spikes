using System;

namespace ADS.Common.Workflow.Contracts
{
    public interface IWorkflowTarget
    {
        Guid Id { get; }
        Guid PersonId { get; }
        double EffectiveAmount { get; }
    }
}
