using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Config.SDK.Models.Support
{
    public class AppSearchCriteriaDTO : SearchCriteria
    {
        #region criteria.        

        public int? Id { get; set; }
        public string Name { get; set; }

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
