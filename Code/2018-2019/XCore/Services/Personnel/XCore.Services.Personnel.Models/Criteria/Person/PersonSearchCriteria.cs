using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Enums;

namespace XCore.Services.Personnel.Models.Personnels
{
    public class PersonSearchCriteria : SearchCriteria
    {
        #region criteria.        
        public int? Id { get; set; }
        public List<int> PersonnelIds { get; set; }
        public List<int?> ManagerIds { get; set; }
        public List<int?> DepartmentIds { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; }
        public SearchIncludesEnum SearchIncludes { get; set; }

        #endregion
        #region order.

        public OrderByExpression? Order { get; set; }

        public enum OrderByExpression
        {
            CreationDate = 0,
            Name = 1,
        }

        #endregion
    }
}
