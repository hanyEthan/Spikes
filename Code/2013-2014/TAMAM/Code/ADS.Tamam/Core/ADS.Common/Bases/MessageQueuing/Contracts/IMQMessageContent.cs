using System.Collections.Generic;

namespace ADS.Common.Bases.MessageQueuing.Contracts
{
    public interface IMQMessageContent
    {
        string ContentType { get; }
    }
}
