using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.API.Models;
using XCore.Services.Organizations.API.Models.Settings;
using XCore.Services.Organizations.Core.Models.Domain;

namespace XCore.Services.Organizations.API.Mappers
{
    public class SettingsSearchResultsMapper : IModelMapper<SearchResults<Settings>, SearchResultsDTO<SettingsDTO>>
    {
        #region props.

        private SettingsMapper SettingsMapper { get; set; } = new SettingsMapper();
        public static SettingsSearchResultsMapper Instance { get; } = new SettingsSearchResultsMapper();

        #endregion
        #region IModelMapper

        public SearchResults<Settings> Map(SearchResultsDTO<SettingsDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<SettingsDTO> Map(SearchResults<Settings> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<SettingsDTO>()
            {
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
                Results = Map(from.Results),
            };

            return to;
        }

        #endregion
        #region helpers.

        public List<SettingsDTO> Map(List<Settings> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<SettingsDTO>();

            foreach (var item in from)
            {
                var toItem = this.SettingsMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        #endregion
    }
}
