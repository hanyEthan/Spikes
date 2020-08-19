
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.Core.Models.Search
{
    public class SkillsSearchCriteria : SearchCriteria
    {
        #region criteria.     
        public string Name { get; set; }
        public bool? IsActive { get; set; } = true;
        public int? Id { get; set; }
        public List<string> Codes { get; set; }
        public SearchIncludes SearchIncludes { get; set; }
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
