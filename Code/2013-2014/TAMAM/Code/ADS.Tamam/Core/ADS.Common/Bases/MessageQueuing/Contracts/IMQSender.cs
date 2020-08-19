using ADS.Common.Contracts;
using ADS.Common.Bases.MessageQueuing.Contracts;
using ADS.Common.Bases.MessageQueuing.Models;

namespace ADS.Common.Bases.MessageQueuing.Contracts
{
    public interface IMQSender : IBaseHandler
    {
        bool Send( MQMessage message );
    }
}