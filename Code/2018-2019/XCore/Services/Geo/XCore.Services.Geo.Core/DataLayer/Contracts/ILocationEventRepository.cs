using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Services.Geo.Core.Models.Domain;
using XCore.Services.Geo.Core.Models.Search;

namespace XCore.Services.Geo.Core.DataLayer.Contracts
{
    public interface ILocationEventRepository : IRepository<LocationEvent>
    {
        bool AddLocationEvent(LocationEvent location);
        LocationEventsSearchResults GetLocations(LocationEventSearchCriteria criteria, string includeProperties = null, bool detached = false);
    }
}
