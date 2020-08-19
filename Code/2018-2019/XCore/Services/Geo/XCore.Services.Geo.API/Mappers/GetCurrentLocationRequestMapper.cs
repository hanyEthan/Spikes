using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Utilities;
using XCore.Services.Geo.API.Models;
using XCore.Services.Geo.Core.Models.Commands;

namespace XCore.Services.Geo.API.Mappers
{
    public class GetCurrentLocationRequestMapper : IModelMapper<GetCurrentLocationRequestDomain, GetCurrentLocationRequestDTO>
    {
        #region IModelMapper

        public GetCurrentLocationRequestDomain Map(GetCurrentLocationRequestDTO from, object metadata = null)
        {
            try
            {
                if (from == null) return null;

                var to = new GetCurrentLocationRequestDomain()
                {
                    EntityCode = from.EntityCode,
                };

                return to;
            }
            catch (Exception e)
            {
                XLogger.Error("Exception : " + e);
                return null;
            }
        }
        public GetCurrentLocationRequestDTO Map(GetCurrentLocationRequestDomain from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
