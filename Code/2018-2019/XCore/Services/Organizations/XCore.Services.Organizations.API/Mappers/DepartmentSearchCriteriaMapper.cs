using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.API.Models.Department;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.API.Mappers
{
    public class DepartmentSearchCriteriaMapper : IModelMapper<DepartmentSearchCriteria, DepartmentSearchCriteriaDTO>
    {
        #region props.

        public static DepartmentSearchCriteriaMapper Instance { get; } = new DepartmentSearchCriteriaMapper();

        #endregion
        #region IModelMapper

        public DepartmentSearchCriteria Map(DepartmentSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new DepartmentSearchCriteria();
           
            to.Code=from.Code;
            to.Ids = from.Ids;
            to.IncludeRecursive = from.IncludeRecursive;
            to.IsActive = from.IsActive;
            to.Name = from.Name;
            to.Order = (DepartmentSearchCriteria.OrderByExpression?)from.Order;
            to.OrderByCultureMode = (SearchCriteria.OrderByCulture)from.OrderByCultureMode;
            to.OrderByDirection = (SearchCriteria.OrderDirection?)from.OrderByDirection;
            to.OrganizationId = from.OrganizationId;
            to.PageNumber = from.PageNumber;
            to.PageSize = from.PageSize;
            to.PagingEnabled = from.PagingEnabled;
            to.ParentDepartmentId = from.ParentDepartmentId;
            

            return to;
        }
        public DepartmentSearchCriteriaDTO Map(DepartmentSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        
        #endregion
    }
}
