using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.API.Models.Search;
using XCore.Services.Hiring.Core.Models;
using XCore.Services.Hiring.Core.Models.Search;

namespace XCore.Services.Advertisements.API.Mappers.Advertisements
{
    public class AdvertisementGetRequestMapper : IModelMapper<AdvertisementsSearchCriteria, AdvertisementsSearchCriteriaDTO>
    {
        #region props.

        public static AdvertisementGetRequestMapper Instance { get; } = new AdvertisementGetRequestMapper();

        #endregion       

        #region IModelMapper

        public AdvertisementsSearchCriteria Map(AdvertisementsSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new AdvertisementsSearchCriteria
            {
                Title = from.Title,
                Apps = from.Apps,
                DateCreatedFrom = from.DateCreatedFrom,
                DateCreatedTo = from.DateCreatedTo,
                Modules = from.Modules,
                Order = (AdvertisementsSearchCriteria.OrderByExpression?) from.Order,
                OrderByCultureMode = (SearchCriteria.OrderByCulture)from.OrderByCultureMode,
                OrderByDirection = (SearchCriteria.OrderDirection?)from.OrderByDirection,
                PageNumber = from.PageNumber,
                PageSize = from.PageSize,
                PagingEnabled = from.PagingEnabled,
                Codes = from.Codes,
                SearchIncludes = from.SearchIncludes.HasValue ? (SearchIncludes)from.SearchIncludes.Value : SearchIncludes.Basic,
                IsActive = from.IsActive,
                Id = from.Id,
            };


            return to;
        }
        public AdvertisementsSearchCriteriaDTO Map(AdvertisementsSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
