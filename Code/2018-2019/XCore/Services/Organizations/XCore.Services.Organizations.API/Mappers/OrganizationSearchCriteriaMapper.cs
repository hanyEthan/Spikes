using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.API.Models.Organization;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.API.Mappers
{
    public class OrganizationSearchCriteriaMapper : IModelMapper<OrganizationSearchCriteria, OrganizationSearchCriteriaDTO>
    {
        #region props.

        public static OrganizationSearchCriteriaMapper Instance { get; } = new OrganizationSearchCriteriaMapper();

        #endregion
        #region IModelMapper

        public OrganizationSearchCriteria Map(OrganizationSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new OrganizationSearchCriteria();

            to.Code = from.Code;
            to.Ids = from.Ids;
            to.IncludeRecursive = from.IncludeRecursive;
            to.IsActive = from.IsActive;
            to.Name = from.Name;
            to.Order = (OrganizationSearchCriteria.OrderByExpression?)from.Order;
            to.OrderByCultureMode = (SearchCriteria.OrderByCulture)from.OrderByCultureMode;
            to.OrderByDirection = (SearchCriteria.OrderDirection?)from.OrderByDirection;
            to.PageNumber = from.PageNumber;
            to.PageSize = from.PageSize;
            to.PagingEnabled = from.PagingEnabled;
            to.DelegateId = from.DelegateId;
            to.DelegatorId = from.DelegatorId;
            to.ParentOrganizationId = from.ParentOrganizationId;
            
            

            return to;
        }
        public OrganizationSearchCriteriaDTO Map(OrganizationSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
