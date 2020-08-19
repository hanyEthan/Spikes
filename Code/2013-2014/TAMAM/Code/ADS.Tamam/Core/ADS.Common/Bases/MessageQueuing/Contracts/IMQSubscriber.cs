using ADS.Common.Contracts;
using ADS.Common.Bases.MessageQueuing.Contracts;
using ADS.Common.Bases.MessageQueuing.Models;

namespace ADS.Common.Bases.MessageQueuing.Contracts
{
    public interface IMQSubscriber<T> : IBaseHandler where T : IMQMessageContent
    {
        bool Handle( MQMessage message );
        bool Handle<T>( T content ) where T : IMQMessageContent;
    }
}
