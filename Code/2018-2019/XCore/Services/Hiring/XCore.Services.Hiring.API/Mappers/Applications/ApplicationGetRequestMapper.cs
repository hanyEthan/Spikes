using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Hiring.API.Models.Search;
using XCore.Services.Hiring.Core.Models.Search;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.Core.Models;

namespace XCore.Services.Applications.API.Mappers.Applications
{
    public class ApplicationGetRequestMapper : IModelMapper<ApplicationsSearchCriteria, ApplicationsSearchCriteriaDTO>
    {
        #region props.

        public static ApplicationGetRequestMapper Instance { get; } = new ApplicationGetRequestMapper();

        #endregion       

        #region IModelMapper

        public ApplicationsSearchCriteria Map(ApplicationsSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ApplicationsSearchCriteria
            {
                Apps = from.Apps,
                DateCreatedFrom = from.DateCreatedFrom,
                DateCreatedTo = from.DateCreatedTo,
                Modules = from.Modules,
                Order = (ApplicationsSearchCriteria.OrderByExpression)from.Order,
                OrderByCultureMode = (SearchCriteria.OrderByCulture)from.OrderByCultureMode,
                OrderByDirection = (SearchCriteria.OrderDirection?)from.OrderByDirection,
                PageNumber = from.PageNumber,
                PageSize = from.PageSize,
                PagingEnabled = from.PagingEnabled,
                Codes = from.Codes,
                IsActive = from.IsActive,
                SearchIncludes = from.SearchIncludes.HasValue ? (SearchIncludes)from.SearchIncludes.Value : SearchIncludes.Basic,
                Id = from.Id,

            };


            return to;
        }
        public ApplicationsSearchCriteriaDTO Map(ApplicationsSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
