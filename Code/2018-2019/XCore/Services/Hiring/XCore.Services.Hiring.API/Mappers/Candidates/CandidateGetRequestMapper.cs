using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Hiring.API.Models.Search;
using XCore.Services.Hiring.Core.Models.Search;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.Core.Models;

namespace XCore.Services.Candidates.API.Mappers.Candidates
{
    public class CandidateGetRequestMapper : IModelMapper<CandidatesSearchCriteria, CandidatesSearchCriteriaDTO>
    {
        #region props.

        public static CandidateGetRequestMapper Instance { get; } = new CandidateGetRequestMapper();

        #endregion       

        #region IModelMapper

        public CandidatesSearchCriteria Map(CandidatesSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new CandidatesSearchCriteria
            {
                Apps = from.Apps,
                DateCreatedFrom = from.DateCreatedFrom,
                DateCreatedTo = from.DateCreatedTo,
                Modules = from.Modules,
                Order = (CandidatesSearchCriteria.OrderByExpression)from.Order,
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
        public CandidatesSearchCriteriaDTO Map(CandidatesSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
