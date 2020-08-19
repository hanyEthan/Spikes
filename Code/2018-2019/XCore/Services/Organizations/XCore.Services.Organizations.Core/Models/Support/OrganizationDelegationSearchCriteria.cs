using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.Core.Models.Support
{
    public  class OrganizationDelegationSearchCriteria : SearchCriteria
    {
        #region criteria.
 
        public int? DelegateId { get; set; }
        public int? DelegatorId { get; set; }
        public bool IncludeRecursive { get; set; } = false;

        public string Code { get; set; }
        public bool? IsActive { get; set; } = true;
        
        #endregion
        #region order.

        public OrderByExpression? Order { get; set; }
        public enum OrderByExpression
        {
            CreationDate = 0,
        }

        #endregion
    }
}
