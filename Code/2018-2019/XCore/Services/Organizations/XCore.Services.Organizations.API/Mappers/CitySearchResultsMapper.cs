using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Organizations.API.Models.City;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.API.Models;

namespace XCore.Services.Organizations.API.Mappers
{
    public class CitySearchResultsMapper : IModelMapper<SearchResults<City>, SearchResultsDTO<CityDTO>>
    {
        #region props.

        private CityMapper CityMapper { get; set; } = new CityMapper();
        public static CitySearchResultsMapper Instance { get; } = new CitySearchResultsMapper();

        #endregion
        #region IModelMapper

        public SearchResults<City> Map(SearchResultsDTO<CityDTO> from, object metadata = null)
        {
            throw new NotImplementedException();
        }
        public SearchResultsDTO<CityDTO> Map(SearchResults<City> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SearchResultsDTO<CityDTO>()
            {
                PageIndex = from.PageIndex,
                TotalCount = from.TotalCount,
                Results = Map(from.Results),
            };

            return to;
        }

        #endregion
        #region helpers.

        public List<CityDTO> Map(List<City> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<CityDTO>();

            foreach (var item in from)
            {
                var toItem = this.CityMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        #endregion
    }
}
