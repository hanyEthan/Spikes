using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Configurations.Core.Models.Support;
using XCore.Services.Configurations.Models;

namespace XCore.Services.Configurations.Mappers
{
    public class ConfigSearchCriteriaMapper : IModelMapper<ConfigSearchCriteria, ConfigSearchCriteriaDTO>
    {
        #region props.

        public static ConfigSearchCriteriaMapper Instance { get; } = new ConfigSearchCriteriaMapper();

        #endregion
        #region IModelMapper

        public ConfigSearchCriteria Map(ConfigSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ConfigSearchCriteria()
            {
                ModuleIds = from.ModuleIds,
                AppIds = from.AppIds,
                Id = from.Id,
                Name = from.Name,
                Order = (ConfigSearchCriteria.OrderByExpression?)from.Order,
                OrderByCultureMode = (SearchCriteria.OrderByCulture)from.OrderByCultureMode,
                OrderByDirection = (SearchCriteria.OrderDirection?)from.OrderByDirection,
                PageNumber = from.PageNumber,
                PageSize = from.PageSize,
                Keys=from.Keys,
                PagingEnabled = from.PagingEnabled,
                Code = from.Code,
                IsActive = from.IsActive

            };

            return to;
        }
        public ConfigSearchCriteriaDTO Map(ConfigSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}