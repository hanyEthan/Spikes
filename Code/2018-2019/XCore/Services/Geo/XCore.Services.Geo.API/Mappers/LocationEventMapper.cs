using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Geo.API.Models;
using XCore.Services.Geo.Core.Models.Domain;

namespace XCore.Services.Geo.API.Mappers
{
    public class LocationEventMapper : IModelMapper<LocationEvent, LocationEventDTO>
    {
        #region IModelMapper

        public LocationEventDTO Map(LocationEvent from, object metadata = null)
        {
            if (from == null) return null;

            var to = new LocationEventDTO()
            {
                Id = from.Id,
                EntityCode = from.EntityCode,
                EntityType = from.EntityType,
                EventCode = from.EventCode,
                Latitude = from.Latitude,
                Longitude = from.Longitude,
                MetaData = from.MetaData,

                CreatedDate = this.Map(from.CreatedDate, "YYYYMMDD HH:mm:ss"),
                ModifiedDate = this.Map(from.ModifiedDate, "YYYYMMDD HH:mm:ss"),
            };

            return to;
        }
        public LocationEvent Map(LocationEventDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new LocationEvent()
            {
                EntityCode = from.EntityCode,
                EntityType = from.EntityType,
                EventCode = from.EventCode,
                Latitude = from.Latitude,
                Longitude = from.Longitude,
                MetaData = from.MetaData,
            };

            return to;
        }

        #endregion
        #region helpers.

        private string Map(DateTime? from , string format)
        {
            return from?.ToString(format);
        }

        #endregion
    }
}
