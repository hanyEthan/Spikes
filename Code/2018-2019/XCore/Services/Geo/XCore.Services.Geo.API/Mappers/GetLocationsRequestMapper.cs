using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Utilities;
using XCore.Services.Geo.API.Models;
using XCore.Services.Geo.Core.Models.Commands;
using XCore.Services.Geo.Core.Models.Search;

namespace XCore.Services.Geo.API.Mappers
{
    public class GetLocationsRequestMapper : IModelMapper<GetLocationsRequestDomain, GetLocationsRequestDTO>
    {
        #region IModelMapper

        public GetLocationsRequestDomain Map(GetLocationsRequestDTO from, object metadata = null)
        {
            try
            {
                if (from == null) return null;

                var to = new GetLocationsRequestDomain()
                {
                    Criteria = new Core.Models.Search.LocationEventSearchCriteria()
                    {
                        EntityCode = from.EntityCode,
                        OrderBy = MapEnum(from.OrderBy),
                        PageNumber = from.PageNumber,
                        PageSize = from.PageSize,
                        PagingEnabled = from.PagingEnabled,
                        OrderByDirection = MapEnum(from.OrderDirection),
                    },
                };

                return to;
            }
            catch (Exception e)
            {
                XLogger.Error("Exception : " + e);
                return null;
            }
        }
        public GetLocationsRequestDTO Map(GetLocationsRequestDomain from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region helpers.

        private LocationEventSearchCriteria.OrderByExepression? MapEnum(GetLocationsRequestDTOOrderBy? from)
        {
            if (from == null) return null;

            switch (from.Value)
            {
                case GetLocationsRequestDTOOrderBy.Date:
                    return LocationEventSearchCriteria.OrderByExepression.CreatedDate;
                case GetLocationsRequestDTOOrderBy.Default:
                default:
                    return LocationEventSearchCriteria.OrderByExepression.Default;
            }
        }
        private SearchCriteria.OrderDirection? MapEnum(GetLocationsRequestDTOOrderDirection? from)
        {
            if (from == null) return null;

            switch (from.Value)
            {
                case GetLocationsRequestDTOOrderDirection.Descending:
                    return SearchCriteria.OrderDirection.Descending;
                case GetLocationsRequestDTOOrderDirection.Ascending:
                default:
                    return SearchCriteria.OrderDirection.Ascending;
            }
        }

        #endregion
    }
}
