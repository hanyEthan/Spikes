using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.Core.Models.Support
{
    public class OrganizationSearchCriteria: SearchCriteria
    {
        #region criteria.        

        public List<int> Ids { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; } = true;
        public int? ParentOrganizationId { get; set; }
        public int? DelegateId { get; set; }
        public int? DelegatorId { get; set; }
        public bool IncludeRecursive { get; set; } = false;

        #endregion
        #region order.

        public OrderByExpression? Order { get; set; }
        public enum OrderByExpression
        {
            Name = 0,
            CreationDate = 1,
        }

        #endregion
    }
}
