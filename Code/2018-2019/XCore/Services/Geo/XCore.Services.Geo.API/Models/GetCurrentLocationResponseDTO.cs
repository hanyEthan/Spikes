using XCore.Services.Geo.Core.Models.Domain;

namespace XCore.Services.Geo.API.Models
{
    public class GetCurrentLocationResponseDTO
    {
        public LocationEventDTO Location { get; set; }
    }
}