using System;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Utilities;
using XCore.Services.Geo.API.Models;
using XCore.Services.Geo.Core.Models.Commands;
using XCore.Services.Geo.Core.Models.Domain;

namespace XCore.Services.Geo.API.Mappers
{
    public class GetCurrentLocationResponseMapper : IModelMapper<ExecutionResponse<LocationEvent>, GetCurrentLocationResponseDTO>
    {
        #region props.

        private LocationEventMapper locationEventMapper { get; set; }

        #endregion
        #region cst.

        public GetCurrentLocationResponseMapper()
        {
            this.locationEventMapper = new LocationEventMapper();
        }

        #endregion

        #region IModelMapper

        public GetCurrentLocationResponseDTO Map(ExecutionResponse<LocationEvent> from, object metadata = null)
        {
            try
            {
                if (from == null) return null;

                var to = new GetCurrentLocationResponseDTO
                {
                    Location = this.locationEventMapper.Map(from.Result),
                };

                return to;
            }
            catch (Exception e)
            {
                XLogger.Error("Exception : " + e);
                return null;
            }
        }
        public ExecutionResponse<LocationEvent> Map(GetCurrentLocationResponseDTO from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
