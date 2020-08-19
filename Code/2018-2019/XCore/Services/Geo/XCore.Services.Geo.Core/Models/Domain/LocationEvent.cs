using Newtonsoft.Json;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Geo.Core.Models.Domain
{
    public class LocationEvent : Entity<int>
    {
        #region props.

        public string EntityCode { get; set; }
        public string EntityType { get; set; }
        public string EventCode { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }

        #endregion

        #region helpers

        public static LocationEvent Map(LocationEventLatest from)
        {
            try
            {
                var serialized = JsonConvert.SerializeObject(from);
                var instance = JsonConvert.DeserializeObject<LocationEvent>(serialized);

                return instance;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
