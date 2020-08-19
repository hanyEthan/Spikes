using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Config.API.Models;
using XCore.Services.Config.Core.Models.Domain;

namespace XCore.Services.Config.API.Mappers
{
    public class ConfigSearchResultsMapper : IModelMapper<SearchResults<ConfigItem>, SearchResultsDTO<ConfigDTO>>
    {
        #region props.

        private ConfigMapper ConfigMapper { get; set; } = new ConfigMapper();

        #endregion
        #region IModelMapper

        public SearchResults<ConfigItem> Map(SearchResultsDTO<ConfigDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<ConfigDTO> Map(SearchResults<ConfigItem> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<ConfigDTO>()
            {
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
                Results = Map(from.Results),
            };

            return to;
        }

        #endregion
        #region helpers.

        public List<ConfigDTO> Map(List<ConfigItem> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<ConfigDTO>();

            foreach (var item in from)
            {
                var toItem = this.ConfigMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        #endregion
    }
}
