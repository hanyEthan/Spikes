using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Utilities;
using XCore.Services.Geo.API.Models;
using XCore.Services.Geo.Core.Models.Domain;
using XCore.Services.Geo.Core.Models.Search;

namespace XCore.Services.Geo.API.Mappers
{
    public class GetLocationsResponseMapper : IModelMapper<ExecutionResponse<LocationEventsSearchResults>, GetLocationsResponseDTO>
    {
        #region props.

        private LocationEventMapper locationEventMapper { get; set; }

        #endregion
        #region cst.

        public GetLocationsResponseMapper()
        {
            this.locationEventMapper = new LocationEventMapper();
        }

        #endregion

        #region IModelMapper

        public GetLocationsResponseDTO Map(ExecutionResponse<LocationEventsSearchResults> from, object metadata = null)
        {
            try
            {
                if (from == null) return null;

                var to = new GetLocationsResponseDTO()
                {
                    Results = Map(from?.Result?.Results),
                    PageIndex = from?.Result?.PageIndex,
                    TotalCount = from?.Result?.TotalCount,
                };

                return to;
            }
            catch (Exception x)
            {
                XLogger.Error("Exception : " + x);
                return null;
            }
        }

        public ExecutionResponse<LocationEventsSearchResults> Map(GetLocationsResponseDTO from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region helpers

        public List<LocationEventDTO> Map(List<LocationEvent> from)
        {
            if (from == null) return null;

            var to = new List<LocationEventDTO>();

            foreach (var item in from)
            {
                var itemDTO = this.locationEventMapper.Map(item);
                if (itemDTO != null)
                {
                    to.Add(itemDTO);
                }
            }

            return to;
        }

        #endregion
    }
}
