using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Configurations.Core.Models.Support;
using XCore.Services.Configurations.Models;

namespace XCore.Services.Configurations.Mappers
{
    public class AppSearchCriteriaMapper : IModelMapper<AppSearchCriteria, AppSearchCriteriaDTO>
    {
        #region props.

        public static AppSearchCriteriaMapper Instance { get; } = new AppSearchCriteriaMapper();

        #endregion
        #region IModelMapper

        public AppSearchCriteria Map(AppSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new AppSearchCriteria()
            {
                Id = from.Id,
                Code = from.Code,
                Name = from.Name,
                IsActive = from.IsActive,

                Order = (AppSearchCriteria.OrderByExpression?)from.Order,
                OrderByCultureMode = (SearchCriteria.OrderByCulture)from.OrderByCultureMode,
                OrderByDirection = (SearchCriteria.OrderDirection?)from.OrderByDirection,
                PageNumber = from.PageNumber,
                PageSize = from.PageSize,
                PagingEnabled = from.PagingEnabled,
            };

            return to;
        }
        public AppSearchCriteriaDTO Map(AppSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}                    
