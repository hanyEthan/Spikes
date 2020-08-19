using XCore.Services.Geo.Core.Models.Search;

namespace XCore.Services.Geo.Core.Models.Commands
{
    public class GetLocationsRequestDomain
    {
        public LocationEventSearchCriteria Criteria { get; set; }
    }
}