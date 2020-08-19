using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.API.Models;
using XCore.Services.Organizations.API.Models.Venue;

namespace XCore.Services.Organizations.API.Mappers
{
    public class VenueSearchResultsMapper : IModelMapper<SearchResults<Venue>, SearchResultsDTO<VenueDTO>>
    {
        #region props.

        private VenueMapper VenueMapper { get; set; } = new VenueMapper();
        public static VenueSearchResultsMapper Instance { get; } = new VenueSearchResultsMapper();

        #endregion
        #region IModelMapper

        public SearchResults<Venue> Map(SearchResultsDTO<VenueDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<VenueDTO> Map(SearchResults<Venue> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<VenueDTO>()
            {
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
                Results = Map(from.Results),
            };

            return to;
        }

        #endregion
        #region helpers.

        public List<VenueDTO> Map(List<Venue> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<VenueDTO>();

            foreach (var item in from)
            {
                var toItem = this.VenueMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        #endregion
    }
}
