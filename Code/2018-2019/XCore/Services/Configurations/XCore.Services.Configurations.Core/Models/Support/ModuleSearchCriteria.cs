using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Configurations.Core.Models.Support
{
    public class ModuleSearchCriteria : SearchCriteria
    {
        #region criteria.        

        public List<int> AppIds { get; set; }
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

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
