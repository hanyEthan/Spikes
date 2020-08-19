using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Services.Geo.Core.Models.Domain;
using XCore.Services.Geo.Core.Models.Search;

namespace XCore.Services.Geo.Core.Contracts
{
    public interface IGeoHandler : IUnityService
    {
        ExecutionResponse<LocationEvent> AddLocationEvent(LocationEvent locationEvent, RequestContext requestContext);
        ExecutionResponse<LocationEventsSearchResults> GetLocations(LocationEventSearchCriteria criteria, RequestContext requestContext);
        ExecutionResponse<LocationEvent> GetCurrentLocation(string entityCode, RequestContext requestContext);
    }
}
