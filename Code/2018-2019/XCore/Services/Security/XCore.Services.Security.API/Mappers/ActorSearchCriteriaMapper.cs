using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Security.API.Model;
using XCore.Services.Security.Core.Models.Support;

namespace XCore.Services.Security.API.Mappers
{
    public class ActorSearchCriteriaMapper : IModelMapper<ActorSearchCriteria, ActorSearchCriteriaDTO>
    {
        #region props.
        public static ActorSearchCriteriaMapper Instance { get; } = new ActorSearchCriteriaMapper();

        #endregion
        #region IModelMapper

        public ActorSearchCriteria Map(ActorSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ActorSearchCriteria()
            {
                Id = from.Id,
                Code = from.Code,
                Name = from.Name,
                IsActive = from.IsActive,
                AppCode = from.AppCode,
                AppId = from.AppId,
                RoleCode = from.RoleCode,
                RoleId = from.RoleId,

                Order = (ActorSearchCriteria.OrderByExpression?)from.Order,
                OrderByCultureMode = (SearchCriteria.OrderByCulture)from.OrderByCultureMode,
                OrderByDirection = (SearchCriteria.OrderDirection?)from.OrderByDirection,
                PageNumber = from.PageNumber,
                PageSize = from.PageSize,
                PagingEnabled = from.PagingEnabled,
                InquiryMode = from.InquiryMode
            };

            return to;
        }
        public ActorSearchCriteriaDTO Map(ActorSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
