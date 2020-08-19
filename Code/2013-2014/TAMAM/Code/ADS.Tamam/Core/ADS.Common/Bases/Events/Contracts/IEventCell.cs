using System;
using ADS.Common.Bases.MessageQueuing.Contracts;

namespace ADS.Common.Bases.Events.Contracts
{
    public interface IEventCell : IMQMessageContent
    {
        Guid Id { get; set; }

        string ContentType { get; }
        string TargetId { get; }
        string TargetType { get; }

        bool Process();
    }
}
