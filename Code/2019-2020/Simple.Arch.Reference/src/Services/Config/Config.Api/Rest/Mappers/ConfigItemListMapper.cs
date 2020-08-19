using System;
using System.Collections.Generic;
using Mcs.Invoicing.Services.Config.Api.Rest.Models;
using Mcs.Invoicing.Services.Config.Domain.Entities;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Models;

namespace Mcs.Invoicing.Services.Config.Api.Rest.Mappers
{
    public class ConfigItemListMapper : IModelMapper<SearchResults<ConfigItem>, SearchResults<ConfigItemDTO>>
    {
        #region props.

        private readonly IModelMapper<ConfigItem, ConfigItemDTO> _configItemMapper;

        #endregion
        #region cst.

        public ConfigItemListMapper(IModelMapper<ConfigItem, ConfigItemDTO> configItemMapper)
        {
            this._configItemMapper = configItemMapper;
        }

        #endregion

        #region IModelMapper

        public SearchResults<ConfigItemDTO> Map(SearchResults<ConfigItem> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResults<ConfigItemDTO>()
            {
                Metadata = new SearchResults<ConfigItemDTO>.MetadataHeader()
                {
                    PageIndex = from.Metadata?.PageIndex,
                    PageSize = from.Metadata?.PageSize,
                    TotalCount = (from.Metadata?.TotalCount).GetValueOrDefault(),
                },
                Results = Map(from.Results),
            };

            return to;
        }
        public TDestinationAlt Map<TDestinationAlt>(SearchResults<ConfigItem> from, object metadata = null) where TDestinationAlt : SearchResults<ConfigItemDTO>
        {
            return Map(from, metadata) as TDestinationAlt;
        }

        #endregion
        #region helpers.

        private List<ConfigItemDTO> Map(List<ConfigItem> from)
        {
            if (from == null) return null;

            var to = new List<ConfigItemDTO>();
            from.ForEach(x => to.Add(this._configItemMapper.Map(x)));
            return to;
        }

        #endregion
    }
}
