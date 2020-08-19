using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Notifications.API.Model;
using XCore.Services.Notifications.Core.Models.Support;

namespace XCore.Services.Notifications.API.Mappers
{
    public class InternalNotificationSearchCriteriaMapper : IModelMapper<InternalNotificationSearchCriteria, InternalNotificationSearchCriteriaDTO>
    {
        #region props.
        public static InternalNotificationSearchCriteriaMapper Instance { get; } = new InternalNotificationSearchCriteriaMapper();
        #endregion
        #region IModelMapper
        public InternalNotificationSearchCriteriaDTO Map(InternalNotificationSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        public InternalNotificationSearchCriteria Map(InternalNotificationSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new InternalNotificationSearchCriteria()
            {
                Id = from.Id,
                Code = from.Code,
               // Name = from.Name,
                IsActive = from.IsActive,

                Order = (InternalNotificationSearchCriteria.OrderByExpression?)from.Order,
                OrderByCultureMode = (SearchCriteria.OrderByCulture)from.OrderByCultureMode,
                OrderByDirection = (SearchCriteria.OrderDirection?)from.OrderByDirection,
                PageNumber = from.PageNumber,
                PageSize = from.PageSize,
                PagingEnabled = from.PagingEnabled,
                InquiryMode = from.InquiryMode

            };

            return to;
        }
        #endregion
    }
}
