using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.API.Models.Settings;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.API.Mappers
{
    public class SettingsSearchCriteriaMapper : IModelMapper<SettingsSearchCriteria, SettingsSearchCriteriaDTO>
    {
        #region props.

        public static SettingsSearchCriteriaMapper Instance { get; } = new SettingsSearchCriteriaMapper();

        #endregion
        #region IModelMapper

        public SettingsSearchCriteria Map(SettingsSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SettingsSearchCriteria()
            {
                Code = from.Code,
                IsActive = from.IsActive,
                Order = (SettingsSearchCriteria.OrderByExpression?)from.Order,
                OrderByCultureMode = (SearchCriteria.OrderByCulture)from.OrderByCultureMode,
                OrderByDirection = (SearchCriteria.OrderDirection?)from.OrderByDirection,
                PageNumber = from.PageNumber,
                PageSize = from.PageSize,
                PagingEnabled = from.PagingEnabled,
               Ids=from.Ids,
               OrgainzationId=from.OrgainzationId

            };

            return to;
        }
        public SettingsSearchCriteriaDTO Map(SettingsSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
