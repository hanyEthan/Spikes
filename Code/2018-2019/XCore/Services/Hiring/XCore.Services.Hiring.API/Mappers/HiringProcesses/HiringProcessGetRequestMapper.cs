using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Hiring.API.Models.Search;
using XCore.Services.Hiring.Core.Models.Search;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.Core.Models;

namespace XCore.Services.HiringProcesses.API.Mappers.HiringProcesses
{
    public class HiringProcessGetRequestMapper : IModelMapper<HiringProcessesSearchCriteria, HiringProcessesSearchCriteriaDTO>
    {
        #region props.

        public static HiringProcessGetRequestMapper Instance { get; } = new HiringProcessGetRequestMapper();

        #endregion       

        #region IModelMapper

        public HiringProcessesSearchCriteria Map(HiringProcessesSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new HiringProcessesSearchCriteria
            {
                Order = (HiringProcessesSearchCriteria.OrderByExpression)from.Order,
                OrderByCultureMode = (SearchCriteria.OrderByCulture)from.OrderByCultureMode,
                OrderByDirection = (SearchCriteria.OrderDirection?)from.OrderByDirection,
                PageNumber = from.PageNumber,
                PageSize = from.PageSize,
                PagingEnabled = from.PagingEnabled,
                Name = from.Name,
                Codes = from.Codes,
                SearchIncludes = from.SearchIncludes.HasValue ? (SearchIncludes)from.SearchIncludes.Value : SearchIncludes.Basic,
                IsActive = from.IsActive,
                Id = from.Id,
            };

            return to;
        }
        public HiringProcessesSearchCriteriaDTO Map(HiringProcessesSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
