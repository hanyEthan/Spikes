using ADS.Common.Context;
using ADS.Common.Contracts;
using ADS.Tamam.Common.Data.Context;

namespace ADS.Tamam.Common.Data.Contracts
{
    public interface IIntegrationHandler : IBaseHandler
    {
        ExecutionResponse<bool> Synchronize(RequestContext requestContext);
    }
}
