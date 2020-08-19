using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Config.API.Models;
using XCore.Services.Config.Core.Models.Domain;

namespace XCore.Services.Config.API.Mappers
{
    public class ModuleSearchResultsMapper : IModelMapper<SearchResults<Module>, SearchResultsDTO<ModuleDTO>>
    {
        #region props.

        private ModuleMapper ModuleMapper { get; set; } = new ModuleMapper();

        #endregion
        #region IModelMapper

        public SearchResults<Module> Map(SearchResultsDTO<ModuleDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<ModuleDTO> Map(SearchResults<Module> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<ModuleDTO>()
            {
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
                Results = Map(from.Results),
            };

            return to;
        }

        #endregion
        #region helpers.

        public List<ModuleDTO> Map(List<Module> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<ModuleDTO>();

            foreach (var item in from)
            {
                var toItem = this.ModuleMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        #endregion
    }
}
