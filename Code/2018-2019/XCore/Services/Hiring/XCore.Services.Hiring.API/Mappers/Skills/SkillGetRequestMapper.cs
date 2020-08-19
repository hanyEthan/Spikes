using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Hiring.API.Models.Search;
using XCore.Services.Hiring.Core.Models.Search;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.Core.Models;

namespace XCore.Services.Skills.API.Mappers.Skills
{
    public class SkillGetRequestMapper : IModelMapper<SkillsSearchCriteria, SkillsSearchCriteriaDTO>
    {
        #region props.

        public static SkillGetRequestMapper Instance { get; } = new SkillGetRequestMapper();

        #endregion       

        #region IModelMapper

        public SkillsSearchCriteria Map(SkillsSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SkillsSearchCriteria
            {
                Order = (SkillsSearchCriteria.OrderByExpression)from.Order,
                OrderByCultureMode = (SearchCriteria.OrderByCulture)from.OrderByCultureMode,
                OrderByDirection = (SearchCriteria.OrderDirection?)from.OrderByDirection,
                PageNumber = from.PageNumber,
                PageSize = from.PageSize,
                PagingEnabled = from.PagingEnabled,
                Name = from.Name,
                Codes = from.Codes,
                IsActive = from.IsActive,
                SearchIncludes = from.SearchIncludes.HasValue ? (SearchIncludes)from.SearchIncludes.Value : SearchIncludes.Basic,
                Id = from.Id,

            };

            return to;
        }
        public SkillsSearchCriteriaDTO Map(SkillsSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
