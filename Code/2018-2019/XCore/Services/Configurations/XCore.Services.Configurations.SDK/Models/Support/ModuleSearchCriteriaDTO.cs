using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Configurations.SDK.Models.Support
{
    public class ModuleSearchCriteriaDTO : SearchCriteria
    {
        #region criteria.        

        public int? AppId { get; set; }
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
