using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Hiring.API.Models.Search;
using XCore.Services.Hiring.Core.Models.Search;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.Core.Models;

namespace XCore.Services.Organizations.API.Mappers.Organizations
{
    public class OrganizationGetRequestMapper : IModelMapper<OrganizationsSearchCriteria, OrganizationsSearchCriteriaDTO>
    {
        #region props.

        public static OrganizationGetRequestMapper Instance { get; } = new OrganizationGetRequestMapper();

        #endregion       

        #region IModelMapper

        public OrganizationsSearchCriteria Map(OrganizationsSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new OrganizationsSearchCriteria
            {
                Apps = from.Apps,
                Modules = from.Modules,
                Order = (OrganizationsSearchCriteria.OrderByExpression)from.Order,
                OrderByCultureMode = (SearchCriteria.OrderByCulture)from.OrderByCultureMode,
                OrderByDirection = (SearchCriteria.OrderDirection?)from.OrderByDirection,
                PageNumber = from.PageNumber,
                PageSize = from.PageSize,
                PagingEnabled = from.PagingEnabled,
                Name = from.Name,
                Codes = from.Codes,
                IsActive=from.IsActive,
                SearchIncludes = from.SearchIncludes.HasValue ? (SearchIncludes)from.SearchIncludes.Value : SearchIncludes.Basic,
                Id = from.Id,

            };


            return to;
        }
        public OrganizationsSearchCriteriaDTO Map(OrganizationsSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
