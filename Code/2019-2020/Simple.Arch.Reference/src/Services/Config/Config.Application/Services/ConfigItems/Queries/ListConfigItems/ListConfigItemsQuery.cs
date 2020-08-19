using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;
using Mcs.Invoicing.Services.Config.Domain.Entities;
using Mcs.Invoicing.Services.Config.Domain.Support;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Models;

namespace Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Queries.ListConfigItems
{
    public class ListConfigItemsQuery : BaseRequestContext, MediatR.IRequest<BaseResponseContext<SearchResults<ConfigItem>>>
    {
        public int? ModuleId { get; set; }
        public string Key { get; set; }
        public bool? IsActive { get; set; } = true;

        public virtual int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public ConfigItemsSearchCriteria.OrderByExpression? Order { get; set; }
        public SearchCriteria.OrderDirection? OrderByDirection { get; set; }
        public SearchCriteria.OrderByCulture OrderByCultureMode { get; set; } = SearchCriteria.OrderByCulture.Default;
    }
}
