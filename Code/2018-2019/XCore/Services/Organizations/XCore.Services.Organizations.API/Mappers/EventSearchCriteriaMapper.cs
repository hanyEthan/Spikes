using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.API.Models.Department;
using XCore.Services.Organizations.API.Models.Event;
using XCore.Services.Organizations.API.Models.Role;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.API.Mappers
{
    public class EventSearchCriteriaMapper : IModelMapper<EventSearchCriteria, EventSearchCriteriaDTO>
    {
        #region props.

        public static EventSearchCriteriaMapper Instance { get; } = new EventSearchCriteriaMapper();

        #endregion
        #region IModelMapper

        public EventSearchCriteria Map(EventSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new EventSearchCriteria()
            {
                Code=from.Code,
                Ids=from.Ids,
                IncludeRecursive=from.IncludeRecursive,
                IsActive=(bool)from.IsActive,
                Name=from.Name,
                Order = (EventSearchCriteria.OrderByExpression?)from.Order,
                OrderByCultureMode = (SearchCriteria.OrderByCulture)from.OrderByCultureMode,
                OrderByDirection = (SearchCriteria.OrderDirection?)from.OrderByDirection,
                PageNumber = from.PageNumber,
                PageSize = from.PageSize,
                PagingEnabled = from.PagingEnabled,



            };

            return to;
        }
        public EventSearchCriteriaDTO Map(EventSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        
        #endregion
    }
}
