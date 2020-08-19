using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Security.API.Model;
using XCore.Services.Security.Core.Models.Domain;

namespace XCore.Services.Security.API.Mappers
{
    public class ClaimSearchResultsMapper : IModelMapper<SearchResults<Claim>, SearchResultsDTO<ClaimDTO>>
    {
        #region props.

        private ClaimMapper ClaimMapper { get; set; } = new ClaimMapper();
        public static ClaimSearchResultsMapper Instance { get; } = new ClaimSearchResultsMapper();
        #endregion
        #region IModelMapper

        public SearchResults<Claim> Map(SearchResultsDTO<ClaimDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<ClaimDTO> Map(SearchResults<Claim> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<ClaimDTO>()
            {
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
                Results = Map(from.Results),
            };

            return to;
        }

        #endregion
        #region helpers.

        public List<ClaimDTO> Map(List<Claim> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<ClaimDTO>();

            foreach (var item in from)
            {
                var toItem = this.ClaimMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        #endregion
    }
}
