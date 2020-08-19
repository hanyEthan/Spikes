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
    public class ClaimSearchCriteriaMapper : IModelMapper<ClaimSearchCriteria, ClaimSearchCriteriaDTO>
    {
        #region props.
        public static ClaimSearchCriteriaMapper Instance { get; } = new ClaimSearchCriteriaMapper();

        #endregion
        #region IModelMapper

        public ClaimSearchCriteria Map(ClaimSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ClaimSearchCriteria()
            {
                Id = from.Id,
                Code = from.Code,
                Key = from.Key,
                IsActive = from.IsActive,
                AppCode = from.AppCode,
                AppId = from.AppId,
                ActorCode = from.ActorCode,
                ActorId = from.ActorId,
                RoleCode = from.RoleCode,
                RoleId = from.RoleId,
                Value = from.Value,
                Map = from.Map,
                Order = (ClaimSearchCriteria.OrderByExpression?)from.Order,
                OrderByCultureMode = (SearchCriteria.OrderByCulture)from.OrderByCultureMode,
                OrderByDirection = (SearchCriteria.OrderDirection?)from.OrderByDirection,
                PageNumber = from.PageNumber,
                PageSize = from.PageSize,
                PagingEnabled = from.PagingEnabled,
                InquiryMode = from.InquiryMode
            };

            return to;
        }
        public ClaimSearchCriteriaDTO Map(ClaimSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
