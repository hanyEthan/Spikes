using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Security.SDK.Models.Support
{
    public class TargetSearchCriteriaDTO : SearchCriteria
    {
        #region criteria.        

        public int? AppId { get; set; }
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string AppCode { get; set; }
        public int? PrivilegId { get; set; }
        public string PrivilegCode { get; set; }
        public bool? IsActive { get; set; } = true;

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
