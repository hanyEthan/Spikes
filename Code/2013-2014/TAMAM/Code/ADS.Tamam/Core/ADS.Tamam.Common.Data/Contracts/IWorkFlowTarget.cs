using System;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;

namespace ADS.Tamam.Common.Data.Contracts
{
    public interface IWorkFlowTarget
    {
        Guid Id { get; }
        Person Person { get; }
        WorkFlowTargetType TargetType { get; }
        int SubTypeId { get; }
        bool CheckApprovalCondition( int condition );
        double EffectiveDaysCount { get; }
        Guid PersonId { get; }
    }

    public enum WorkFlowTargetType
    {
        Leave = 1 ,
        Excuse = 2 ,
        Away = 3
    }
}
