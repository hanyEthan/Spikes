using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Config.API.Models;
using XCore.Services.Config.Core.Models.Domain;

namespace XCore.Services.Config.API.Mappers
{
    public class AppSearchResultsMapper : IModelMapper<SearchResults<App>, SearchResultsDTO<AppDTO>>
    {
        #region props.

        private AppMapper AppMapper { get; set; } = new AppMapper();

        #endregion
        #region IModelMapper

        public SearchResults<App> Map(SearchResultsDTO<AppDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<AppDTO> Map(SearchResults<App> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<AppDTO>()
            {
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
                Results = Map(from.Results),
            };

            return to;
        }

        #endregion
        #region helpers.

        public List<AppDTO> Map(List<App> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<AppDTO>();

            foreach (var item in from)
            {
                var toItem = this.AppMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        #endregion
    }
}
