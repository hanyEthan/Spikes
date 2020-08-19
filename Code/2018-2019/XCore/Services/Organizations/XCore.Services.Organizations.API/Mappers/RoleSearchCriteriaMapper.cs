using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.API.Models.Department;
using XCore.Services.Organizations.API.Models.Role;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.API.Mappers
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
                Code=from.Code,
                Id=from.Id,
                IncludeRecursive=from.IncludeRecursive,
                IsActive=from.IsActive,
                Name=from.Name,
                Order = (RoleSearchCriteria.OrderByExpression?)from.Order,
                OrderByCultureMode = (SearchCriteria.OrderByCulture)from.OrderByCultureMode,
                OrderByDirection = (SearchCriteria.OrderDirection?)from.OrderByDirection,
                PageNumber = from.PageNumber,
                PageSize = from.PageSize,
                PagingEnabled = from.PagingEnabled,



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
