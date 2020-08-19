using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Config.API.Models;
using XCore.Services.Config.Core.Models.Support;

namespace XCore.Services.Config.API.Mappers
{
    public class ConfigSearchCriteriaMapper : IModelMapper<ConfigSearchCriteria, ConfigSearchCriteriaDTO>
    {
        #region IModelMapper

        public ConfigSearchCriteria Map(ConfigSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ConfigSearchCriteria()
            {
                ModuleId=from.ModuleId,
                AppId = from.AppId,
                Id = from.Id,
                Name = from.Name,
                Order = (ConfigSearchCriteria.OrderByExpression?)from.Order,
                OrderByCultureMode = (SearchCriteria.OrderByCulture)from.OrderByCultureMode,
                OrderByDirection = (SearchCriteria.OrderDirection?)from.OrderByDirection,
                PageNumber = from.PageNumber,
                PageSize = from.PageSize,
                PagingEnabled = from.PagingEnabled,
                Code=from.Code,
                IsActive=from.IsActive
               
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