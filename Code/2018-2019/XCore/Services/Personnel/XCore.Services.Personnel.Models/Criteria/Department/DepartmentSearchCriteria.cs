using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Enums;

namespace XCore.Services.Personnel.Models.Departments
{
    public class DepartmentSearchCriteria : SearchCriteria
    {
        #region criteria.        

        public List<string> DepartmentReferenceId { get; set; }
        public List<int?> HeadDepartmentId { get; set; }
        public string Name { get; set; }
        public int? Id { get; set; }
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
            DepartmentReferenceId = 2,
            HeadDepartmentId = 3
        }

        #endregion
    }
}
