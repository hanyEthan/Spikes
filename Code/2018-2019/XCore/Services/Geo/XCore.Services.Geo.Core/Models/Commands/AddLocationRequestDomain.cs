using XCore.Services.Geo.Core.Models.Domain;

namespace XCore.Services.Geo.Core.Models.Commands
{
    public class AddLocationRequestDomain
    {
        public LocationEvent Location { get; set; }
    }
}