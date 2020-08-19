using Mcs.Invoicing.Services.Config.Domain.Support;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;

namespace Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Queries.AnyConfigItems
{
    public class AnyConfigItemsQueryMapper : IModelMapper<AnyConfigItemsQuery, ConfigItemsSearchCriteria>
    {
        #region IModelMapper

        public ConfigItemsSearchCriteria Map(AnyConfigItemsQuery from, object metadata = null)
        {
            return from;
        }
        public TDestinationAlt Map<TDestinationAlt>(AnyConfigItemsQuery from, object metadata = null) where TDestinationAlt : ConfigItemsSearchCriteria
        {
            return Map(from, metadata) as TDestinationAlt;
        }

        #endregion
    }
}
