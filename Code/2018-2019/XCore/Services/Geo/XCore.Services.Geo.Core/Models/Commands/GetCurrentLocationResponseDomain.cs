
using XCore.Services.Geo.Core.Models.Domain;

namespace XCore.Services.Geo.Core.Models.Commands
{
    public class GetCurrentLocationResponseDomain
    {
        public LocationEvent Location { get; set; }
    }
}