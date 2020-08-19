using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Audit.API.Models;
using XCore.Services.Audit.Core.Models;
using static XCore.Services.Audit.Core.Models.AuditSearchCriteria;

namespace XCore.Services.Audit.API.Mappers
{
    public class AuditGetRequestMapper : IModelMapper<AuditSearchCriteria, AuditSearchCriteriaDTO>
    {
        #region props.

        public static AuditGetRequestMapper Instance { get; } = new AuditGetRequestMapper();

        #endregion
        #region cst.

        static AuditGetRequestMapper()
        {
        }
        public AuditGetRequestMapper()
        {
        }

        #endregion

        #region IModelMapper

        public AuditSearchCriteria Map(AuditSearchCriteriaDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new AuditSearchCriteria();

            to.Actions = (from.Actions?.Any() ?? false) ? from.Actions : null;
            to.Apps = (from.Apps?.Any() ?? false) ? null : from.Apps;
            to.Modules = (from.Modules?.Any() ?? false) ? from.Modules : null;
            to.Entities = (from.Entities?.Any() ?? false) ? from.Entities : null;
            to.Text = from?.Text;
            to.UserIds = (from.UserIds?.Any() ?? false) ? from.UserIds : null;
            to.UserNames = (from.UserNames?.Any() ?? false) ? from.UserNames : null;

            to.Order = MapOrder(from.Order);
            to.OrderByCultureMode = MapOrderByCulture(from.OrderByCultureMode);
            to.OrderByDirection = MapOrderByDirection(from.OrderByDirection);
            to.PageNumber = from.PageNumber;
            to.PageSize = from.PageSize;
            to.PagingEnabled = from.PagingEnabled;

            return to;
        }
        public AuditSearchCriteriaDTO Map(AuditSearchCriteria from, object metadata = null)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region helpers.

        private OrderByExpression? MapOrder(int? from)
        {
            return (OrderByExpression?) from;
        }
        private SearchCriteria.OrderByCulture MapOrderByCulture(int? from)
        {
            return (SearchCriteria.OrderByCulture?) from ?? SearchCriteria.OrderByCulture.Default;
        }
        private SearchCriteria.OrderDirection MapOrderByDirection(int? from)
        {
            return (SearchCriteria.OrderDirection?) from ?? SearchCriteria.OrderDirection.Ascending;
        }

        #endregion
    }
}
