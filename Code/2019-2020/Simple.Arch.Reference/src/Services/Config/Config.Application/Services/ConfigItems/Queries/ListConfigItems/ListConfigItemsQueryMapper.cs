using Mcs.Invoicing.Services.Config.Domain.Support;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;

namespace Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Queries.ListConfigItems
{
    public class ListConfigItemsQueryMapper : IModelMapper<ListConfigItemsQuery, ConfigItemsSearchCriteria>
    {
        #region IModelMapper

        public ConfigItemsSearchCriteria Map(ListConfigItemsQuery from, object metadata = null)
        {
            if (from == null) return null;

            return new ConfigItemsSearchCriteria()
            {
                IsActive = from.IsActive,
                Key = from.Key,
                ModuleId = from.ModuleId,
                Order = from.Order,
                OrderByCultureMode = from.OrderByCultureMode,
                OrderByDirection = from.OrderByDirection,
                PageNumber = from.PageNumber,
                PageSize = from.PageSize,
                PagingEnabled = from.PageSize.HasValue,
            };
        }
        public TDestinationAlt Map<TDestinationAlt>(ListConfigItemsQuery from, object metadata = null) where TDestinationAlt : ConfigItemsSearchCriteria
        {
            return Map(from, metadata) as TDestinationAlt;
        }

        #endregion
    }
}
