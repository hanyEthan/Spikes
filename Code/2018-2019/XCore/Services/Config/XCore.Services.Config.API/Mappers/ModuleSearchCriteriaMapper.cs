using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Config.API.Models;
using XCore.Services.Config.Core.Models.Support;

namespace XCore.Services.Config.API.Mappers
{
    public class ModuleSearchCriteriaMapper : IModelMapper<ModuleSearchCriteria, ModuleSearchCriteriaDTO>
    {
        #region IModelMapper

        public ModuleSearchCriteria Map(ModuleSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ModuleSearchCriteria()
            {
                AppId=from.AppId,
                Id = from.Id,
                Name = from.Name,
                Order = (ModuleSearchCriteria.OrderByExpression?)from.Order,
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
        public ModuleSearchCriteriaDTO Map(ModuleSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
