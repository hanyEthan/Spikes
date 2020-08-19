using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.API.Models.City;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.API.Mappers
{
    public class CitySearchCriteriaMapper : IModelMapper<CitySearchCriteria, CitySearchCriteriaDTO>,
                                           IModelMapper<CitySearchCriteriaDTO, CitySearchCriteria>
    {
        #region props.

        public static CitySearchCriteriaMapper Instance { get; } = new CitySearchCriteriaMapper();

        #endregion
        #region IModelMapper

        public CitySearchCriteria Map(CitySearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new CitySearchCriteria()
            {
                Id = from.Id,
                Code = from.Code,
                Name = from.Name,
                IsActive = from.IsActive,
                Order = (CitySearchCriteria.OrderByExpression?)from.Order,
                OrderByCultureMode = (SearchCriteria.OrderByCulture)from.OrderByCultureMode,
                OrderByDirection = (SearchCriteria.OrderDirection?)from.OrderByDirection,
                PageNumber = from.PageNumber,
                PageSize = from.PageSize,
                PagingEnabled = from.PagingEnabled,




            };

            return to;
        }
        public CitySearchCriteriaDTO Map(CitySearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}                    
