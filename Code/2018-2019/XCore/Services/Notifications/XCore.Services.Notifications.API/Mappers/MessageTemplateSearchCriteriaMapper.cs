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
    public class MessageTemplateSearchCriteriaMapper : IModelMapper<MessageTemplateSearchCriteria, MessageTemplateSearchCriteriaDTO>
    {
        #region props.
        public static MessageTemplateSearchCriteriaMapper Instance { get; } = new MessageTemplateSearchCriteriaMapper();

        #endregion
        #region IModelMapper

        public MessageTemplateSearchCriteriaDTO Map(MessageTemplateSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public MessageTemplateSearchCriteria Map(MessageTemplateSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new MessageTemplateSearchCriteria()
            {
                Id = from.Id,
                Code = from.Code,
                Name = from.Name,
                IsActive = from.IsActive,

                Order = (MessageTemplateSearchCriteria.OrderByExpression?)from.Order,
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
