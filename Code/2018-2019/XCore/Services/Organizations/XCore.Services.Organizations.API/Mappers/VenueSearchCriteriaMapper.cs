using System;
using VenueDTOXCore.Services.Organizations.API.Models.Venue;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.API.Mappers
{
    public class VenueSearchCriteriaMapper : IModelMapper<VenueSearchCriteria, VenueSearchCriteriaDTO>,
                                           IModelMapper<VenueSearchCriteriaDTO, VenueSearchCriteria>




    {
        #region props.

        public static VenueSearchCriteriaMapper Instance { get; } = new VenueSearchCriteriaMapper();

        #endregion
        #region IModelMapper

        public VenueSearchCriteria Map(VenueSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new VenueSearchCriteria()
            {
                Id = from.Id,
                Code = from.Code,
                Name = from.Name,
                IsActive = from.IsActive,
                Order = (VenueSearchCriteria.OrderByExpression?)from.Order,
                OrderByCultureMode = (SearchCriteria.OrderByCulture)from.OrderByCultureMode,
                OrderByDirection = (SearchCriteria.OrderDirection?)from.OrderByDirection,
                PageNumber = from.PageNumber,
                PageSize = from.PageSize,
                PagingEnabled = from.PagingEnabled,




            };

            return to;
        }
        public VenueSearchCriteriaDTO Map(VenueSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}                    
