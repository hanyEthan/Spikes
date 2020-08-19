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
    public class RoleSearchCriteriaMapper : IModelMapper<RoleSearchCriteria, RoleSearchCriteriaDTO>
    {
        #region props.
        public static RoleSearchCriteriaMapper Instance { get; } = new RoleSearchCriteriaMapper();

        #endregion
        #region IModelMapper

        public RoleSearchCriteria Map(RoleSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new RoleSearchCriteria()
            {
                Id = from.Id,
                Code = from.Code,
                Name = from.Name,
                IsActive = from.IsActive,
                AppCode = from.AppCode,
                AppId = from.AppId,
                ActorCode = from.ActorCode,
                ActorId = from.ActorId,
                PrivilegCode = from.PrivilegCode,
                PrivilegId = from.PrivilegId,

                Order = (RoleSearchCriteria.OrderByExpression?)from.Order,
                OrderByCultureMode = (SearchCriteria.OrderByCulture)from.OrderByCultureMode,
                OrderByDirection = (SearchCriteria.OrderDirection?)from.OrderByDirection,
                PageNumber = from.PageNumber,
                PageSize = from.PageSize,
                PagingEnabled = from.PagingEnabled,
                InquiryMode = from.InquiryMode
            };

            return to;
        }
        public RoleSearchCriteriaDTO Map(RoleSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
