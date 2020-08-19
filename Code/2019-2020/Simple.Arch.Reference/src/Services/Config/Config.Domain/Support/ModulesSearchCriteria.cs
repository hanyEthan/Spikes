using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Models;

namespace Mcs.Invoicing.Services.Config.Domain.Support
{
    public class ModulesSearchCriteria : SearchCriteria
    {
        #region criteria.        

        public int? Id { get; set; }
        public string Name { get; set; }
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
