using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Security.API.Model;
using XCore.Services.Security.Core.Models.Domain;

namespace XCore.Services.Security.API.MTargeters
{
    public class TargetSearchResultsMapper : IModelMapper<SearchResults<Target>, SearchResultsDTO<TargetDTO>>
    {
        #region props.

        private TargetMapper TargetMapper { get; set; } = new TargetMapper();
        public static TargetSearchResultsMapper Instance { get; } = new TargetSearchResultsMapper();
        #endregion
        #region IModelMapper

        public SearchResults<Target> Map(SearchResultsDTO<TargetDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<TargetDTO> Map(SearchResults<Target> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<TargetDTO>()
            {
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
                Results = Map(from.Results),
            };

            return to;
        }

        #endregion
        #region helpers.

        public List<TargetDTO> Map(List<Target> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<TargetDTO>();

            foreach (var item in from)
            {
                var toItem = this.TargetMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        #endregion
    }
}
