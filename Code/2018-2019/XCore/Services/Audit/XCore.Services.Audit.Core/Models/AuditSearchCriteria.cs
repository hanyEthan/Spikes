using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Audit.Core.Models
{
    public class AuditSearchCriteria : SearchCriteria
    {
        #region criteria.        

        public List<string> UserIds { get; set; }
        public List<string> UserNames { get; set; }
        public List<string> Apps { get; set; }
        public List<string> Modules { get; set; }
        public List<string> Actions { get; set; }
        public List<string> Entities { get; set; }
        public string Text { get; set; }

        #endregion
        #region order.

        public OrderByExpression? Order { get; set; }
        public enum OrderByExpression
        {
            CreationDate = 0,
        }

        #endregion
    }
}
