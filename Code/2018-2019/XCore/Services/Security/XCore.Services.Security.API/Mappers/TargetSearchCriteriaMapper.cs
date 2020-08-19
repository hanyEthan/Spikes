using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Security.API.Model;
using XCore.Services.Security.Core.Models.Support;

namespace XCore.Services.Security.API.MTargeters
{
    public class TargetSearchCriteriaMapper : IModelMapper<TargetSearchCriteria, TargetSearchCriteriaDTO>
    {
        #region props.
        public static TargetSearchCriteriaMapper Instance { get; } = new TargetSearchCriteriaMapper();

        #endregion
        #region IModelMapper

        public TargetSearchCriteria Map(TargetSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new TargetSearchCriteria()
            {
                Id = from.Id,
                Code = from.Code,
                Name = from.Name,
                IsActive = from.IsActive,
                AppCode = from.AppCode,
                AppId = from.AppId,
                PrivilegCode = from.PrivilegCode,
                PrivilegId = from.PrivilegId,

                Order = (TargetSearchCriteria.OrderByExpression?)from.Order,
                OrderByCultureMode = (SearchCriteria.OrderByCulture)from.OrderByCultureMode,
                OrderByDirection = (SearchCriteria.OrderDirection?)from.OrderByDirection,
                PageNumber = from.PageNumber,
                PageSize = from.PageSize,
                PagingEnabled = from.PagingEnabled,
                InquiryMode = from.InquiryMode
            };

            return to;
        }
        public TargetSearchCriteriaDTO Map(TargetSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
