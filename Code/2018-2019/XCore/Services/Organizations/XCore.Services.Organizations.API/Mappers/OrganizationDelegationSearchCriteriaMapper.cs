using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.API.Models.OrganizationDelegation;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.API.Mappers
{
    public class OrganizationDelegationSearchCriteriaMapper : IModelMapper<OrganizationDelegationSearchCriteria, OrganizationDelegationSearchCriteriaDTO>
    {
        #region props.

        public static OrganizationDelegationSearchCriteriaMapper Instance { get; } = new OrganizationDelegationSearchCriteriaMapper();

        #endregion
        #region IModelMapper

        public OrganizationDelegationSearchCriteria Map(OrganizationDelegationSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new OrganizationDelegationSearchCriteria()
            {
                Code = from.Code,
                IsActive = from.IsActive,
                Order = (OrganizationDelegationSearchCriteria.OrderByExpression?)from.Order,
                OrderByCultureMode = (SearchCriteria.OrderByCulture)from.OrderByCultureMode,
                OrderByDirection = (SearchCriteria.OrderDirection?)from.OrderByDirection,
                PageNumber = from.PageNumber,
                PageSize = from.PageSize,
                PagingEnabled = from.PagingEnabled,
                DelegateId=from.OrganizationDelegateId,
                DelegatorId=from.OrganizationDelegatorId
            
            };

            return to;
        }
        public OrganizationDelegationSearchCriteriaDTO Map(OrganizationDelegationSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
