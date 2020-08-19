using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Configurations.Core.Models.Domain;
using XCore.Services.Configurations.Models;

namespace XCore.Services.Configurations.Mappers
{
    public class ModuleSearchResultsMapper : IModelMapper<SearchResults<Module>, SearchResultsDTO<ModuleDTO>>
    {
        #region props.

        private ModuleMapper ModuleMapper { get; set; } = ModuleMapper.Instance;
        public static ModuleSearchResultsMapper Instance { get; } = new ModuleSearchResultsMapper();

        #endregion
        #region IModelMapper

        public SearchResults<Module> Map(SearchResultsDTO<ModuleDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<ModuleDTO> Map(SearchResults<Module> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<ModuleDTO>();

            to.PageIndex = from.PageIndex;
            to.TotalCount = from.TotalCount;
            to.Results = Map(from.Results);

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
                var toItem = ModuleMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        #endregion
    }
}
