using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Enums;

namespace XCore.Services.Personnel.Models.Settings
{
    public class SettingSearchCriteria : SearchCriteria
    {
        #region criteria.        
        public List<int> SettingIds { get; set; }
        public List<int> AccountIds { get; set; }
        public int? Id { get; set; }
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
