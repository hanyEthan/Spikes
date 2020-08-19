
using System.Collections.Generic;
using XCore.Services.Geo.Core.Models.Domain;

namespace XCore.Services.Geo.Core.Models.Commands
{
    public class GetLocationsResponseDomain
    {
        public List<LocationEvent> Location { get; set; }
    }
}