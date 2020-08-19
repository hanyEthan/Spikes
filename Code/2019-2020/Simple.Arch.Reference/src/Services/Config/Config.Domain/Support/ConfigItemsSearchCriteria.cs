using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Models;

namespace Mcs.Invoicing.Services.Config.Domain.Support
{
    public class ConfigItemsSearchCriteria : SearchCriteria
    {
        #region criteria.        

        public int? ModuleId { get; set; }
        public string Key { get; set; }
        public bool? IsActive { get; set; } = true;

        #endregion
        #region order.

        public OrderByExpression? Order { get; set; }
        public enum OrderByExpression
        {
            Key = 0,
            CreationDate = 1,
        }

        #endregion
    }
}
