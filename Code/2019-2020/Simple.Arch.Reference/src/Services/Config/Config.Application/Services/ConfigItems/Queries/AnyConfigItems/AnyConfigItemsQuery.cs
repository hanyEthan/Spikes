using Mcs.Invoicing.Services.Config.Domain.Support;

namespace Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Queries.AnyConfigItems
{
    public class AnyConfigItemsQuery : ConfigItemsSearchCriteria, MediatR.IRequest<bool>
    {
    }
}
